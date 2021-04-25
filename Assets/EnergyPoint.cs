using UnityEngine;
using UnityEngine.UI;

public class EnergyPoint : MonoBehaviour {
    public Sprite FullSprite;
    public Sprite EmptySprite;
    public EnergyPoint Init() {
        Fill();
        return this;
    }
    public void Empty() {
        GetComponent<Image>().sprite = EmptySprite;
    }

    public void Fill() {
        GetComponent<Image>().sprite = FullSprite;
    }
}