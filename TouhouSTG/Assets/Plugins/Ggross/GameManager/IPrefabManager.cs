using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ggross.GameManagement{
    public interface IPrefabManager<T>
    {
        void ResizeParent();
        T Add();
        void Delete(T t);
        void Clear();
    }
}

