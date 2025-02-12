using UnityEngine;
using UnityEngine.EventSystems;

namespace X2SLIME3D
{
    public class UIChecker : MonoBehaviour
    {
        public static bool IsPointerOverUI { get; private set; }

        void Update()
        {
            IsPointerOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }
    }
}
