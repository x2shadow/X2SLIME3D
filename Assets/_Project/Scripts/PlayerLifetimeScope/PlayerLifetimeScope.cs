using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace X2SLIME3D
{
    public class PlayerLifetimeScope : LifetimeScope
    {
        [SerializeField] PlayerView playerView;
        [SerializeField] InputReader inputReader;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PlayerPresenter>(Lifetime.Scoped);
            builder.Register<PlayerService>(Lifetime.Scoped);
            builder.RegisterComponent(playerView);
            builder.RegisterInstance(inputReader);
        }
    }
}
