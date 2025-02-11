using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace X2SLIME3D
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GamePresenter>();
        }
    }
}
