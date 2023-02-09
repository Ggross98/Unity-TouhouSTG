using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotScript : MonoBehaviour {
   
    public int damage = 1;

    public float lifetime = 5;
    private float leftLifetime;

    public bool isEnemyShot = false;

    public float speed=5f;
    private float suspendTime = 0;


    public Vector3 direction;

    //private Rigidbody2D rigidbody2D;

    private bool isOutCheck = true;

    private bool isStraightMove=true,isCircularMove=false;

    private bool isBallModePlay=false;

    private bool isBouncing = false;

    void Start()
    {
        leftLifetime = lifetime;
        //Destroy(gameObject, lifetime);
        
    }

    void Awake()
    {
        //rigidbody2D = this.GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        if (isStraightMove)
        {
            if(suspendTime <= 0)
            {
                transform.position += direction * speed * Time.deltaTime;
            }
            else
            {
                suspendTime -= Time.deltaTime;
            }
           
        }
        
        if(!isBouncing&&isOutCheck )
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            if (screenPos.x <= 0 || screenPos.x >= Screen.width|| screenPos.y <= 0 || screenPos.y >= Screen.height)//左右边界
            {
                leftLifetime =0;
            }
        }

        leftLifetime -= Time.deltaTime;
        if(leftLifetime <= 0)
        {
            Destroy(gameObject );
        }
       

        //改变朝向

    }

    /// <summary>
    /// 生命结束后秒后自动回收到对象池
    /// </summary>
    /// <returns></returns>
    IEnumerator AutoRecycle()
    {
        
        leftLifetime = lifetime;
        //float leftLifetime = lifetime;

        /*
        while (leftLifetime > 0)
        {
            leftLifetime -= Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }*/

        yield return new WaitForSeconds(3);
        //ObjectPool.instance.RecycleObj(gameObject);
        //Reset();
        /*
        Debug.Log("I will recycle");
        yield return new WaitForSeconds(lifetime );
        Debug.Log(ObjectPool .instance  );
        */
    }

    public void Recycle()
    {
        leftLifetime = 0;
    }

    /*
    private void OnEnable()
    {
        StartCoroutine(AutoRecycle());
    }*/


    private void Reset()
    {
        isStraightMove = true;
        isCircularMove = false;

        isBallModePlay = false;
        isBouncing = false;

        //leftLifetime = lifetime;
}

    public void SetLifetime(float time)
    {
        lifetime = time;
        leftLifetime = lifetime;
    }







    /// <summary>
    /// 动态变换移动方向的移动模式
    /// </summary>
    /// <param name="endTime">移动结束时间</param>
    /// <param name="dirChangeTime"><方向转变时间</param>
    /// <param name="angle">方向改变的角度</param>
    /// <returns></returns>
    public IEnumerator DirChangeMoveMode(float endTime, float dirChangeTime, float angle)
    {
        float time = 0;
        bool isRotate = true;
        isBallModePlay = true;
        isStraightMove = false;
        while (isBallModePlay && this!=null)
        {
            time += Time.deltaTime;
            transform.position += speed * direction * Time.deltaTime;
            if (time >= dirChangeTime && isRotate)
            {
                isRotate = false;
                StartCoroutine(BulletRotate(angle));
            }

            yield return null;
        }
    }

    /// <summary>
    /// 弹幕动态改变移动方向
    /// </summary>
    IEnumerator BulletRotate(float angle)
    {
        while (isBallModePlay&&this!=null)
        {
            Quaternion tempQuat = Quaternion.AngleAxis(angle, Vector3.forward);
            direction = tempQuat * direction;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator CircularMove(Vector3 center,float angleSpeed)
    {
        float r;
        //float angled = 0f;
        isCircularMove = true;
        while(isCircularMove && this != null)
        {
            r = (center - transform.position).magnitude;

            //angled += (angleSpeed  * Time.deltaTime) % 360;//累加已经转过的角度

            /*
            float posX = r * (Mathf.Cos(angled) + angled * Mathf.Sin(angled));//计算x位置
            float posY = r * (Mathf.Sin(angled) - angled * Mathf.Cos(angled));//计算y位置
            transform.position = new Vector3(posX, posY,0) + center;//更新位置
            */
            bool clockwise = angleSpeed > 0;
            Vector3 a = r * angleSpeed*angleSpeed * GetNormalVector3(transform .position -center,clockwise );
            direction = (direction*speed +a).normalized;
            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }

    public IEnumerator CircularMove(Vector3 center, float distance,float angleSpeed)
    {
        isCircularMove = true;

        bool clockwise = angleSpeed > 0;
        while (isCircularMove && this != null)
        {
            
            Vector3 a = distance * angleSpeed * angleSpeed * GetNormalVector3(transform.position - center, clockwise);
            direction = (direction * speed + a).normalized;
            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }


    public IEnumerator Suspend(float time)
    {
        suspendTime = time;
        yield return null;
    }

    /// <summary>
    /// 在一定时间后暂停若干秒
    /// </summary>
    /// <param name="starttime"></param>
    /// <param name="pausetime"></param>
    /// <returns></returns>
    public IEnumerator Suspend(float starttime,float pausetime)
    {
        float timeSum = 0;
        while (this != null && timeSum < starttime )
        {
            timeSum += Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }
        if (this != null)
        {
            suspendTime = pausetime;
        }
        yield return null;
    }
    /// <summary>
    /// 子弹在一定时间后改变
    /// </summary>
    /// <param name="time"></param>
    /// <param name="newSpeed"></param>
    /// <param name="newDirection"></param>
    /// <returns></returns>
    public IEnumerator ChangeDirAndSpeed(float time,float newSpeed,Vector3 newDirection)
    {
        float timeSum = 0;
        while (this != null && timeSum < time)
        {
            timeSum += Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }

        if (this != null)
        {

            speed = newSpeed;
            direction = newDirection;
        }
        yield return null;
    }

    public IEnumerator DelaySniper(float time, float newSpeed,Transform player)
    {
        float timeSum = 0;
        while (this != null && timeSum < time)
        {
            timeSum += Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }

        if (this != null)
        {

            speed = newSpeed;
            if (player != null)
            {
                direction = (player.position - transform.position).normalized;
            }
        }
        yield return null;
    }

    public IEnumerator BounceMode()
    {
        //世界坐标转为屏幕坐标
        Vector3 screenPos;
        isBouncing = true;

        while (this != null)
        {
            screenPos = Camera.main.WorldToScreenPoint(transform.position);
            if (screenPos.x <= 0 || screenPos.x >= Screen.width)//左右边界
            {
                direction = new Vector3(-direction .x,direction.y,0);
            }
            if (screenPos.y <=0|| screenPos.y >= Screen.height)//上下边界
            {
                direction = new Vector3(direction.x, -direction.y, 0);
            }
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }

    public IEnumerator BounceMode(int time)
    {
        int t = 0;
        isBouncing = true;
        //世界坐标转为屏幕坐标
        Vector3 screenPos;

        while (this != null&&isBouncing )
        {
            screenPos = Camera.main.WorldToScreenPoint(transform.position);
            if (screenPos.x <= 0 || screenPos.x >= Screen.width)//左右边界
            {
                direction = new Vector3(-direction.x, direction.y, 0);
                t++;
            }
            if (screenPos.y <= 0 || screenPos.y >= Screen.height)//上下边界
            {
                direction = new Vector3(direction.x, -direction.y, 0);
                t++;
            }
            if (t >= time) isBouncing  = false;

            yield return new WaitForSeconds(0.05f);
        }
        while (this != null)
        {
            screenPos = Camera.main.WorldToScreenPoint(transform.position);
            if (screenPos.x <= 0 || screenPos.x >= Screen.width || screenPos.y <= 0 || screenPos.y >= Screen.height)
            {
                Destroy(gameObject);
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }



    public void ChangeDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    public Vector3 GetNormalVector3(Vector3 v,bool clockwise)
    {
        Vector3 target = v.normalized;
        Vector3 result=new Vector3(1,0,0);
        if (target.y == 0)
        {
            result = new Vector3(0, 1, 0);
        }
        else
        {
            result.y = -target.x / target.y;
        }
        if(clockwise)
        {
            if (target.y > 0)
            {
                return result.normalized;
            }
            else
            {
                return -(result.normalized );
            }
        }
        else
        {
            if (target.y > 0)
            {
                return -result.normalized;
            }
            else
            {
                return (result.normalized);
            }
        }
        //return result;
    }

    /// <summary>
    /// 弹幕动态改变移动方向
    /// </summary>
    IEnumerator BulletRotate(float angle,float delay)
    {
        while (this != null)
        {
            Quaternion tempQuat = Quaternion.AngleAxis(angle, Vector3.forward);
            direction = tempQuat * direction;
            yield return new WaitForSeconds(delay);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        LaserClearBullet laser = collider.GetComponent<LaserClearBullet>();
        /*
        if (laser != null&&laser.IsBombing ())
        {
            Destroy(gameObject);
        }*/
    }

    #region Getters and Setters

    public void SetSpeed(float s)
    {
        speed = s;
    }

    public void SetDirection(Vector3 s)
    {
        direction = s;
    }

    public bool IsStraightMove()
    {
        return isStraightMove;
    }

    public void SetStraightMove(bool b)
    {
        isStraightMove = b;
    }

    public bool IsCircularMove()
    {
        return isCircularMove;
    }

    public void SetCircularMove(bool b)
    {
        isCircularMove = b;
    }

    public void SetCheckOut(bool b)
    {
        isOutCheck = b;
    }
    #endregion
}
