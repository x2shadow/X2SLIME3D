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
        //readonly AudioService audioService;
        readonly JumpConfig jumpConfig;

        readonly ReactiveProperty<float> pressStartTime = new ReactiveProperty<float>();
        private readonly ReactiveProperty<bool> jumpEligible = new ReactiveProperty<bool>(false);
        private bool isJumpButtonHeld = false; // Флаг удержания кнопки в воздухе

        PlayerPresenter(PlayerView playerView, PlayerService playerService, InputReader inputReader, JumpConfig jumpConfig)//,
                        //AudioService audioService)
        {
            this.playerView    = playerView;
            this.playerService = playerService;
            this.inputReader   = inputReader;
            this.jumpConfig    = jumpConfig;
            //this.audioService  = audioService;
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
                        //audioService.PlaySound(audioView.soundJumpIn);
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

                    if (playerView.IsGrounded) playerView.Jump(forwardForce, jumpForce);
                    
                    jumpEligible.Value = false;

                    playerView.SetChargingJump(false);
                    //Debug.Log($"audioView: {audioView}");
                    //Debug.Log($"soundJump: {audioView.soundJump}");
                    //Debug.Log($"audioService: {audioService}");
                    //audioService.PlayJumpSound();
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
