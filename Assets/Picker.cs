using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Picker : MonoBehaviour {
    public Action<Pickable> OnTouchedPickable;

    void OnTriggerEnter(Collider other) {
        var pickable = other.GetComponent<Pickable>();
        if (!pickable) return;

        OnTouchedPickable.Invoke(pickable);
    }
}