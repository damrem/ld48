using UnityEngine;

namespace Damrem.UnityEngine {
    public static class LayerMaskExt {
        public static LayerMask GetMask(this GameObject go) {
            return (int)Mathf.Pow(2, go.layer);
        }
    }
}

