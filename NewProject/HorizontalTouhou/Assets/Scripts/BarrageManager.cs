using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ggross.Pattern;

public class BarrageManager : SingletonMonoBehaviour<BarrageManager>
{
    [SerializeField] private Barrage prefab;
    private BarragePool pool;

    private void Awake() {
        
        var poolObj = new GameObject("Barrage Pool");
        poolObj.transform.parent = transform;
        poolObj.transform.position = Vector3.zero;

        pool = poolObj.AddComponent<BarragePool>();
        pool.SetItemParent(pool.transform);
        pool.SetPrefab(prefab);

        pool.minSize = 3;
        pool.maxSize = 10;
        pool.Init();

        poolObj.SetActive(true);
    }

    // public Shot CreateShot(int index, Vector3 pos, Vector3 dir, float baseSpeed, bool enemy){
    //     var shot = poolList[index].Get();
    //     shot.Init(pos, dir, baseSpeed, enemy);
    //     shot.transform.position = pos;

    //     return shot;
    // }

    public Barrage CreateBarrage(){
        return pool.Get();

        // return CreateShot(data.prefabIndex, pos, dir, data.baseSpeed, enemy);
    }
}
