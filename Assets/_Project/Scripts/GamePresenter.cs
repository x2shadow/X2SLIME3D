using System;
using System.ComponentModel.Design;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using R3;

namespace X2SLIME3D
{
    public class GamePresenter : IStartable, IDisposable
    {
        readonly CompositeDisposable disposable = new CompositeDisposable();

        GamePresenter()
        {
        }

        public void Start()
        {
        }

        public void Dispose() => disposable.Dispose();
    }
}
