using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STGOptionMovement : MonoBehaviour {
    private enum UpdateType { @update, @fixedupdate, @manual }
    [SerializeField] UpdateType updateType = UpdateType.update;
    [Header("--- GameObject ---")]
    [SerializeField] Transform Target = null;
    private Vector3 headPos;
    [SerializeField] List<Part> Bodys = new List<Part>();
    [Header("--- Movement Info ---")]
    [SerializeField] float distance = 1f;
    [SerializeField] bool tracePosition = true;
    [SerializeField] bool traceRotation = true;
    private float movingDistance = 0f;
    [Header("--- Cash ---")]
    [SerializeField] TransformCash defaultCash = new TransformCash();
    private TransformCash[] transformCashes;

    public void Update_() {
        MovePosition();
        this.headPos = this.Target.position;
    }

    void Update() {
        if (this.updateType == UpdateType.update) Update_();
    }
    void FixedUpdate() {
        if (this.updateType == UpdateType.fixedupdate) Update_();
    }
    void OnValidate() {
        InitCash();
    }
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        foreach (var cash in this.transformCashes)
            Gizmos.DrawSphere(cash.Pos, 0.2f);
    }

    private void InitCash() {
        int num = 0;
        foreach (var body in Bodys)
            num += body.Step;
        this.transformCashes = new TransformCash[num + 1 + 1];
        for (int i = 0; i < this.transformCashes.Length; i++) {
            this.transformCashes[i] = new TransformCash(this.defaultCash.Pos, this.defaultCash.Rot);
        }
        for (int i = 0; i < this.Bodys.Count; i++) {
            this.Bodys[i].Body.position = defaultCash.Pos;
        }
        this.headPos = defaultCash.Pos;
    }
    private void MovePosition() {
        float move = Vector3.Magnitude(this.Target.position - this.headPos);
        this.movingDistance += move;
        if (this.movingDistance > distance) {
            this.movingDistance = 0f;
            CashUpdate();
        }
        UpdateBodyPosition();
    }
    private void CashUpdate() {
        this.transformCashes[0] = new TransformCash(this.Target.position, this.Target.rotation);
        for (int i = 0; i < this.transformCashes.Length - 1; i++) {
            int k = (this.transformCashes.Length - 1) - i;
            this.transformCashes[k] = this.transformCashes[k - 1];
        }
    }
    private void UpdateBodyPosition() {
        int cashIndex = 0;
        foreach (var body in Bodys) {
            cashIndex += body.Step;
            TransformCash fromCash = this.transformCashes[cashIndex + 1];
            TransformCash toCash = this.transformCashes[cashIndex];
            float t = movingDistance / distance;
            if (body.Body == null) continue;
            if (this.tracePosition) body.Body.transform.position = Vector3.Lerp(fromCash.Pos, toCash.Pos, t);
            if (this.traceRotation) body.Body.transform.rotation = Quaternion.Lerp(fromCash.Rot.normalized, toCash.Rot.normalized, t);
        }
    }
    [System.Serializable]
    private class Part {
        [SerializeField] Transform body = null;
        [SerializeField] int step = 1;
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
