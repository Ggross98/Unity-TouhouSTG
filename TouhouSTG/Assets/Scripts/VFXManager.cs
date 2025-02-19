using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ggross.Pattern;

public class VFXManager : SingletonMonoBehaviour<VFXManager>
{
    [SerializeField] List<VFX> prefabList;
    [SerializeField] VFXPool pool;

    private void Awake() {
        pool.Init();
    }

    public void CreateVFX(int index, Vector3 pos){
        if(index < 0 || index >= prefabList.Count) return;

        var prefab = prefabList[index];
        pool.SetPrefab(prefab);
        var vfx = pool.Get();
        vfx.SetPosition(pos);
        vfx.Play();
    }
}
