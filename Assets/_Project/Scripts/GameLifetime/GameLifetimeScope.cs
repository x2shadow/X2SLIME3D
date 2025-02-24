using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace X2SLIME3D
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] PlayerView playerView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GamePresenter>();
            builder.Register<UIService>(Lifetime.Singleton);
            builder.RegisterComponent(playerView);
        }
    }
}
