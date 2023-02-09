using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Danmaku.Data;

public class PlayerController: CharacterController  {

    
    public float focusSpeedFactor;

    
    // private float focusFactor = 0.4f;

    private bool focus = false;

    // private Vector3 direction = new Vector3(1, 0,0);
    public float baseSpeed;
    private Vector3 speed;
    private Vector3 movement;

    [SerializeField] private Rigidbody2D rb;
    // [SerializeField] private Animator animator;

    public static readonly float sqrt2 = Mathf.Sqrt(2);

    // private bool interactive = false;
    private bool testSpelling = false;


    public Transform[] shootPos;
    public Vector3[] shootDir = {Vector3.right, new Vector3(1f,0,0.1f), new Vector3(1f, 0,-0.1f)};
    private ShotData shotData;
    
    [SerializeField] private SpriteRenderer shootAura;
    [SerializeField] private SpriteRenderer hitPoint;

    [SerializeField] private GameObject magicCircle;
    private bool invincible = false;
    private float invincibleTimer = 0f;
    public float missInvincibleTime = 2f;


    public int life;
    public int maxLife, startLife;
    public int bomb;
    public int maxBomb, startBomb;



    public void SetInvincible(float duration){
        invincible = true;
        magicCircle.SetActive(true);

        invincibleTimer = duration;
    }


    void Awake()
    {
        // rb = this.GetComponent<Rigidbody2D>();
        speed = new Vector2(baseSpeed, baseSpeed);
        shotData = new ShotData(0, 20);


        life = startLife;
        bomb = startBomb;
    }

    void Update()
    {

        
        if(!interactive) return;
        // rb.velocity = new Vector2(0, 0);


        //低速
        focus = Input.GetKey(KeyCode.LeftShift);
        hitPoint.color = focus ? Color.white : Color.clear;


        #region Movement

        float inputX=0;
        float inputY=0;

        bool right = Input.GetKey(KeyCode.RightArrow );
        bool left = Input.GetKey(KeyCode.LeftArrow);
        bool up = Input.GetKey(KeyCode.UpArrow);
        bool down = Input.GetKey(KeyCode.DownArrow);

        if (right) inputX = 1;
        if (left) inputX = -1;
        if (up) inputY = 1;
        if (down) inputY = -1;

        var moving = inputX != 0 || inputY != 0;
        // Animation
        // animator.SetBool("Moving", moving);
        animator.SetInteger("Horizontal", (int)inputX);
        
        if (moving)
        {

            // Animation
            // animator.SetBool("Moving", moving);
            // animator.SetInteger("Horizontal", (int)inputX);

            // 对角线移动修正
            if(inputX * inputY != 0){
                inputX *= sqrt2 / 2;
                inputY *= sqrt2 / 2;
            }

            // 速度
            movement = new Vector2(speed.x * inputX, speed.y * inputY);
            

            
            if (focus)
            {
                movement = new Vector2(speed.x * inputX * focusSpeedFactor, speed.y * inputY * focusSpeedFactor);
            }


            transform.position = transform.position + movement * Time.deltaTime;
            //rigidbody2D.position = transform.position;


            //限制不离开屏幕
            var dist = (transform.position - Camera.main.transform.position).z;
            var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
            var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
            var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
            var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
                Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
                transform.position.z
            );
        }
        #endregion

        #region Spell (test only)
        if(Input.GetKeyDown(KeyCode.X)){
            if(testSpelling) animator.SetTrigger("SpellEnd");
            else animator.SetTrigger("SpellStart");

            testSpelling = !testSpelling;
        }
        #endregion

        #region Shoot (test only)
        if(Input.GetKey(KeyCode.Z)){
            shootAura.color = Color.white;
            TryShoot();
        }
        else{
            shootAura.color = Color.clear;
        }
        #endregion
    
        #region Invincible
        if(invincible){
            invincibleTimer -= Time.deltaTime;
            if(invincibleTimer <= 0f){
                invincible = false;
                magicCircle.SetActive(false);
            }
        }
        #endregion
    }

    public override void StartBattle()
    {
        gameObject.SetActive(true);
        SetInteractive(true);

        life = startLife;
        bomb = startBomb;
    }



    float shootTimer;
    public float shootInvertal;
    public float bulletSpeed;

    private void TryShoot(){
        shootTimer -= Time.deltaTime;
        if(shootTimer <= 0){
            shootTimer = shootInvertal;
            DoShoot();
        }
    }

    private void DoShoot(){
        // SFXManager.Instance.CreateSFX(0);
        for(int i = 0; i < shootDir.Length; i++){
            var dir = focus ? Vector3.right : shootDir[i];
            ShotManager.Instance.CreateShot(shotData, shootPos[i].position, dir, false);
        }
    }


    void FixedUpdate()
    {
        rb.velocity = new Vector2(0, 0);
    }

    public void Miss(){
        life -= 1;

        // biu音效
        SFXManager.Instance.CreateSFX(3);
    }

    public void Hit(){
        if(invincible) return;

        Miss();

        if(life > 0){
            SetInvincible(missInvincibleTime);
        }   
        else{
            VFXManager.Instance.CreateVFX(1, transform.position);
            gameObject.SetActive(false);
            GameMain.Instance.GameOver(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        var shot = other.GetComponent<Shot>();

        if(shot != null && shot.enemyShot){
            shot.TouchCharacter();
            Hit();
        }
    }
}
