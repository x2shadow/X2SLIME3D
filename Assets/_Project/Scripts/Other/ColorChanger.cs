using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace X2SLIME3D
{
    public class ColorChanger : MonoBehaviour
    {
        ColorPalette palette;

        [Header("Палитра цветов")]
        public Color characterColor;
        public Color characterJumpColor;
        public Color checkpointColor;
        public Color checkpointFlagColor;
        public Color platformColor;
        public Color platformOutsideColor;
        public Color platformInsideColor;
        public Color bouncyCubeOutsideColor;
        public Color bouncyCubeInsideColor;
        public Color waterColor;
        [Range(0f, 1f)]
        public float waterAlpha;
        public Color gradientTopColor;
        public Color gradientBottomColor;

        private void Awake()
        {
            palette = Resources.Load<ColorPalette>("ColorPalette");
            ApplyColors();
        }

        private void ApplyColors()
        {
            palette.characterColor                  = characterColor;
            palette.characterJumpColor              = characterJumpColor;
            palette.checkpointMaterial.color        = checkpointColor;
            palette.checkpointFlagMaterial.color    = checkpointFlagColor;
            palette.platformMaterial.color          = platformColor;
            palette.platformOutsideMaterial.color   = platformOutsideColor;
            palette.platformInsideMaterial.color    = platformInsideColor;
            palette.bouncyCubeOutsideMaterial.color = bouncyCubeOutsideColor;
            palette.bouncyCubeInsideMaterial.color  = bouncyCubeInsideColor;
            
            waterColor.a = waterAlpha;
            palette.waterMaterial.color             = waterColor;
            
            palette.gradientSkybox.SetColor("_TopColor",    gradientTopColor);
            palette.gradientSkybox.SetColor("_BottomColor", gradientBottomColor);
        }

        private void OnValidate()
        {
            palette = Resources.Load<ColorPalette>("ColorPalette");
            ApplyColors();
        }
    }
}
