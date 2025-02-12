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
                .Subscribe(_ => pressStartTime.Value = Time.time)
                .AddTo(disposable);

            inputReader.JumpEnd
                .AsObservable()
                .Subscribe(_ =>
                {
                    float pressDuration = Time.time - pressStartTime.Value;
                    float jumpForce = playerService.CalculateJumpForce(pressDuration);
                    playerView.Jump(jumpConfig.horizontalSpeed, jumpForce);
                })
                .AddTo(disposable);
        }

        public void Dispose() => disposable.Dispose();
    }
}
