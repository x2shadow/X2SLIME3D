using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace X2SLIME3D
{
    public class PlayerService
    {
        const float minJumpForce = 5f;
        const float maxJumpForce = 15f;
        const float maxPressTime = 1f;

        public float CalculateJumpForce(float pressDuration)
        {
            float time = Mathf.Clamp01(pressDuration / maxPressTime);
            return Mathf.Lerp(minJumpForce, maxJumpForce, time);
        }
    }
}
