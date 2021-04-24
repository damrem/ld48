using System.Collections;
using UnityEngine;
using System;

namespace Damrem.Grids.Irregular {

    public class RelaxationAnimator : MonoBehaviour {
        public event Action OnRelaxed;

        [SerializeField] float AnimationDurationSeconds = 1;

        Relaxation Relaxation;

        public void Init(Relaxation relaxation) {
            Relaxation = relaxation;
        }

        public void Animate(float strength, int iterationCount, bool freezeBorders = false) {
            Animate(strength, iterationCount, freezeBorders, AnimationDurationSeconds);
        }

        public void Animate(float strength, int iterationCount, bool freezeBorders, float animationDurationSeconds) {
            StartCoroutine(AnimateRelax(strength, iterationCount, freezeBorders, animationDurationSeconds));
        }

        IEnumerator AnimateRelax(float strength, int iterationCount, bool freezeBorders, float animationDurationSeconds) {
            float period = animationDurationSeconds / iterationCount;
            for (int i = 0; i < iterationCount; i += 1) {
                Relaxation.Relax(strength, iterationCount, freezeBorders);
                yield return new WaitForSeconds(period);
            }
            OnRelaxed();
        }


    }
}