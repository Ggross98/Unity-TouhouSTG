using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Danmaku.Data;

public class EnemyController : CharacterController
{

    // [SerializeField] private Barrage[] barrages;
    // private BarrageData[] barrageData;

    private BarrageManager barrageManager;
    private List<Barrage> barrages;



    [SerializeField]
    private List<DanmakuData> danmakuList;
    private int danmakuIndex;
    // private DanmakuData danmaku;


    // [SerializeField] private Animator animator;
    // [SerializeField] private float minX = 3f, maxX = 6f;
    // [SerializeField] private float minY = -3f, maxY = 3f;
    [SerializeField] private Transform player;


    // [SerializeField] private FilledRing hpRing;


    Vector3 centerPosition = new Vector3(5.5f, 0, 0);

    // float timer, interval = 3.5f;
    // int counter = 0;

    #region 弹幕中移动
    private float moveTimer;
    private DanmakuMove move;
    #endregion

    public int spellLife, spellMaxLife;
    // float spellTime, spellMaxTime;

    #region 弹幕间移动
    [SerializeField] float spellInterval = 1.5f;
    #endregion

    private bool spelling = false;
    // private bool interactive = false;

    [SerializeField] private Transform magicCircle;
    [SerializeField] private ShotEraser shotEraser;

    private void Awake()
    {
        barrageManager = BarrageManager.Instance;
    }

    private void Start()
    {

    }

    // private static ShotData sd = new ShotData(2, 6f);

    private void Update()
    {
        if (!interactive) return;

        if (spelling)
        {

            // 发射
            var danmaku = danmakuList[danmakuIndex];
            for (int i = 0; i < danmaku.data.Count; i++)
            {
                var barrage = barrages[i];
                var data = danmaku.data[i];

                barrage.timer -= Time.deltaTime;
                if (barrage.timer <= 0)
                {


                    if (danmaku.animation == DanmakuAnimation.AttackBeforeFire)
                    {
                        animator.SetTrigger("Attack");
                        StartCoroutine(WaitForSeconds(0.4f, () =>
                        {
                            InvokeBarrage(barrage, data);
                        }));
                    }
                    else
                    {
                        InvokeBarrage(barrage, data);
                    }


                    barrage.timer = data.interval;
                }
            }

            // 移动
            // var move = danmaku.move;
            if (move.type == DanmakuMoveType.RandomMove)
            {

                moveTimer -= Time.deltaTime;

                if (moveTimer <= 0)
                {
                    // 移动
                    var x = Random.Range(move.minX, move.maxX);
                    var y = Random.Range(move.minY, move.maxY);
                    var pos = new Vector3(x, y) + move.startPosition;
                    MoveTo(pos, move.duration, move.delay);

                    moveTimer = move.interval;
                }
            }


            // 时符：按时间掉血
            if (danmaku.type == DanmakuType.SurvivalCard)
            {
                spellLife -= 1;
            }
            else{
                if(hitByBomb){
                    spellLife -= 1;
                }
            }

            // 符卡结束判定
            if (spellLife == 0)
            {
                EndDanmaku();
            }
            

        }


    }


    public override void StartBattle()
    {

        animator.SetTrigger("Intro");

        StartCoroutine(WaitForSeconds(1f, ()=>{

            SetInteractive(true);
            
            danmakuIndex = 0;
            StartDanmaku();

            
        }));

        
    }

    [SerializeField] private float startDanmakuMoveDelay = 0.1f;
    [SerializeField] private float startDanmakuMoveDuration = 1f;
    [SerializeField] private float hpRingFillDuration = 0.5f;
    [SerializeField] private float startDanmakuIdleDuration = 0.1f;

