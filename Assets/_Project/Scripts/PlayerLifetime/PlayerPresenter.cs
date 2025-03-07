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
        readonly AudioService audioService;

        readonly ReactiveProperty<float> pressStartTime = new ReactiveProperty<float>();
        readonly ReactiveProperty<bool> jumpEligible = new ReactiveProperty<bool>(false);
        bool isJumpButtonHeld = false; // Флаг удержания кнопки в воздухе
        bool jumpInPlayed = false;     // Флаг: уже запущен звук JumpIn в текущей попытке прыжка

        PlayerPresenter(PlayerView playerView, PlayerService playerService, InputReader inputReader, AudioService audioService)
        {
            this.playerView    = playerView;
            this.playerService = playerService;
            this.inputReader   = inputReader;
            this.audioService  = audioService;
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
                        audioService.PlaySoundJumpIn();
                        jumpInPlayed = true;
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

                    jumpEligible.Value = false;
                    
                    playerView.SetChargingJump(false);
                    audioService.StopSoundJumpIn();
                    jumpInPlayed = false;

                    if (playerView.IsGrounded) { playerView.Jump(forwardForce, jumpForce);  audioService.PlaySound(SoundType.Jump); }
                })
                .AddTo(disposable);

            // Реакция на приземление (если кнопка зажата в воздухе)
            playerView.IsGroundedObservable
                .Where(isGrounded => isGrounded) // Срабатывает только при приземлении
                .Subscribe(_ =>
                {
                    if (isJumpButtonHeld && !jumpInPlayed)
                    {
                        pressStartTime.Value = Time.time;
                        jumpEligible.Value = true;

                        playerView.SetChargingJump(true);
                        audioService.PlaySoundJumpIn();
                        jumpInPlayed = true;
                    }
                })
                .AddTo(disposable);

            playerView.OnCollisionImpact
                .Subscribe(_ => audioService.PlaySoundCollision())
                .AddTo(disposable);
        }

        public void Dispose() => disposable.Dispose();
    }
}
