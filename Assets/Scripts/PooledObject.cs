using UnityEngine;

public class PooledObject : MonoBehaviour
{

    public GameObject prefabReference;


    public virtual void OnSpawned() { }
    public virtual void OnDespawned() { }
}