    private void StartDanmaku()
    {
        var danmaku = danmakuList[danmakuIndex];

        // 刷新移动信息
        move = danmaku.move;
        moveTimer = move.startDelay;

        // 移动至初始位置
        var distance = (transform.position - move.startPosition).magnitude;
        
        var waitTime = startDanmakuMoveDelay;
        if(distance > 0.5f){
            MoveTo(move.startPosition, startDanmakuMoveDuration, startDanmakuMoveDelay);
            waitTime = startDanmakuMoveDuration + startDanmakuMoveDelay;
        }

        // 符卡展开音效
        SFXManager.Instance.CreateSFX(4);

        // 加载弹幕信息
        var barrageData = danmaku.data;
        // Debug.Log(danmaku);
        // Debug.Log(danmaku.data);

        // 弹幕类型和生命值
        spellMaxLife = danmaku.hp;
        spellLife = spellMaxLife;
        switch (danmaku.type)
        {
            case DanmakuType.SpellCard:                 // 符卡
                
                StartCoroutine(ShowMagicCircle(1f, 2f));
                animator.SetTrigger("SpellCard");
                break;

            case DanmakuType.NonSpellCard:              // 非符
                // spellMaxLife = danmaku.hp;
                // spellLife = spellMaxLife;
                break;

            case DanmakuType.SurvivalCard:              // 时符
                // spellMaxTime = danmaku.survivalTime;
                // spellTime = spellMaxTime;
                // spellMaxLife = danmaku.hp;
                // spellLife = spellMaxLife;
                StartCoroutine(ShowMagicCircle(1f, 2f));
                animator.SetTrigger("SpellCard");
                break;
        }

        // 装填弹幕
        barrages = new List<Barrage>();

        for (int i = 0; i < barrageData.Count; i++)
        {
            barrages.Add(barrageManager.CreateBarrage());

            var data = barrageData[i];
            var barrage = barrages[i];

            barrage.transform.position = transform.position;
            barrage.SetShotData(data.shotData);
            barrage.timer = data.startDelay;
            barrage.counter = 0;
        }

        // 显示符卡名
        GameUI.Instance.ShowSpellName(danmaku.danmakuName);

        StartCoroutine(WaitForSeconds(waitTime + startDanmakuIdleDuration, () =>
        {
            // 显示符卡生命值
            var ringColor = danmaku.type == DanmakuType.SurvivalCard ? 1 : 0;
            GameUI.Instance.FillHPRing(hpRingFillDuration, ringColor);
        }));

        StartCoroutine(WaitForSeconds(waitTime + startDanmakuIdleDuration + hpRingFillDuration, () => {
            spelling = true;
        }));

        GameMain.Instance.StartDanmaku();
    }




    private void EndDanmaku()
    {
        GameMain.Instance.EndDanmaku(danmakuList[danmakuIndex].type == DanmakuType.SpellCard);

        // 清除弹幕
        shotEraser.EraseScreen(transform.position);

        // 刷新弹幕发射器
        foreach (var barrage in barrages)
        {
            barrage.Deactive();
        }
        barrages.Clear();

        spelling = false;


        // 动画显示
        GameUI.Instance.HideHPRing();
        StartCoroutine(BreakMagicCircle());

        danmakuIndex++;
        if (danmakuIndex == danmakuList.Count)
        {

            // 击破光效
            VFXManager.Instance.CreateVFX(1, transform.position);

            // 击破音效
            SFXManager.Instance.CreateSFX(2);

            // 游戏胜利
            GameMain.Instance.GameOver(true);
        }
        else
        {

            // 收取符卡音效
            SFXManager.Instance.CreateSFX(1);

            // 继续下一张符卡
            animator.SetTrigger("GuardBreak");


            StartCoroutine(WaitForSeconds(spellInterval, () =>
            {
                StartDanmaku();
            }));

        }

        
    }

    private void InvokeBarrage(Barrage barrage, BarrageData barrageData)
    {
        var fireData = barrageData.fireData;


        // 计算每轮射击改变的参数

        // 计算初始偏移的改变量
        float deltaStartAngle = 0f;

        var fireOffset = barrageData.fireOffset;
        if (fireOffset.type != OffsetType.None && fireOffset.cycle != 0)
        {

            var cycleIndex = barrage.counter % fireOffset.cycle;
            // 往复运动
            if (fireOffset.reciprocate)
            {
                if ((barrage.counter / fireOffset.cycle) % 2 == 1)
                {
                    cycleIndex = fireOffset.cycle - cycleIndex;
                }
            }

            var delta = fireOffset.range / fireOffset.cycle;
            cycleIndex -= fireOffset.startCycleIndex;

            switch (fireOffset.type)
            {
                case OffsetType.Linear:
                    deltaStartAngle = cycleIndex * delta;
                    break;
            }

            // fireData.startAngle += deltaStartAngle;
        }



        switch (fireData.type)
        {
            case FireType.Round:
                // var frd = (FireRoundData)fireData;
                StartCoroutine(FireRound(barrage, fireData, deltaStartAngle));
                break;
            case FireType.Sector:
                StartCoroutine(FireSector(barrage, fireData, deltaStartAngle));
                break;
            case FireType.Spray:
                StartCoroutine(FireSpray(barrage, fireData, deltaStartAngle));
                break;

        }


        barrage.firing = true;
        barrage.counter += 1;

    }

    // private IEnumerator WaitForSeconds(float duration, System.Action action){
    //     yield return new WaitForSeconds(duration);
    //     action.Invoke();
    // }

