using UnityEngine;

namespace Damrem.UnityEngine.Rendering.Universal {
    public static class LitShaderMaterialExt {
        public static void SetLitBaseColor(this Material material, Color color) {
            material.SetColor("_BaseColor", color);
        }
    }
}