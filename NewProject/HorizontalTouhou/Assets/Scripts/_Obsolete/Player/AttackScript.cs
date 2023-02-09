using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {
    #region 1 - 变量

    /// <summary>
    /// 子弹预设
    /// </summary>
    [SerializeField] private Transform shotPrefab;

    /// <summary>
    /// 两发子弹之间的发射间隔时间
    /// </summary>
    public float shootingRate = 0.25f;

    /// <summary>
    /// 当前冷却时间
    /// </summary>
    private float shootCooldown;

    #endregion

    // Use this for initialization
    void Start()
    {
        // 初始化冷却时间为0
        shootCooldown = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // 冷却期间实时减少时间
        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
    }

    private Transform CreateBullet(Vector3 gun, Vector3 dir, float distance, Transform shotPrefab, float speed)
    {
        //Debug.Log(dir);

        var shotTransform = Instantiate(shotPrefab) as Transform;

        // 设置子弹归属
        Shot shot = shotTransform.gameObject.GetComponent<Shot>();
        shot.enemyShot = false;
        shot.direction = dir;
        shot.lifetime = 3f;
        shot.SetSpeed(speed);

        //子弹出发位置
        shotTransform.position = gun + dir.normalized * distance;

        return shotTransform;
    }



    /// <summary>
    /// 射击
    /// </summary>
    /// <param name="isEnemy">是否是敌人的子弹</param>
    public void Attack(bool isEnemy)
    {
        if (CanAttack)
        {

            /*
            if (isEnemy)
            {
                SoundEffectsHelper.Instance.MakeEnemyShotSound();
            }
            else
            {
                SoundEffectsHelper.Instance.MakePlayerShotSound();
            }
            */

            shootCooldown = shootingRate;
            Vector3 dir1 = Quaternion.AngleAxis(3, Vector3.forward) * Vector3.right;
            Vector3 dir2 = Quaternion.AngleAxis(-3, Vector3.forward) * Vector3.right;

           // CreateBullet(transform.position ,Vector3.right ,0 ,shotPrefab ,20);
            CreateBullet(transform.position, dir1, 0, shotPrefab, 20);
            CreateBullet(transform.position, dir2, 0, shotPrefab, 20);

            /*

            // 创建一个子弹
            var shotTransform = Instantiate(shotPrefab) as Transform;

            //Destroy(shotTransform, 20);

            // 指定子弹位置
            shotTransform.position = transform.position;

            // 设置子弹归属
            ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
            if (shot != null)
            {
                shot.isEnemyShot = isEnemy;
            }

            // 设置子弹运动方向
            MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
            if (move != null)
            {
                // towards in 2D space is the right of the sprite
                move.direction = this.transform.right;
            }*/

        }
    }

    /// <summary>
    /// 武器是否准备好再次发射
    /// </summary>
    public bool CanAttack
    {
        get
        {
            return shootCooldown <= 0f;
        }
    }
}
