using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace X2SLIME3D
{
    public class PlatformMover : MonoBehaviour
    {
        Vector3 startPosition; 
        Vector3 endPosition;   

        [SerializeField] Transform target;

        [SerializeField] private float moveDuration = 2f; // Время движения
        [SerializeField] private Ease moveEase = Ease.Linear; // Тип анимации
        [SerializeField] private bool loop = true; // Должна ли платформа двигаться бесконечно

        void Start()
        {
            startPosition = transform.position;
            endPosition   = target.position;

            target.gameObject.SetActive(false);

            MovePlatform().Forget(); // Запускаем асинхронное движение
        }

        async UniTask MovePlatform()
        {
            while (loop) // Бесконечный цикл, если включен `loop`
            {
                if (this == null || !gameObject.activeInHierarchy) return;

                await transform.DOMove(endPosition, moveDuration)
                            .SetEase(moveEase)
                            .AsyncWaitForCompletion();

                if (this == null || !gameObject.activeInHierarchy) return;
                
                await transform.DOMove(startPosition, moveDuration)
                            .SetEase(moveEase)
                            .AsyncWaitForCompletion();
            }
        }

        void OnDrawGizmos()
        {
            if (target != null)
            {
                // Установить цвет Gizmos для платформы
                Gizmos.color = Color.cyan;

                // Если игра не запущена, используем позицию target
                Vector3 positionToDraw = Application.isPlaying ? endPosition : target.position;

                Gizmos.DrawWireCube(positionToDraw, new Vector3(target.localScale.x * 2, target.localScale.y, target.localScale.z));
            }
        }

        void OnDestroy()
        {
            transform.DOKill();
        }
    }
}
