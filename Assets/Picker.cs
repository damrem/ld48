using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Picker : MonoBehaviour {
    public Action<Pickable> OnTouchedPickable;

    void OnTriggerEnter(Collider other) {
        Debug.Log("OnCollisionEnter2D " + other);
        var pickable = other.GetComponent<Pickable>();
        Debug.Log("pickable= " + pickable);
        if (!pickable) return;

        OnTouchedPickable.Invoke(pickable);
    }
}