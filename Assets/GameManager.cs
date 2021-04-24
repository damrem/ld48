using UnityEngine;

public class GameManager : MonoBehaviour {
    void Start() {
        Init();
    }

    void Init() {
        var level = GetComponentInChildren<Level>().Init();
    }

    void Update() {

    }
}
