using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Block : MonoBehaviour {
    public Block Init(Color color) {
        GetComponent<MeshRenderer>().material.color = color;
        return this;
    }
}