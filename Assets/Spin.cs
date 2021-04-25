using System.Collections;
using UnityEngine;

public class Spin : MonoBehaviour {
    public Spin Init() {
        StartCoroutine(AnimateSpin());
        return this;
    }

    IEnumerator AnimateSpin() {
        transform.Rotate(Vector3.up, Random.Range(0f, 360f));
        while (true) {
            transform.Rotate(Vector3.up, 1);
            yield return null;
        }
    }
}