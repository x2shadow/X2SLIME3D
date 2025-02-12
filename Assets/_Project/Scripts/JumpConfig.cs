using UnityEngine;

namespace X2SLIME3D
{
    [CreateAssetMenu(fileName = "JumpConfig", menuName = "X2SLIME3D/JumpConfig")]
    public class JumpConfig : ScriptableObject
    {
        public float minJumpForce = 5f;
        public float maxJumpForce = 15f;
        public float maxPressTime = 1f;
        public float horizontalSpeed = 1f;
    }
}