    private void MoveTo(Vector3 target, float duration, float startDelay)
    {

        if (transform.position == target) return;

        var horizont = 0;
        if (target.x < transform.position.x) horizont = -1;
        else if (target.x > transform.position.x) horizont = 1;

        var tween = transform.DOMove(target, duration);
        tween.OnPlay(() =>
        {
            animator.SetInteger("Horizont", horizont);
        });
        tween.OnComplete(() =>
        {
            animator.SetInteger("Horizont", 0);
        });

        Sequence seq = DOTween.Sequence();
        seq.Insert(startDelay, tween);
        seq.Append(transform.DOMove(target, duration));

        seq.Play();

    }

    public Vector3 GetPlayerPosition()
    {
        return player.position;
    }

    private IEnumerator ShowMagicCircle(float duration, float maxSize)
    {

        magicCircle.localScale = new Vector3(0, 0, 0);
        magicCircle.gameObject.SetActive(true);
        float size = 0f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            size = maxSize / duration * timer;
            magicCircle.localScale = Vector3.one * size;

            yield return new WaitForFixedUpdate();

        }


        yield return null;
    }

    private IEnumerator BreakMagicCircle()
    {
        magicCircle.gameObject.SetActive(false);
        yield return null;
    }

    public void Hit()
    {
        if (danmakuList[danmakuIndex].type == DanmakuType.SurvivalCard) return;

        // 击中音效
        SFXManager.Instance.CreateSFX(0, 0.1f);

        spellLife -= 1;
        if (spellLife < 0) spellLife = 0;
    }

    private bool hitByBomb = false;
    private void OnTriggerEnter2D(Collider2D other)
    {

        // 判定被玩家的弹幕击中
        var shot = other.GetComponent<Shot>();

        if (shot != null && !shot.enemyShot)
        {
            if (spelling)
            {
                shot.TouchCharacter();
                Hit();
            }

        }

        // 判定被玩家的Bomb击中
        // var eraser = other.GetComponent<ShotEraser>();
        // if(eraser != null && eraser.erasing && eraser.playerBomb){
        //     // Hit();
        //     hitByBomb = true;
        // }
    }

    private void OnTriggerStay2D(Collider2D other){

        // 判定被玩家的Bomb击中
        var eraser = other.GetComponent<ShotEraser>();
        if(eraser != null && eraser.erasing && eraser.playerBomb){
            Hit();
        }
    }

    // private void OnTriggerExit2D(Collider2D other){

    //     // 判定被玩家的Bomb击中
    //     var eraser = other.GetComponent<ShotEraser>();
    //     if(eraser != null && eraser.playerBomb){
    //         hitByBomb = false;
    //     }
    // }


    public void StartDelayOperation(List<DelayOperation> operations, List<Shot> shots){
        foreach(var operation in operations){
            StartCoroutine(WaitForSeconds(operation.delay, ()=>{
                InvokeShotOperation(operation, shots);
            }));
        }
    }

    private void InvokeShotOperation(DelayOperation operation, List<Shot> shots){

        foreach(var shot in shots){
            if(shot == null || !shot.isActiveAndEnabled) continue;

            switch(operation.type){
                case ShotOperationType.ChangeDirectionAndSpeed:
                    // 方向
                    switch(operation.directionType){
                        case DirectionType.Fixed:
                            shot.startDirection = operation.direction;
                            break;
                        case DirectionType.Aimed:
                            shot.startDirection = (player.transform.position - shot.transform.position).normalized;
                            break;
                        case DirectionType.Random:
                            shot.startDirection = Utils.GetRandomDirection();
                            break;
                    }

                    // 速度
                    shot.SetSpeed(operation.speed, operation.deltaSpeed);

                    // 角度
                    shot.SetAngle(operation.angle, operation.deltaAngle);

                    break;
            }
        }
        
    }


    // *********************************************************************************************

    #region 弹幕实现


    
    private IEnumerator FireRound(Barrage barrage, FireData data, float deltaStartAngle)
    {
        var group = data.group;

        // 组内射击间隔计时
        float interval = group.interval;
        float timer = interval;

        var shotList = new List<Shot>();

        // 子弹方向和速度
        var angle = data.startAngle + deltaStartAngle;
        var dist = data.startDistance;
        var count = data.count;
        Vector3 startDir = Vector3.zero;
        switch (data.directionType)
        {
            case DirectionType.Fixed:
                startDir = data.startDir;
                break;
            case DirectionType.Aimed:
                startDir = player.transform.position - transform.position;
                break;
            case DirectionType.Random:
                startDir = Utils.GetRandomDirection();
                break;

        }
        startDir = startDir.normalized;
        // var startDir = data.aimed ? player.transform.position - transform.position : data.startDir;

        // 发射点位置
        var posDir = data.posDir.normalized;
        var posDistance = data.posStartDistance;

        for (int i = 0; i < group.num; i++)
        {

            while (timer < interval)
            {
                timer += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            var deltaPos = posDir * posDistance;

            shotList.AddRange(barrage.FireRound(startDir, angle, count, dist, deltaPos));
            timer = 0;

            // 组内改变的参数
            angle += group.deltaAngle;
            dist += group.deltaDistance;
            posDir = Quaternion.AngleAxis(group.posDeltaAngle, Vector3.forward) * posDir;
            posDistance += group.posDeltaDistance;

        }
        barrage.firing = false;


        // if (data.shotOperationDelay > 0 && data.shotOperation != null)
        // {
        //     yield return new WaitForSeconds(data.shotOperationDelay);
        //     data.shotOperation(shotList);
        // }
        if(data.delayOperations.Count > 0){
            StartDelayOperation(data.delayOperations, shotList);
        }

        yield return null;

    }

    private IEnumerator FireSector(Barrage barrage, FireData data, float deltaStartAngle)
    {
        var group = data.group;

        // 组内射击间隔计时
        float interval = group.interval;
        float timer = interval;

        var shotList = new List<Shot>();

        // 子弹方向和速度
        var angle = data.startAngle + deltaStartAngle;
        var dist = data.startDistance;
        var count = data.count;
        var sectorAngle = data.sector.deltaAngle;
        // var startDir = data.aimed ? player.transform.position - transform.position : data.startDir;

        // 发射点位置
        var posDir = data.posDir.normalized;
        var posDistance = data.posStartDistance;

        for (int i = 0; i < group.num; i++)
        {

            while (timer < interval)
            {
                timer += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            var deltaPos = posDir * posDistance;
            Vector3 startDir = Vector3.zero;
            switch (data.directionType)
            {
                case DirectionType.Fixed:
                    startDir = data.startDir;
                    break;
                case DirectionType.Aimed:
                    startDir = player.transform.position - transform.position;
                    break;
                case DirectionType.Random:
                    startDir = Utils.GetRandomDirection();
                    break;

            }
            startDir = startDir.normalized;
            shotList.AddRange(barrage.FireSector(startDir, angle, count, sectorAngle, dist, deltaPos));
            timer = 0;

            // 组内改变的参数
            angle += group.deltaAngle;
            dist += group.deltaDistance;
            posDir = Quaternion.AngleAxis(group.posDeltaAngle, Vector3.forward) * posDir;
            posDistance += group.posDeltaDistance;

        }
        barrage.firing = false;


        // if (data.shotOperationDelay > 0 && data.shotOperation != null)
        // {
        //     yield return new WaitForSeconds(data.shotOperationDelay);
        //     data.shotOperation(shotList);
        // }
        if(data.delayOperations.Count > 0){
            StartDelayOperation(data.delayOperations, shotList);
        }

        yield return null;
    }

    private IEnumerator FireSpray(Barrage barrage, FireData data, float deltaStartAngle){
        var group = data.group;

        // 组内射击间隔计时
        float interval = group.interval;
        float timer = interval;

        var shotList = new List<Shot>();

        // 子弹方向和速度
        var angle = data.startAngle + deltaStartAngle;
        var dist = data.startDistance;
        var count = data.count;
        Vector3 startDir = Vector3.zero;
        switch (data.directionType)
        {
            case DirectionType.Fixed:
                startDir = data.startDir;
                break;
            case DirectionType.Aimed:
                startDir = player.transform.position - transform.position;
                break;
            case DirectionType.Random:
                startDir = Utils.GetRandomDirection();
                break;

        }
        startDir = startDir.normalized;
        // var startDir = data.aimed ? player.transform.position - transform.position : data.startDir;

        // 发射点位置
        var posDir = data.posDir.normalized;
        var posDistance = data.posStartDistance;

        for (int i = 0; i < group.num; i++)
        {

            while (timer < interval)
            {
                timer += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            var deltaPos = posDir * posDistance;

            shotList.AddRange(barrage.FireSpray(startDir, angle, data.spray.fire, data.spray.angle, dist, deltaPos));
            timer = 0;

            // 组内改变的参数
            angle += group.deltaAngle;
            dist += group.deltaDistance;
            posDir = Quaternion.AngleAxis(group.posDeltaAngle, Vector3.forward) * posDir;
            posDistance += group.posDeltaDistance;

        }
        barrage.firing = false;


        // if (data.shotOperationDelay > 0 && data.shotOperation != null)
        // {
        //     yield return new WaitForSeconds(data.shotOperationDelay);
        //     data.shotOperation(shotList);
        // }
        if(data.delayOperations.Count > 0){
            StartDelayOperation(data.delayOperations, shotList);
        }

        yield return null;
    }



    #endregion

}
