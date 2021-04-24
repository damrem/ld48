using System;

namespace Damrem.Grids.Irregular {

    public class Relaxation {

        public event Action OnRelaxed;

        public IrregularQuadGrid Grid { get; private set; }

        readonly RelaxationType Type;
        // RelaxationAnimator Animator;

        public Relaxation(IrregularQuadGrid irregularQuadGrid, RelaxationType type/*, RelaxationAnimator animator*/) {
            Grid = irregularQuadGrid;
            Type = type;
            // Animator = animator;

            //if(animator is null) Relax(irregularGridGenerationDef.RelaxationIterationCount);
            //else {
            //    animator.OnRelaxed += OnRelaxed;
            //    animator.Init(this);
            //}
        }

        public void Relax(float strength, int iterationCount = 1, bool freezeBorders = false) {
            for (int i = 0; i < iterationCount; i += 1) {
                RelaxOnce(strength, freezeBorders);
            }
            OnRelaxed.Invoke();
        }

        void RelaxOnce(float strength, bool freezeBorders) {
            switch (Type) {
                case RelaxationType.AverageDistanceToQuadCentroid:
                    Grid.RelaxAverageDistanceToQuadrangleBarycenter(strength, freezeBorders);
                    break;

                case RelaxationType.SideSize:
                    Grid.RelaxSideSize(strength, freezeBorders);
                    break;

                case RelaxationType.OskStaStyle:
                    Grid.RelaxOskSta(strength, freezeBorders);
                    break;

                case RelaxationType.ToAdjacentVerticesCentroid:
                    Grid.RelaxToAdjacentCornersBarycenter(strength);
                    break;

                case RelaxationType.None:
                default:
                    break;
            }
        }

        internal void Clear() {
            OnRelaxed = null;
        }
    }
}