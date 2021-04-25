using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CellPosition))]
[RequireComponent(typeof(MeshRenderer))]
public class Block : MonoBehaviour {
    public Cell Cell { get { return GetComponent<CellPosition>().Cell; } }
    public bool IsUnbreakable { get; private set; } = false;
    public int Type { get; private set; }
    public Block Init(Cell cell, int type, Color color) {
        Type = type;
        GetComponent<CellPosition>().Init(cell);
        SetColor(color);
        transform.localRotation = Quaternion.Euler(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f));
        return this;
    }

    public void SetColor(Color color) {
        GetComponent<MeshRenderer>().material.color = color;
    }

    public void SetUnbreakable() {
        Type = -1;
        IsUnbreakable = true;
        SetColor(Color.gray);
    }

    public void AnimateDestroy(Action<Block> onEnd) {
        StartCoroutine(AnimateDestroyCoroutine(onEnd));
    }

    IEnumerator AnimateDestroyCoroutine(Action<Block> onEnd) {
        while (transform.localScale.x > Mathf.Epsilon) {
            transform.localScale -= Vector3.one * .05f;
            yield return null;
        }
        onEnd.Invoke(this);
    }

    public override string ToString() {
        return name;
    }
}