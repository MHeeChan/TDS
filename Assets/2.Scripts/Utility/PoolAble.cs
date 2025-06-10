using UnityEngine;
using UnityEngine.Pool;

public abstract class PoolAble : MonoBehaviour {
    public IObjectPool<GameObject> Pool { get; set; }

    public abstract void SetDirection(Vector3 direction);

    public void ReleaseObject() {
        Pool.Release(gameObject);
    }
}