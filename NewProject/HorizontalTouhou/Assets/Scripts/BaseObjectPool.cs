using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Ggross.Pattern;

public class BaseObjectPool<T> : MonoBehaviour where T: Component
{   
    [SerializeField] protected T prefab;
    [SerializeField] protected Transform parent;

    [SerializeField] public int minSize = 10, maxSize = 200;
    protected ObjectPool<T> pool;

    public void Init(bool collectionCheck = false){
        pool = new ObjectPool<T>(OnCreateItem, OnGetItem, OnReleaseItem, OnDestroyItem, collectionCheck, minSize, maxSize);
    }

    protected virtual void OnDestroyItem(T item){
        Destroy(item.gameObject);
    }

    protected virtual void OnReleaseItem(T item){
        item.gameObject.SetActive(false);
    }

    protected virtual void OnGetItem(T item){
        item.gameObject.SetActive(true);
    }

    protected virtual T OnCreateItem(){
        var item = Instantiate<T>(prefab, parent);
        return item;
    }

    public T Get(){
        return pool.Get();
    }

    public void Release(T item){
        pool.Release(item);
    }

    public void Clear(){
        pool.Clear();
    }

    public void SetPrefab(T p){
        prefab = p;
    }

    public void SetItemParent(Transform ip){
        parent = ip;
    }


}
