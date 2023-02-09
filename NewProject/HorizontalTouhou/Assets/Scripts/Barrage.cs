using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Danmaku.Data;

public class Barrage : MonoBehaviour
{
    private ShotManager shotManager;
    private ShotData shotData;
    public bool firing = false;


    public int counter = 0;
    public float timer = 0f;

    private Vector3 startPos;


    public PlayerController player;

    // public void SetAction(System.Action<Barrage> a, float inverval){
    //     this.action = a;
    // }


    void Awake()
    {
        shotManager = ShotManager.Instance;
        startPos = transform.position;
    }

    private System.Action<Barrage> deactiveAction;

    public void SetDeactiveAction(System.Action<Barrage> action){
        this.deactiveAction = action;
    }

    public void Deactive(){
        deactiveAction.Invoke(this);
    }

    public void SetPosition(Vector3 pos){
        transform.position = pos;
    }

    public void BackToStartPosition(){
        SetPosition(startPos);
    }

    public void SetShotData(ShotData sd){
        this.shotData = sd;
    }

    public Shot CreateBullet(Vector3 dir, float distance, Vector3 deltaPos){
        var prefabIndex = shotData.prefabIndex;
        var baseSpeed = shotData.baseSpeed;

        dir = dir.normalized;
        var startPos = transform.position + dir * distance + deltaPos;
        // var shot = shotManager.CreateShot(prefabIndex, startPos, dir, baseSpeed, true);
        var shot = shotManager.CreateShot(this.shotData, startPos, dir, true);

        shot.wallCheckMode = shotData.wallCheckMode;

        return shot;
    }

    // public Shot CreateBulletToTarget(Vector3 targetPos, float distance = 0f){
    //     var dir = targetPos = transform.position;
    //     return CreateBullet(dir, distance, Vector3.zero);
    // }

    /// <summary>
    /// 向指定位置射出扇形弹幕
    /// </summary>
    /// <param name="prefabIndex"></param>
    /// <param name="target"></param>
    /// <param name="fire"></param>
    /// <param name="angle"></param>
    /// <param name="speed"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public List<Shot> FireSector(Vector3 target, int fire, float angle, float distance = 0f)
    {

        float deltaAngle = angle / (fire - 1);

        var centerDir = (target - transform.position).normalized;
        var dir = Quaternion.AngleAxis(-angle / 2, Vector3.forward) * centerDir;
        var offset = Quaternion.AngleAxis(deltaAngle, Vector3.forward);

        var shotList = new List<Shot>();
        for(int i = 0; i < fire; i++)
        {
            var shot = CreateBullet(dir, distance, Vector3.zero);
            shotList.Add(shot);

            dir = offset * dir;
        }
        return shotList;
    }

    /// <summary>
    /// 发射圆周
    /// </summary>
    /// <param name="prefabIndex"></param>
    /// <param name="target"></param>
    /// <param name="fire"></param>
    /// <param name="speed"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public List<Shot> FireRound(Vector3 startDir, float startAngle, int fire, float distance, Vector3 deltaPos)
    {
        var centerDir = startDir.normalized;
        var dir = Quaternion.AngleAxis(startAngle, Vector3.forward) * centerDir;

        float deltaAngle = 360f / fire;
        var offset = Quaternion.AngleAxis(deltaAngle, Vector3.forward);

        var shotList = new List<Shot>();
        for(int i = 0; i < fire; i++)
        {
            var shot = CreateBullet(dir, distance, deltaPos);
            shotList.Add(shot);

            dir = offset * dir;
        }
        return shotList;

    }


    public List<Shot> FireSector(Vector3 startDir, float startAngle, int fire, float deltaAngle, float distance, Vector3 deltaPos)
    {
        var centerDir = startDir.normalized;

        // 根据奇偶性计算初始角度
        var startDirAngle = fire % 2 == 0 ? -(float)(fire/2 - 0.5f) * deltaAngle : -(fire/2) * deltaAngle;
        var dir = Quaternion.AngleAxis(startAngle + startDirAngle, Vector3.forward) * centerDir;

        // 每次射击的偏差
        var offset = Quaternion.AngleAxis(deltaAngle, Vector3.forward);

        var shotList = new List<Shot>();
        for(int i = 0; i < fire; i++)
        {
            var shot = CreateBullet(dir, distance, deltaPos);
            shotList.Add(shot);

            dir = offset * dir;
        }
        return shotList;

    }

    // public List<Shot> FireRound(FireData data, float deltaAngle){
    //     var startAngle = data.startAngle + deltaAngle;
    //     // if(data.offsetAngle != null) data.offsetAngle(counter);
    //     // var target = data.aimed ? player.transform.position : data.startDir + transform.position;
    //     var dir = data.aimed ? player.transform.position - transform.position : data.startDir;


    //     return FireRound(target, startAngle, data.count, data.startDistance);
    // }

}
