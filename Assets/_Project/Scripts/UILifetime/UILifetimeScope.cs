using VContainer;
using VContainer.Unity;
using UnityEngine;

namespace X2SLIME3D
{
    public class UILifetimeScope : LifetimeScope
    {
        [SerializeField] UIView uiView;
        [SerializeField] InputReader inputReader;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<UIPresenter>(Lifetime.Scoped);
            builder.RegisterComponent(uiView);
            builder.RegisterInstance(inputReader);
        }
    }
}
