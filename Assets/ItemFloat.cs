using System.Collections;
using UnityEngine;

public class ItemFloat : MonoBehaviour {
    public ItemFloat Init() {
        Debug.Log("Init");
        StartCoroutine(Animate());
        return this;
    }

    IEnumerator Animate() {
        var initialPosition = transform.position;
        var offset = Random.Range(0, 1000);
        while (true) {
            transform.position = initialPosition + Vector3.up * Mathf.Cos(Time.time + offset) / 4;
            yield return null;
        }
    }
}