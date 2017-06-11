using UnityEngine;
using System.Collections;

public class PooledObject : MonoBehaviour {

    [System.NonSerialized]
    ObjectPool poolInstanceForPrefab;

    public ObjectPool Pool { get; set; }

    public void ReturnToPool()
    {
        if (Pool)
        {
            Pool.AddObject(this);                  
        }

        else
            Destroy(gameObject);
    }

    public T GetPooledInstance<T>() where T : PooledObject
    {
        if (!poolInstanceForPrefab)
        {
            poolInstanceForPrefab = ObjectPool.GetPool(this);
        }
        return (T)poolInstanceForPrefab.GetObject();
    }
}