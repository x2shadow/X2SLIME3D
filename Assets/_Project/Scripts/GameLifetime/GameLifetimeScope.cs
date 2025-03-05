using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace X2SLIME3D
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] PlayerView playerView;
        [SerializeField] AudioView audioView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GamePresenter>();
            builder.Register<UIService>(Lifetime.Singleton);
            builder.RegisterComponent(playerView);
            builder.RegisterComponent(audioView);
            builder.Register<AudioService>(Lifetime.Singleton);
        }
    }
}
