using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMan : MonoBehaviour {
    Transform Target;
    Level Level;
    public float OffsetY = 4;
    public float SizeOffset = .25f;
    public float BottomLimit = 8;
    public CameraMan Init(Transform target, Level level) {
        GetComponent<Camera>().orthographicSize = level.Def.Width + SizeOffset;
        Target = target;
        Level = level; ;
        return this;
    }

    void Update() {
        if (!Target) return;

        var targetY = Target.position.y + OffsetY;
        if (targetY < -Level.Def.Depth + BottomLimit) targetY = -Level.Def.Depth + BottomLimit;
        var pos = new Vector3((float)Level.Def.Width / 2 - .5f, targetY, -10);
        transform.position = pos;
    }
}