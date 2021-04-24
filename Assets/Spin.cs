using System.Collections;
using UnityEngine;

public class Spin : MonoBehaviour {
    public Spin Init() {
        StartCoroutine(AnimateSpin());
        return this;
    }

    IEnumerator AnimateSpin() {
        while (true) {
            transform.Rotate(Vector3.up, 1);
            yield return null;
        }
    }
}