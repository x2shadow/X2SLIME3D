using UnityEngine;
using UnityEngine.Audio;
using VContainer;
using VContainer.Unity;

namespace X2SLIME3D
{
    public class AudioLifetimeScope : LifetimeScope
    {
        [SerializeField] AudioMixer audioMixer;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<AudioPresenter>(Lifetime.Scoped);
            builder.RegisterInstance(audioMixer);
        }
    }
}
