using Damrem.UnityEngine;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Pickable : MonoBehaviour {
    public PickableType Type { get; private set; }

    public Pickable Init(PickableType type) {
        Type = type;
        GetComponent<Collider>().isTrigger = true;
        return this;
    }

    public void Pick() {
        //TODO: play sound
        gameObject.Destroy();
    }
}