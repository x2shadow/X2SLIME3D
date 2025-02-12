using R3;
using UnityEngine;

namespace X2SLIME3D
{
    public class LevelFinish : MonoBehaviour
    {
        // Используем Subject, чтобы уведомлять о завершении уровня
        public readonly Subject<Unit> OnLevelFinished = new Subject<Unit>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Передаём событие завершения уровня
                OnLevelFinished.OnNext(Unit.Default);
            }
        }

        private void OnDestroy()
        {
            OnLevelFinished.OnCompleted();
        }
    }
}
