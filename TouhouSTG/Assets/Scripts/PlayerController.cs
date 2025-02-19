using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Danmaku.Data;
using System.Security.Permissions;

public class PlayerController: CharacterController  {

    [Header("移动")]
    [Tooltip("低速模式的移动速度倍率"), Range(0, 1f)] public float focusSpeedFactor;

    
    // private float focusFactor = 0.4f;

    private bool focus = false;

    // private Vector3 direction = new Vector3(1, 0,0);
    [Tooltip("高速模式的移动速度")] public float baseSpeed;
    private Vector3 speed;
    private Vector3 movement;

    private Rigidbody2D rb;
    // [SerializeField] private Animator animator;

    public static readonly float sqrt2 = Mathf.Sqrt(2);

    // private bool interactive = false;

    [Header("Bomb")]
    [Tooltip("Bomb动画持续时间")] public float bombDuration = 5f;
    [Tooltip("Bomb无敌时间")] public float bombInvincibleTime = 6f;
    private bool bombing = false;
    [SerializeField] private PlayerBomber bomber;

    [Header("射击")]
    [Tooltip("子弹发射点相对自机位置")] public Transform[] shootPos;
    [Tooltip("子弹发射方向")] public Vector3[] shootDir = {Vector3.right, new Vector3(1f,0,0.1f), new Vector3(1f, 0,-0.1f)};
    
    private ShotData shotData;

    
    [Header("判定")]
    // [SerializeField] private SpriteRenderer shootAura;
    [SerializeField] private SpriteRenderer hitPoint;
    [SerializeField] private SpriteRenderer magicCircle;
    [Tooltip("Miss后无敌时间")] public float missInvincibleTime = 2f;


    [Header("当前状态")]
    public int life;
    public int maxLife, startLife;
    public int bomb;
    public int maxBomb, startBomb;
    private bool invincible = false;
    private float invincibleTimer = 0f;



    public void SetInvincible(float duration){
        invincible = true;
        magicCircle.gameObject.SetActive(true);

        invincibleTimer = duration;
    }


    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();

        speed = new Vector2(baseSpeed, baseSpeed);
        shotData = new ShotData(0, 20);

        life = startLife;
        bomb = startBomb;

        // magicCircle.gameObject.SetActive(false);

    }

    void Update()
    {

        
        if(!interactive) return;
        // rb.velocity = new Vector2(0, 0);


        // 是否低速模式
        // 影响两点：射击方向和移动速度
        focus = Input.GetKey(KeyCode.LeftShift);
        hitPoint.color = focus ? Color.white : Color.clear;


        #region Movement

        float inputX = 0;
        float inputY = 0;

        bool right = Input.GetKey(KeyCode.RightArrow);
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
            
            // 低速模式
            if (focus)
            {
                movement = new Vector2(speed.x * inputX * focusSpeedFactor, speed.y * inputY * focusSpeedFactor);
            }

            // 位移
            transform.position = transform.position + movement * Time.deltaTime;
            //rigidbody2D.position = transform.position;


            //限制不离开屏幕
            var dist = (transform.position - Camera.main.transform.position).z;
            var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0.02f, 0, dist)).x;
            var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(0.98f, 0, dist)).x;
            var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.02f, dist)).y;
            var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.98f, dist)).y;

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
                Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
                transform.position.z
            );
        }
        #endregion

        #region Spell (test only)
        if(Input.GetKeyDown(KeyCode.X)){

            TryUseSpell();

            // if(spelling) animator.SetTrigger("SpellEnd");
            // else animator.SetTrigger("SpellStart");
            // spelling = !spelling;
        }
        #endregion

        #region Shoot (test only)
        if(Input.GetKey(KeyCode.Z)){
            // shootAura.color = Color.white;
            TryShoot();
        }
        else{
            // shootAura.color = Color.clear;
        }
        #endregion
    
        #region Invincible
        if(invincible){
            invincibleTimer -= Time.deltaTime;
            if(invincibleTimer <= 0f){
                invincible = false;
                magicCircle.gameObject.SetActive(false);
            }
        }
        #endregion
    }


    public override void StartBattle()
    {
        animator.SetTrigger("Intro");

        StartCoroutine(WaitForSeconds(1f, ()=>{
            gameObject.SetActive(true);
            SetInteractive(true);

            life = startLife;
            bomb = startBomb;

            hitPoint.gameObject.SetActive(true);
        }));

        
    }



    private float shootTimer;
    public float shootInvertal;
    // public float bulletSpeed;

    private void TryShoot(){

        if(bombing) return;

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

    public void TryUseSpell(){
        if(!bombing){
            if(bomb > 0){
                bomb --;
                
                StartCoroutine(UseSpell());

                GameMain.Instance.getSpellBonus = false;
            }
        }
    }

    private IEnumerator UseSpell(){
        
        bombing = true;
        animator.SetTrigger("SpellStart");
        SetInvincible(bombInvincibleTime);
        bomber.SetFire(true);

        yield return new WaitForSeconds(bombDuration);

        bombing = false;
        animator.SetTrigger("SpellEnd");
        bomber.SetFire(false);
        bombing = false;

        yield return null;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(0, 0);
    }

    public void Miss(){
        life -= 1;
        bomb = startBomb;

        // biu音效
        SFXManager.Instance.CreateSFX(3);

        // 动画
        animator.SetTrigger("Hit");
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
