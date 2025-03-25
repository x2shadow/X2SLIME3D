using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace X2SLIME3D
{
    [CreateAssetMenu(fileName = "ColorPalette", menuName = "X2SLIME3D/ColorPalette")]
    public class ColorPalette : ScriptableObject
    {
        public Color    characterColor;
        public Color    characterJumpColor;
        public Material checkpointMaterial;
        public Material checkpointFlagMaterial;
        public Material platformMaterial;
        public Material platformOutsideMaterial;
        public Material platformInsideMaterial;
        public Material bouncyCubeOutsideMaterial;
        public Material bouncyCubeInsideMaterial;
        public Material waterMaterial;
        public Material gradientSkybox;
    }
}
