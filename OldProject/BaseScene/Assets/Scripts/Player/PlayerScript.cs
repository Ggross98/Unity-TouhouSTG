using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : PauseObject  {
    

    public Input_Key laser;

    //基础属性
    public int startLife = 2, startBomb = 2;
    private int life, bomb,score=0;

   // private Vector2 speed = new Vector2(18, 18);
    
    private Vector2 movement;

    private Rigidbody2D rigidbody2D;
    
    //控制闪烁
    private float blinkTime=0;

    //检测碰撞
    private bool collision = false;

    //决死
    bool missing = false;
    private float missingTime;

    //保护时间
    private bool isProtected = false;
    private float protectTime = 0;


    //渲染组件，控制闪烁
    private SpriteRenderer renderer;

    void Awake()
    {
        rigidbody2D = this.GetComponent<Rigidbody2D>();
        renderer = this.GetComponent<SpriteRenderer>();
        life = startLife;
        bomb = startBomb;
    }
    
    public int GetLife()
    {
        return life;
    }

    public void Bomb()
    {
        if (bomb > 0)
        {
            laser.Bomb(3.5f);
            //SoundEffectHelper.Instance.MakeLaserSound();
            Protect(4f);
            Blink(3f);
            bomb--;
        }

    }



    public void Protect(float time)
    {
        //PlayerLifeScript life =gameObject.GetComponent<PlayerLifeScript>();
        //life.Protect(time);

        //isProtected = true;
        protectTime = time;
    }

    public void Miss()
    {
        if (!missing)
        {
            SoundEffectHelper.Instance.MakeMissSound();
            missing = true;
            missingTime = 0.15f;
            /*
            if (life >= 0)
            {
                bomb = startBomb;
            }
            life--;*/

        }
        
    }

    public int GetBomb()
    {
        return bomb;
    }

    public void Blink(float time)
    {
        blinkTime = time;
    }
    
    void Update()
    {

        if (_isPause) return;

        #region 射击控制

        // 5 - 射击
        bool shoot = Input.GetKey(KeyCode .Z );
        // 小心：对于Mac用户，按Ctrl +箭头是一个坏主意

        if (shoot)
        {
            AttackScript weapon = GetComponent<AttackScript>();
            if (weapon != null)
            {
                weapon.Attack(false);
               // SoundEffectHelper.Instance.MakePlayerShotSound();
            }
        }

        #endregion

        #region bomb释放
        bool bombrelease = Input.GetKey(KeyCode.X)&&(! laser.IsBombing ());
        if (bombrelease)
        {
            //PlayerLifeScript life = gameObject.GetComponent<PlayerLifeScript>();
            if(protectTime <= 0)
            {
                if (missing)
                {
                    missing = false;
                }

                Bomb();
            }
            
        }

        #endregion

        #region 决死与死亡
        if (missingTime > 0)
            missingTime -= Time.deltaTime;
        if(missing&&missingTime <= 0)
        {
            life--;
            if (life >= 0)
            {
                bomb = startBomb;
                Protect(2.5f);
                Blink(2f);
            }
            else
            {
                Destroy(gameObject);
            }
            missing = false;
        }

        #endregion

        #region 闪烁
        if (blinkTime  > 0)
        {
            if(blinkTime %0.2 > 0.1f)
            {
                renderer.enabled = true;
            }
            else
            {
                renderer.enabled = false;
            }

            blinkTime -= Time.deltaTime;
        }
        else
        {
            renderer.enabled = true;
        }


        #endregion

        #region 保护
        if (protectTime > 0)
            protectTime -= Time.deltaTime;
        isProtected = (protectTime > 0);

        if (collision && (!isProtected))
        {
            Miss();
            
            collision = false;
        }
        #endregion

        //低速
        bool focus = Input.GetKey(KeyCode.LeftShift);
        this.transform.Find("point").gameObject.SetActive(focus);

        //魔法阵
        this.transform.Find("魔法阵").gameObject.SetActive(laser.IsBombing());


    }

    void FixedUpdate()
    {
        rigidbody2D.velocity = movement;
    }

    void OnDestroy()
    {
        // Game Over.
        // Add the script to the parent because the current game
        // object is likely going to be destroyed immediately.
        SpecialEffectsHelper.Instance.Explosion (transform.position);
        SpecialEffectsHelper.Instance.ClearPlayerBullet(); 
        //transform.parent.gameObject.AddComponent<GameOverScript>();
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {

        if (isProtected) return;
        if (collision) return;

        ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
        if (shot != null)
        {
            // 判断子弹归属,避免误伤
            if (shot.isEnemyShot)
            {
                SpecialEffectsHelper.Instance.Hit(shot.transform.position);
                collision = true;
            }
        }
    }

    void OnColliderEnter2D(Collider2D otherCollider)
    {
        if (isProtected) return;
        if (collision) return;

        EnemyScript enemy = otherCollider.gameObject.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            // 判断子弹归属,避免误伤
           SpecialEffectsHelper.Instance.Hit(enemy.transform.position);
           collision = true;
            
        }
    }
}
