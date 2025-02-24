using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace X2SLIME3D
{
    public class LevelRestart : MonoBehaviour
    {
        public readonly Subject<Unit> OnLevelRestarted = new Subject<Unit>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnLevelRestarted.OnNext(Unit.Default);
            }
        }

        private void OnDestroy()
        {
            OnLevelRestarted.OnCompleted();
        }
    }
}
