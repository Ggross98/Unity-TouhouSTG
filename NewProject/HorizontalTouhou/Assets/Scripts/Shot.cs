using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Danmaku.Data;

public class Shot : MonoBehaviour {

    // [SerializeField] private SpriteRenderer shotRenderer;
   
    // public ShotData data;

    public int damage = 1;

    public float lifetime = 5f;
    private float leftLifetime;

    public bool enemyShot = false;

    
    private float speed;
    private float baseSpeed, minSpeed, maxSpeed, deltaSpeed;

    private float angle;
    private float baseAngle, minAngle, maxAngle, deltaAngle;


    private float suspendTime = 0;

    public Vector3 direction;
    public Vector3 startDirection;

    //private Rigidbody2D rigidbody2D;

    // private bool outOfScreen = true;

    // private bool isStraightMove=true, isCircularMove=false;
    // private bool isBallModePlay=false;
    
    public MoveMode moveMode = MoveMode.Straight;
    public WallCheckMode wallCheckMode = WallCheckMode.Clear;
    

    private bool moving = false;

    private System.Action<Shot> deactiveAction;

    public void SetDeactiveAction(System.Action<Shot> action){
        this.deactiveAction = action;
    }

    public void SetSpeed(LimitedValue v, float delta){
        baseSpeed = v.value;
        minSpeed = v.min;
        maxSpeed = v.max;
        deltaSpeed = delta;

        speed = baseSpeed;
    }

    public void SetAngle(LimitedValue v, float delta){
        baseAngle = v.value;
        minAngle = v.min;
        maxAngle = v.max;
        deltaAngle = delta;

        angle = baseAngle;
    }

    public void LoadData(ShotData sd){
        if(sd.lifetime > 5) lifetime = sd.lifetime;
        SetSpeed(sd.speed, sd.deltaSpeed);
        SetAngle(sd.angle, sd.deltaAngle);
    }

    private void Init(Vector3 pos, Vector3 dir, bool enemy){
        // 位置
        transform.position = pos;

        // 速度
        // this.speed = baseSpeed;

        // 方向
        this.startDirection = dir.normalized;
        this.direction = startDirection;
        // this.angle = baseAngle;
        Toward();

        this.enemyShot = enemy;

        StartMove(dir);

    }

    public void Init(Vector3 pos, Vector3 dir, bool enemy, ShotData sd){
        LoadData(sd);
        Init(pos, dir, enemy);
    }

    public void StartMove(Vector3 dir){

        // moveMode = MoveMode.Straight;
        // this.direction = dir;

        leftLifetime = lifetime;
        moving = true;
    }

    public void Kill(){
        leftLifetime = 0f;
    }

    public void TouchWall()
    {
        switch(wallCheckMode){
            case WallCheckMode.Clear:
                leftLifetime = 0f;
                break;
        }
    }

    public void TouchCharacter()
    {
        leftLifetime = 0;
        VFXManager.Instance.CreateVFX(0, transform.position);
    }

    public void ChangeSpeed(float delta){
        speed += delta;
        if(speed < minSpeed) speed = minSpeed;
        if(speed > maxSpeed) speed = maxSpeed;
    }

    public void ChangeAngle(float delta){
        angle += delta;
        if(angle < minAngle) angle = minAngle;
        if(angle > maxAngle) angle = maxAngle;

        Quaternion offset = Quaternion.AngleAxis(angle, Vector3.forward);
        direction = offset * startDirection;
    }

    void FixedUpdate()
    {
        if(!moving) return;

        if(suspendTime > 0){
            suspendTime -= Time.deltaTime;
            return;
        }


        
        switch(moveMode){
            case MoveMode.Straight:

                ChangeSpeed(deltaSpeed * Time.deltaTime);
                ChangeAngle(deltaAngle * Time.deltaTime);
                

                transform.position += direction * speed * Time.deltaTime;
                break;
        }

        // if (isStraightMove)
        // {
        //     if(suspendTime <= 0)
        //     {
        //         transform.position += direction * baseSpeed * Time.deltaTime;
        //     }
        //     else
        //     {
        //         suspendTime -= Time.deltaTime;
        //     }
           
        // }
        
        // if(!isBouncing && outOfScreen )
        // {
        //     Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        //     if (screenPos.x <= 0 || screenPos.x >= Screen.width|| screenPos.y <= 0 || screenPos.y >= Screen.height)//左右边界
        //     {
        //         leftLifetime =0;
        //     }
        // }

        leftLifetime -= Time.deltaTime;
        if(leftLifetime <= 0)
        {
            // gameObject.SetActive(false);
            deactiveAction.Invoke(this);
        }
       

        //改变朝向
        Toward();
    }

    private void Toward(){
        float eulerAngle = 0;
        if (direction.x> 0)
        {
            eulerAngle = -Vector3.Angle(Vector3.up, direction.normalized);
        }
        else
        {
            eulerAngle = Vector3.Angle(Vector3.up, direction.normalized);
        }
        transform.eulerAngles = new Vector3(0, 0, eulerAngle);
    }

    /*
    private void OnEnable()
    {
        StartCoroutine(AutoRecycle());
    }*/


    private void Reset()
    {
        // isStraightMove = true;
        // isCircularMove = false;

        // isBallModePlay = false;
        // isBouncing = false;

        //leftLifetime = lifetime;
    }

    public void SetLifetime(float time)
    {
        lifetime = time;
        leftLifetime = lifetime;
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

    #endregion
}