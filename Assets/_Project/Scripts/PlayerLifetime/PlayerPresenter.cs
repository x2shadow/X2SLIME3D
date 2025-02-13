using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;
using VContainer.Unity;

namespace X2SLIME3D
{
    public class PlayerPresenter : IStartable, IDisposable
    {
        readonly CompositeDisposable disposable = new CompositeDisposable();

        readonly PlayerView playerView;
        readonly PlayerService playerService;
        readonly InputReader inputReader;
        readonly JumpConfig jumpConfig;

        readonly ReactiveProperty<float> pressStartTime = new ReactiveProperty<float>();
        private readonly ReactiveProperty<bool> jumpEligible = new ReactiveProperty<bool>(false);
        private bool isJumpButtonHeld = false; // Флаг удержания кнопки в воздухе

        PlayerPresenter(PlayerView playerView, PlayerService playerService, InputReader inputReader, JumpConfig jumpConfig)
        {
            this.playerView    = playerView;
            this.playerService = playerService;
            this.inputReader   = inputReader;
            this.jumpConfig    = jumpConfig;
        }

        public void Start()
        {
            inputReader.EnablePlayerActions();
            
            inputReader.JumpStart
                .AsObservable()
                .Subscribe(_ => 
                {
                    pressStartTime.Value = Time.time;
                    isJumpButtonHeld = true;


                    if (playerView.IsGrounded)
                    {
                        jumpEligible.Value = true;
                        playerView.SetChargingJump(true);
                    }
                })
                .AddTo(disposable);

            inputReader.JumpEnd
                .AsObservable()
                .Subscribe(_ =>
                {
                    isJumpButtonHeld = false;

                    if (!jumpEligible.Value) return;

                    float pressDuration = Time.time - pressStartTime.Value;
                    Debug.Log("pressDuration:" + pressDuration);

                    float jumpForce    = playerService.CalculateJumpForce(pressDuration);
                    float forwardForce = playerService.CalculateForwardForce(pressDuration);

                    playerView.Jump(forwardForce, jumpForce);
                    
                    jumpEligible.Value = false;

                    playerView.SetChargingJump(false);
                })
                .AddTo(disposable);

            // Реакция на приземление (если кнопка зажата в воздухе)
            playerView.IsGroundedObservable
                .Where(isGrounded => isGrounded) // Срабатывает только при приземлении
                .Subscribe(_ =>
                {
                    if (isJumpButtonHeld)
                    {
                        pressStartTime.Value = Time.time;
                        jumpEligible.Value = true;

                        playerView.SetChargingJump(true);
                    }
                })
                .AddTo(disposable);
        }

        public void Dispose() => disposable.Dispose();
    }
}
