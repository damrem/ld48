using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMan : MonoBehaviour {
    Transform Target;
    Level Level;
    public CameraMan Init(Transform target, Level level) {
        Debug.Log("Init " + target);
        GetComponent<Camera>().orthographicSize = level.Def.Width;
        Target = target;
        Level = level; ;
        return this;
    }

    void Update() {
        if (!Target) return;

        var targetY = Target.position.y + 4;
        if (targetY < -Level.Def.Depth + 8) targetY = -Level.Def.Depth + 8;
        var pos = new Vector3((float)Level.Def.Width / 2 - .5f, targetY, -10);
        transform.position = pos;
    }
}