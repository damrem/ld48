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
        while (true) {
            var y = Mathf.Cos(Time.time) / 4;
            Debug.Log(y);
            transform.position = initialPosition + Vector3.up * y;
            yield return null;
        }
    }
}