using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBarrage : MonoBehaviour {

    private Transform player;
    //public ObjectPool bulletPool;
    void Awake()
    {

    }

    public void SetPlayer(Transform p)
    {
        player = p;
    }


    private Transform CreateBullet(Vector3 gun, Vector3 dir, float distance,Transform shotPrefab,float speed)
    {
        //Debug.Log(dir);
        //GameObject shotTransform = ObjectPool.instance .GetObj(shotPrefab .gameObject );

        Transform shotTransform = Instantiate(shotPrefab) as Transform;


        // 设置子弹归属
        Shot shot = shotTransform.gameObject.GetComponent<Shot>();
        shot.enemyShot = true;
        shot.direction = dir;
        shot.SetSpeed(speed);

        //子弹出发位置
        shotTransform.transform.position = gun+dir.normalized *distance;

        return shotTransform.transform ;
    }

    private Transform CreateBullet(Vector3 gun, Vector3 dir, Transform shotPrefab,float speed)
    {
        //Debug.Log(dir);
        //Debug.Log(shotPrefab .gameObject .name);
        Transform shotTransform = Instantiate(shotPrefab) as Transform;


        // 设置子弹归属
        Shot shot = shotTransform.gameObject.GetComponent<Shot>();
        shot.enemyShot = true;
        shot.direction = dir.normalized ;
        shot.SetSpeed(speed);

        //子弹出发位置
        shotTransform.position = gun;
        
        return shotTransform;
    }

    public Transform CreateBullet(Vector3 gun,Transform  shotPrefab)
    {
        Transform shotTransform = Instantiate(shotPrefab) as Transform;

        shotTransform.position = gun;

        Shot shot = shotTransform.gameObject.GetComponent<Shot>();
        shot.SetSpeed(1);
        shot.SetDirection(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0));
        return shotTransform;
    }

    private Transform CreateBullet(Transform gun, Vector3 dir, Transform shotPrefab,float speed)
    {
        return CreateBullet (gun.position ,dir,shotPrefab ,speed);
    }

    public void FireCircularBullet(Vector3 gun, Vector3 dir, Vector3 center,float angleSpeed,Transform bullet,float speed)
    {
        Transform tempBullet = CreateBullet(gun, dir, bullet,speed);
        StartCoroutine(tempBullet.GetComponent<Shot>().CircularMove(center, angleSpeed ));
    }



    public void FireCircularBullet(Vector3 gun,Vector3 dir,Transform bullet,float speed)
    {
        Transform tempBullet = CreateBullet(gun, dir, bullet,speed);
        StartCoroutine(tempBullet.GetComponent <Shot >().CircularMove(gun,2f));
    }

    /// <summary>
    /// 以某个坐标为圆心，在周围生成圆周运动的子弹
    /// </summary>
    /// <param name="center">圆心坐标</param>
    /// <param name="fire">子弹数量</param>
    /// <param name="distance">半径</param>
    /// <param name="clockwise">是否顺时针</param>
    /// <param name="bullet">预制子弹</param>
    public void FireCircle(Vector3 center,int fire,float distance,float angleSpeed,float speed,Transform bullet,float startOffset)
    {
        //第一颗发射方向
        Vector3 dir = Vector3.up;
        //初始偏移
        Quaternion offset = Quaternion.AngleAxis(startOffset , Vector3.forward);
        dir = offset * dir;

        //偏移量
        Quaternion rotateQuate;
        if (angleSpeed >0)
            rotateQuate = Quaternion.AngleAxis(360 /fire, Vector3.forward);
        else
        {
            rotateQuate = Quaternion.AngleAxis(-360 / fire, Vector3.forward);
        }
        Vector3 gun;
        for (int i = 0; i < fire; i++)
        {
            gun = dir.normalized * distance + center;
            Shot shot = CreateBullet(gun, dir, bullet,speed).GetComponent <Shot >();
            // shot.SetCheckOut(false);
            dir = rotateQuate * dir;
            StartCoroutine(shot.CircularMove(center,angleSpeed ));
        }

    }

    /// <summary>
    /// 连续发射若干组在一定角度内的旋转子弹
    /// </summary>
    /// <param name="center"></param>
    /// <param name="fire"></param>
    /// <param name="distance"></param>
    /// <param name="angleSpeed"></param>
    /// <param name="speed"></param>
    /// <param name="bullet"></param>
    /// <param name="startOffset"></param>
    /// <param name="angle"></param>
    /// <param name="firePerRound"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    public IEnumerator FireCircleGroup(Vector3 center, int fire, float distance, float angleSpeed, float speed, Transform bullet, float startOffset,float angle,int firePerRound,float delay)
    {
        Vector3 dir = Vector3.up;
        //初始偏移
        //Quaternion offset;
        float totalAngle = startOffset;
        float deltaAngle = angle / firePerRound;
        for(int i = 0; i < firePerRound; i++)
        {
            //offset = Quaternion.AngleAxis(totalAngle +deltaAngle*i, Vector3.forward);
            FireCircle(center,fire,distance,angleSpeed ,speed ,bullet , totalAngle + deltaAngle * i);

            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }
            
        }

        yield return null;
    }


    /// <summary>
    /// 扇形弹幕
    /// </summary>
    /// <param name="gun">子弹出发点坐标</param>
    /// <param name="target">扇形中心方向上任意一个坐标</param>
    /// <param name="bullet">预制子弹</param>
    /// <param name="fire">发射数量</param>
    /// <param name="angle">散布角度</param>
    public IEnumerator FireSector(Vector3 gun,Vector3 target,Transform bullet,int fire, float angle,float speed)
    {
        float deltaAngle = angle / (fire - 1);
        Vector3 dir = (target - gun).normalized ;
        Quaternion offset = Quaternion.AngleAxis(-angle / 2, Vector3.forward);
        dir = offset * dir;

        offset = Quaternion.AngleAxis(deltaAngle, Vector3.forward);
        for(int i = 0; i < fire; i++)
        {
            CreateBullet(gun, dir, bullet,speed);
            dir = offset * dir;
        }
        yield return null;
    }

    public IEnumerator FireSector(float centerOffset,Vector3 gun,Transform bullet, int fire, float angle, float speed)
    {
        float deltaAngle = angle / (fire - 1);
        Vector3 dir = Quaternion.AngleAxis(centerOffset, Vector3.forward)*Vector3 .up;
        Quaternion offset= Quaternion.AngleAxis(-angle / 2, Vector3.forward);
        dir = offset * dir;
        offset = Quaternion.AngleAxis(deltaAngle , Vector3.forward);
        for (int i = 0; i < fire; i++)
        {
            CreateBullet(gun, dir, bullet, speed);
            dir = offset * dir;
        }
        
        yield return null;
    }




    public void FireSectorBounce(Vector3 gun, Vector3 dir, Transform bullet, int fire, float angle, float speed)
    {
        float deltaAngle = angle / (fire - 1);
        //Vector3 dir = (target - gun).normalized;
        Quaternion offset = Quaternion.AngleAxis(-angle / 2, Vector3.forward);
        dir = offset * dir;

        offset = Quaternion.AngleAxis(deltaAngle, Vector3.forward);
        for (int i = 0; i < fire; i++)
        {
            Shot shot=CreateBullet(gun, dir, bullet, speed).GetComponent <Shot >();
            StartCoroutine (shot.BounceMode(2));
            dir = offset * dir;
        }

    }

    public IEnumerator FireSectorBounceGroup(Vector3 gun, Vector3 dir, Transform bullet, int fire, float angle, float speed,int group,float totalOffset,float delay)
    {
        Quaternion offset = Quaternion.AngleAxis(angle/group,Vector3 .forward );
        //Vector3 dir = gun;
        for(int i = 0; i < group; i++)
        {
            FireSectorBounce(gun,dir,bullet,fire,angle,speed);
            dir = offset * dir;
            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }







    //发射球形回转弹幕，限定角度
    public IEnumerator FireCircum(float startOffset,Vector3 start, Transform bullet, float distance, float endTime, float changeTime, float angle, float speed, float delay, int n1, int n2)
    {
        
        Vector3 bulletDir = Vector3.up;      //发射方向
        Quaternion rotateQuate = Quaternion.AngleAxis(360 / n2+startOffset, Vector3.forward);
        for (int j = 0; j < n1; j++)
        {
            for (int i = 0; i < n2; i++)
            {
                Vector3 creatPoint = start + bulletDir * distance;
                Transform tempBullet = CreateBullet(creatPoint, bulletDir, bullet,speed);
                Shot shot = tempBullet.GetComponent<Shot>();
                if (shot != null)
                {
                    // shot.SetCheckOut(false);
                    // shot.SetStraightMove (false);
                    shot.lifetime = 99;
                    //shot.speed = speed;
                    StartCoroutine(shot.DirChangeMoveMode(endTime, changeTime, angle));
                    bulletDir = rotateQuate * bulletDir;
                }

            }
            yield return new WaitForSeconds(delay);
        }

        yield return null;
    }

    //发射球形回转弹幕，固定0度
    public IEnumerator FireCircum(Vector3 start, Transform bullet, float distance, float endTime, float changeTime, float angle, float speed, float delay, int n1, int n2)
    {
        StartCoroutine(
        FireCircum(0f, start, bullet, distance, endTime, changeTime, angle, speed, delay, n1, n2));
        
        yield return null;
    }

    //圆周弹幕，限定角度
    public void FireAround(float startOffset,Vector3 gun, Transform shotPrefab,int firePerRound,float speed)
    {
        //第一颗发射方向
        Vector3 dir = Vector3.up ;
        //初始偏移
        Quaternion offset = Quaternion.AngleAxis(startOffset, Vector3.forward);
        dir = offset * dir;

        //偏移量
        Quaternion rotateQuate = Quaternion.AngleAxis(360/firePerRound, Vector3.forward);

        for(int i = 0; i < firePerRound; i++)
        {
            CreateBullet(gun, dir, shotPrefab, speed);
            dir = rotateQuate * dir;
        }


    }

    //圆周弹幕，随机角度
    public void FireAround(Vector3 gun, Transform shotPrefab, int firePerRound,float speed)
    {
        
        //初始偏移
        float rd = Random.Range(0, 720 / firePerRound);
        FireAround(rd, gun, shotPrefab, firePerRound,speed);

    }

    //随机弹
    public void FireRandom(Vector3 gun, Transform shotPrefab,float speed)
    {
        //第一颗发射方向
        Vector3 dir = Vector3.up;
        //初始偏移
        float rd = Random.Range(0, 360);
        Quaternion offset = Quaternion.AngleAxis(rd, Vector3.forward);
        dir = offset * dir;

        CreateBullet(gun, dir, shotPrefab,speed);
    }

    //自机狙
     public void FireSniper(Vector3 gun,Transform target, Transform shotPrefab,float speed)
    {
        Vector3 dir = -gun + target.position;
        dir = dir.normalized;
        CreateBullet(gun, dir, shotPrefab, speed);

    }

    //涡轮弹幕
    public IEnumerator FireTurbine(float startOffset,float angle,Vector3 gun,float radius,float distance,int round,Transform bullet,int firePerRound,float speed,float delay)
    {
        Vector3 bulletDir = Vector3.up;      //发射方向
        //float rd = Random.Range(0, 360);
        Quaternion offset = Quaternion.AngleAxis(startOffset, Vector3.forward);
        bulletDir = offset * bulletDir;
        Quaternion rotateQuate = Quaternion.AngleAxis(angle, Vector3.forward);//使用四元数制造绕Z轴旋转20度的旋转
        for (int i = 0; i < round; i++)
        {
            Vector3 firePoint = gun + bulletDir * radius;   //使用向量计算生成位置
            FireAround(firePoint ,bullet,firePerRound,speed );     //在算好的位置生成一波圆形弹幕
            yield return new WaitForSeconds(delay);     //延时较小的时间（为了表现效果），计算下一步
            bulletDir = rotateQuate * bulletDir;        //发射方向改变
            radius += distance;     //生成半径增加
        }
    }

    //圆周弹幕二次生成
    public IEnumerator FireAroundGroup(Vector3 gun, float startOffset,int group, Transform bullet, int firePerRound,int round,float createdelay,float rounddelay,float speed)
    {
        Vector3 bulletDir = Vector3.up;

        Quaternion offset = Quaternion.AngleAxis(startOffset, Vector3.forward);
        bulletDir = offset * bulletDir;


        Quaternion rotateQuate = Quaternion.AngleAxis(360/group, Vector3.forward);//使用四元数制造绕Z轴旋转45度的旋转
        List<Transform> bullets = new List<Transform>();       //装入开始生成弹幕
        for (int i = 0; i < group; i++)
        {
            var tempBullet = CreateBullet(gun,bulletDir,bullet, speed);
            bulletDir = rotateQuate * bulletDir; //生成新的子弹后，让发射方向旋转，到达下一个发射方向
            bullets.Add(tempBullet);
        }


        yield return new WaitForSeconds(createdelay );   //延迟后再生成多波弹幕
        for(int i = 0; i < group; i++)
        {
            if (bullets[i] != null)
            {

                bullets[i].GetComponent<Shot>().SetSpeed(0);

            }
        }
        for (int i = 0; i < round; i++)
        {
            for (int j = 0; j < bullets.Count; j++)
            {
                if(bullets[j]!=null)
                   FireAround(0,bullets[j].position, bullet, firePerRound, speed);//通过之前弹幕的位置，生成多波多方向的圆形弹幕。这里调用了上面写过的圆形弹幕函数
                //Destroy(bullets[j].gameObject);
            }
            yield return new WaitForSeconds(rounddelay );
        }

        for (int i = 0; i < group; i++)
        {
            if (bullets[i] != null)
                Destroy(bullets[i].gameObject );

        }
    }

    public void CreateDelaySniper(Vector3 gun, Vector3 dir,float starttime,float pausetime,float speed,float newspeed,Transform prefab)
    {
        Shot bullet = CreateBullet(gun,dir,prefab ,speed ).GetComponent <Shot >();
        bullet.lifetime = 20;
        // bullet.SetCheckOut(false);
        //StartCoroutine(bullet.Suspend(starttime, pausetime));
        StartCoroutine(bullet.DelaySniper(starttime + pausetime, newspeed, player));
    }
    
    /// <summary>
    /// 发射一圈子弹，一定时间后变为自机狙
    /// </summary>
    /// <param name="gun"></param>
    /// <param name="starttime"></param>
    /// <param name="pausetime"></param>
    /// <param name="speed"></param>
    /// <param name="newspeed"></param>
    /// <param name="firePerRound"></param>
    /// <param name="prefab"></param>
    public void FireAroundDelaySniper(Vector3 gun,float starttime,float pausetime,float speed,float newspeed,int firePerRound,Transform prefab)
    {
        //第一颗发射方向
        Vector3 dir = Vector3.up;
        //初始偏移
        Quaternion offset = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
        dir = offset * dir;

        //偏移量
        Quaternion rotateQuate = Quaternion.AngleAxis(360 / firePerRound, Vector3.forward);

        for (int i = 0; i < firePerRound; i++)
        {
            CreateDelaySniper (gun,dir,starttime ,pausetime ,speed ,newspeed ,prefab );
            dir = rotateQuate * dir;
        }
    }

    public IEnumerator FireLineSniper(Vector3 gun,Vector3 dir,Transform  bullet,float speed,int number,float deltaTime)
    {
        for(int i = 0; i < number; i++)
        {
            CreateBullet(gun,dir,bullet,speed);
            yield return new WaitForSeconds(deltaTime );
        }
        yield return null;
    }

    public IEnumerator FireLineBounce(Vector3 gun, Vector3 dir, Transform bullet, float speed, int number, float deltaTime)
    {
        for (int i = 0; i < number; i++)
        {
            Shot shot = CreateBullet(gun, dir, bullet, speed).GetComponent <Shot >();
            StartCoroutine(shot.BounceMode(1));
            yield return new WaitForSeconds(deltaTime);
        }
        yield return null;
    }


    /// <summary>
    /// 沿某条直线生成一系列子弹
    /// </summary>
    /// <param name="gun"></param>
    /// <param name="fireDir"></param>
    /// <param name="moveDir"></param>
    /// <param name="distance"></param>
    /// <param name="fire"></param>
    /// <param name="bullet"></param>
    /// <param name="speed"></param>
    public void FireLine(Vector3 gun,Vector3 fireDir,Vector3 moveDir,float distance,int fire,Transform bullet,float speed)
    {
        Vector3 firePoint = gun;
        Vector3 deltaFirePoint = distance * fireDir.normalized;
        for (int i = 0; i < fire; i++)
        {
            Shot shot = CreateBullet(firePoint, moveDir, bullet, speed).GetComponent < Shot>();
            // shot.SetCheckOut(false);
            firePoint = firePoint + deltaFirePoint;
        }
    }

    public IEnumerator FireLineGroup(Vector3 gun,Vector3 groupDir,Vector3 fireDir,Vector3 moveDir,float groupDistance,float fireDistance,int group,int fire,Transform bullet,float speed,float delay)
    {
        Vector3 firePoint = gun;
        Vector3 deltaFirePoint = groupDir.normalized * groupDistance;
        for(int i = 0; i < group; i++)
        {
            FireLine(firePoint ,fireDir ,moveDir ,fireDistance ,fire,bullet,speed);
            firePoint = firePoint + deltaFirePoint;
            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }

    public void CreateBounceBullet(Vector3 gun, Vector3 dir, float distance, Transform shotPrefab, float speed)
    {
        Shot bullet = CreateBullet(gun,  dir,distance, shotPrefab,speed).GetComponent <Shot >();
        bullet.lifetime = 99f;
        StartCoroutine(bullet.BounceMode());

    }

    public void FireAroundBounce(Vector3 gun, float startOffset,Transform bullet, int firePerRound,float speed,int bounceTime)
    {
        Vector3 bulletDir = Quaternion.AngleAxis(startOffset , Vector3.forward)*Vector3.up;
        Quaternion rotateQuate = Quaternion.AngleAxis(360/firePerRound,Vector3.forward );
        //List<Transform> bullets = new List<Transform>();       //装入开始生成弹幕
        for (int i = 0; i < firePerRound ; i++)
        {
            Shot  tempBullet = CreateBullet(gun, bulletDir, bullet, speed).GetComponent <Shot >();
            bulletDir = rotateQuate * bulletDir; //生成新的子弹后，让发射方向旋转，到达下一个发射方向
            tempBullet.lifetime = 99;
            StartCoroutine(tempBullet .BounceMode (bounceTime ));
        }
    }

    public IEnumerator FireBounceFirework(Vector3 gun, Vector3 dir, Transform prefab0, Transform prefab1, float speed0,float speed1,float angle,float lifetime1,float delay)
    {
        Shot bullet0 = CreateBullet(gun, dir, prefab0, speed0).GetComponent<Shot>();
        bullet0.lifetime = 25f;
        StartCoroutine(bullet0.BounceMode());

        Vector3 firePoint;
        Vector3 direction;
        float offset;

        while (bullet0 != null)
        {
            firePoint = bullet0.transform.position;
            direction = -bullet0.direction;
            offset = Random.Range(-angle ,angle);
            direction = Quaternion.AngleAxis(offset,Vector3.forward )* direction;
            Shot bullet1 = CreateBullet(firePoint ,direction ,prefab1,speed1).GetComponent <Shot >();
            bullet1.lifetime = lifetime1;
            yield return new WaitForSeconds(delay);
        }

        yield return null;
    }


    public IEnumerator FireRandomField(Vector3 gun,float deltaX,float deltaY,Vector3 dir,float speedMin,float speedMax,Transform bullet,int number)
    {
        Vector3 firePoint;
        float speed;

        for(int i = 0; i < number; i++)
        {
            firePoint = gun + new Vector3(Random .Range (0,deltaX ), Random.Range(0, deltaY));
            speed = Random.Range(speedMin, speedMax);
            Shot shot =CreateBullet(firePoint ,dir,bullet ,speed).GetComponent <Shot >();
            // shot.SetCheckOut(false);
        }

        yield return null;
    }

    public IEnumerator FireRandomSniper(Vector3 gun,Vector3 dir, float angle,float minSpeed,float maxSpeed,Transform bullet,int number)
    {
        float speed;
        Vector3 direction;

        for (int i = 0; i < number; i++){
            speed = Random.Range(minSpeed ,maxSpeed );
            direction = Quaternion.AngleAxis(Random.Range(-angle / 2, angle / 2), Vector3.forward) * dir;

            CreateBullet(gun,direction ,bullet ,speed );




        }

        yield return null;
    }


    public IEnumerator FireSin(Vector3 gun,Vector3 dir,float A,float T,Transform bullet,float speed,float deltaTime,float totalTime,bool positive)
    {
        Vector3 firePoint;
        Vector3 _x = Quaternion.AngleAxis(90,Vector3 .forward ) * dir;

        float omega = 2 * 3.14159f / T;

        float time = 0;
        while (time<totalTime )
        {
            if(positive)
            {
                firePoint = gun + A * Mathf.Sin(omega * time) * _x;
            }
            else
            {
                firePoint = gun - A * Mathf.Sin(omega * time) * _x;
            }
            

            Shot shot = CreateBullet(firePoint ,dir,bullet ,speed ).GetComponent <Shot >();
            // shot.SetCheckOut(false);
            time += Time.deltaTime;
            yield return new WaitForSeconds(deltaTime );
        }


        yield return null;
    }
}
