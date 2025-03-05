using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace X2SLIME3D
{
    public class PlayerLifetimeScope : LifetimeScope
    {
        [SerializeField] InputReader inputReader;
        [SerializeField] JumpConfig jumpConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PlayerPresenter>(Lifetime.Scoped);
            builder.Register<PlayerService>(Lifetime.Scoped);
            builder.RegisterInstance(inputReader);
            builder.RegisterInstance(jumpConfig);
        }
    }
}
