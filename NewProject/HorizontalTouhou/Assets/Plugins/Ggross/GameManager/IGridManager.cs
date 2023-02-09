using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ggross.GameManagement{
    public interface IGridManager<T>: IPrefabManager<T>
    {   
        T Add(int x, int y);
        T[,] AddAll();
        bool IsEmpty(int x, int y);
        T GetAt(int x, int y);
        GameObject GetObjectAt(int x, int y);
        void DeleteAt(int x, int y);
    }

}
