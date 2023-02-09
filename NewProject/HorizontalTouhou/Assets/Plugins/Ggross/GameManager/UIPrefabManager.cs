using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ggross.GameManagement{
    public class UIPrefabManager<T> : MonoBehaviour, IPrefabManager<T> where T: MonoBehaviour
    {
        [SerializeField] private T prefab;
        [SerializeField] private Transform parent;

        protected List<T> list;
        protected List<GameObject> objectList;

        public void Init(){
            list = new List<T>();
            objectList = new List<GameObject>();
        }

        public T Add()
        {
            var t = Instantiate(prefab, parent);

            list.Add(t);
            objectList.Add(t.gameObject);

            t.gameObject.name = "index: " + objectList.Count;

            return t;
        }

        public void Clear()
        {   
            int count = list.Count;

            for(int i = 0; i<count; i++){
                Delete(list[0]);
            }

            list = new List<T>();
            objectList = new List<GameObject>();
        }

        public void Delete(T t)
        {
            list.Remove(t);
            objectList.Remove(t.gameObject);

            Destroy(t.gameObject);
        }

        public void ResizeParent()
        {
            // throw new System.NotImplementedException();
        }
    }
}

