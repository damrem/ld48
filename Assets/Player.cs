using Damrem.UnityEngine;
using UnityEngine;

[RequireComponent(typeof(CellPosition))]
public class Player : MonoBehaviour {
    public float MovePeriod = .5f;

    float OwnTime;

    public Player Init(Cell cell) {
        GetComponent<CellPosition>().Init(cell);
        return this;
    }

    void Update() {
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
        var h = Input.GetAxis(AxisName.Horizontal);
        if (h == 0) return;
        var movement = Vector3.right;
        if (h < 0) movement *= -1;
        transform.position = transform.position + movement;
    }
}
