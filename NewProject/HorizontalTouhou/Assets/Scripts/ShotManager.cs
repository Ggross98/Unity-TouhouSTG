using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ggross.Pattern;
using Danmaku.Data;

public class ShotManager : SingletonMonoBehaviour<ShotManager>
{
    [SerializeField] private Shot[] shotPrefabs;
    private List<ShotPool> poolList;

    private void Awake() {
        poolList = new List<ShotPool>();
        foreach(var prefab in shotPrefabs){
            var poolObj = new GameObject($"Pool {prefab.name}");
            poolObj.transform.parent = transform;
            poolObj.transform.position = Vector3.zero;

            var pool = poolObj.AddComponent<ShotPool>();
            pool.SetItemParent(pool.transform);
            pool.SetPrefab(prefab);

            pool.Init();

            poolObj.SetActive(true);
            poolList.Add(pool);
        }
    }

    // public Shot CreateShot(int index, Vector3 pos, Vector3 dir, float baseSpeed, bool enemy){
    //     var shot = poolList[index].Get();
    //     shot.Init(pos, dir, baseSpeed, enemy);
    //     shot.transform.position = pos;

    //     return shot;
    // }

    public Shot CreateShot(ShotData data, Vector3 pos, Vector3 dir, bool enemy){
        var index = data.prefabIndex;
        var shot = poolList[index].Get();

        shot.Init(pos, dir, enemy, data);

        return shot;

        // return CreateShot(data.prefabIndex, pos, dir, data.baseSpeed, enemy);
    }



}


