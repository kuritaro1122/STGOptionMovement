using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement1 : MonoBehaviour {

    [Header("--- GameObject ---")]
    [SerializeField] Transform Head;
    [SerializeField] Part[] Bodys;
    [Header("--- Movement Info ---")]
    [SerializeField] float distance;
    private float movingDistance = 0f;
    [Header("--- Cash ---")]
    [SerializeField, Disable] TransformCash[] transformCashes;

    public void AddPosition(float move) { // move: speed * Time.deltaTime
        movingDistance += move;
        if (movingDistance > distance) {
            movingDistance = 0f;
            CashUpdate();
        }
        UpdateBodyPosition();
    }

    void OnValidate() {
        int num = 0;
        foreach (var body in Bodys)
            num += body.Step;
        transformCashes = new TransformCash[num + 1 + 1];
        /*for (int i = 0; i < transformCashes.Length; i++) {
            if (Head == null) break;
            Vector3 _pos = Head.position + Vector3.forward * distance * i;
            transformCashes[i] = new TransformCash(_pos, Quaternion.identity);
        }*/
    }

    private Vector3 headPos;
    void Update() {
        AddPosition(Vector3.Magnitude(Head.position - headPos));
        headPos = Head.position;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        foreach (var cash in transformCashes)
            Gizmos.DrawSphere(cash.Pos, 0.2f);
    }

    private void CashUpdate() {
        transformCashes[0] = new TransformCash(Head.position, Head.rotation);
        for (int i = 0; i < transformCashes.Length - 1; i++) {
            int k = (transformCashes.Length - 1) - i;
            transformCashes[k] = transformCashes[k - 1];
        }
    }
    private void UpdateBodyPosition() {
        int cashIndex = 0;
        foreach (var body in Bodys) {
            cashIndex += body.Step;
            TransformCash fromCash = transformCashes[cashIndex + 1];
            TransformCash toCash = transformCashes[cashIndex];
            float t = movingDistance / distance;
            body.Body.transform.position = Vector3.Lerp(fromCash.Pos, toCash.Pos, t);
            body.Body.transform.rotation = Quaternion.Lerp(fromCash.Rot.normalized, toCash.Rot.normalized, t);
        }
    }

    [System.Serializable]
    private struct Part {
        [SerializeField] Transform body;
        [SerializeField] int step;
        public Transform Body { get { return this.body; } }
        public int Step { get { return this.step; } }
    }

    [System.Serializable]
    private struct TransformCash {
        [SerializeField] Vector3 pos;
        [SerializeField] Quaternion rot;
        public TransformCash(Vector3 pos, Quaternion rot) {
            this.pos = pos;
            this.rot = rot;
        }
        public Vector3 Pos { get { return this.pos; } }
        public Quaternion Rot { get { return this.rot; } }
    }
}
