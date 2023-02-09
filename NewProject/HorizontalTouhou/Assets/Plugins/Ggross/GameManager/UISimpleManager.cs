using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ggross.GameManagement{
    public abstract class UISimpleManager<T> : MonoBehaviour, IPrefabManager<T> where T : MonoBehaviour
    {
        [SerializeField] protected T prefab;
        [SerializeField] protected Transform parent;
        protected List<T> list = new List<T>();
        protected List<GameObject> objects = new List<GameObject>();
        // protected int size;

        public T Add()
        {
            var t = Instantiate(prefab, parent);
            var obj = t.gameObject;

            list.Add(t);
            objects.Add(obj);

            return t;
        }

        public List<T> AddAll(int count){
            for(int i = 0; i < count; i++){
                Add();
            }
            return list;
        }

        public void Clear()
        {
            var size = list.Count;
            for(int i = 0; i<size; i++){
                Delete(list[0]);
            }
        }

        public void Delete(T t)
        {
            list.Remove(t);
            objects.Remove(t.gameObject);
            Destroy(t.gameObject);
        }

        public void ResizeParent()
        {
            return;
        }
    }
}

