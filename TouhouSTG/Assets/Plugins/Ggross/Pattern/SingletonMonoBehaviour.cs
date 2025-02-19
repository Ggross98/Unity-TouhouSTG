using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ggross.Pattern{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T: MonoBehaviour
    {
        private static volatile T instance;
        private static object syncRoot = new Object();
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            T[] instances = FindObjectsOfType<T>();
                            // Debug.Log(instances.Length);
                            if (instances != null && instances.Length > 0)
                            {
                                instance = instances[0];
                                if(instance != null)
                                    return instance;
                                else{
                                    GameObject obj = new GameObject();
                                    obj.name = typeof(T).Name;
                                    instance = obj.AddComponent<T>();
                                }
                            }
                            else{
                                GameObject go = new GameObject();
                                go.name = typeof(T).Name;
                                instance = go.AddComponent<T>();
                            }
                            
                            //DontDestroyOnLoad(instance);
                        }
                    }
                }
                return instance;
            }
        }
    }

}
