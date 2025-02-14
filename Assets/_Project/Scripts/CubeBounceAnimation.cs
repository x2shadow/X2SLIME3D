using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CubeBounceAnimation : MonoBehaviour
{
    [SerializeField] float scaleFactor = 1.2f;      // Во сколько раз увеличится кубик
    [SerializeField] float animationDuration = 0.2f;  // Длительность каждого этапа анимации
    [SerializeField] Ease ease = Ease.OutBounce;

    Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            AnimateBounce().Forget(); // Запускаем анимацию асинхронно без ожидания
        }
    }

    async UniTask AnimateBounce()
    {
        if (this == null || !gameObject.activeInHierarchy) return;

        // Увеличение размера
        await transform.DOScale(originalScale * scaleFactor, animationDuration)
                       .SetEase(ease)
                       .AsyncWaitForCompletion();

        if (this == null || !gameObject.activeInHierarchy) return;

        // Возвращение к исходному размеру
        await transform.DOScale(originalScale, animationDuration)
                       .SetEase(ease)
                       .AsyncWaitForCompletion();
    }

    void OnDestroy()
    {
        transform.DOKill();
    }
}
