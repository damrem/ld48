using System.Collections.Generic;
using UnityEngine;

namespace Damrem.Grids.Irregular {

    public enum CardinalDirection {
        East,
        South,
        West,
        North,
    }

    public class CardinalDirectionUtil {

        public static Dictionary<CardinalDirection, Vector2> VectorByDirection = new Dictionary<CardinalDirection, Vector2> {
            {CardinalDirection.East, Vector2.right },
            {CardinalDirection.South, Vector2.down},
            {CardinalDirection.West, Vector2.left },
            {CardinalDirection.North, Vector2.up},
        };
    }
}