using Damrem.UnityEngine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CellPosition))]
public class Player : MonoBehaviour {
    InputActionAsset Actions;
    public float MovePeriod = .5f;

    float OwnTime;
    float LastHorizontalAxis = 0;

    public Player Init(Cell cell) {
        GetComponent<CellPosition>().Init(cell);
        return this;
    }

    void Update() {
        LastHorizontalAxis = Input.GetAxis(AxisName.Horizontal);
        if (IsMoveTime()) Move();
    }

    bool IsMoveTime() {
        OwnTime += Time.deltaTime;
        if (OwnTime < MovePeriod) return false;

        OwnTime = 0;
        return true;
    }

    void Move() {
        Debug.Log("Move");
        if (LastHorizontalAxis == 0) return;

        var movement = Vector3.right;
        if (LastHorizontalAxis < 0) movement *= -1;
        transform.position = transform.position + movement;
    }
}
