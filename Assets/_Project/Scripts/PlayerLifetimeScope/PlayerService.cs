using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace X2SLIME3D
{
    public class PlayerService
    {
        private readonly JumpConfig jumpConfig;
        
        public PlayerService(JumpConfig jumpConfig)
        {
            this.jumpConfig = jumpConfig;
        }

        public float CalculateJumpForce(float pressDuration)
        {
            float time = Mathf.Clamp01(pressDuration / jumpConfig.maxPressTime);
            return Mathf.Lerp(jumpConfig.minJumpForce, jumpConfig.maxJumpForce, time);
        }
    }
}
