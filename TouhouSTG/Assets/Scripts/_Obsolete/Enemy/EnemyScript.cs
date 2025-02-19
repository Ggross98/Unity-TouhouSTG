using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : PauseObject  {
    #region 变量
    public Transform player;

    public PlayerUIScript ui;

    protected HpScript hp;

    protected EnemyAttack attack;

    protected float maxHp = 1700;

    protected Vector3 startPosition;

    protected float invincibleTime = 0;

    #endregion

    void Awake()
    {
        attack = GetComponent<EnemyAttack>();
        // 检索生命
        hp = GetComponent<HpScript>();
        hp.SetHp(maxHp);

        
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPause) return;

        if(invincibleTime > 0)
        {
            invincibleTime -= Time.deltaTime;
        }

        CheckSpell();
        
    }


    public bool IsInvincible()
    {
        return invincibleTime>0;
    }

    public void SetInvincibleTime(float time)
    {
        invincibleTime = time;
    }
    /// <summary>
    /// 根据生命值改变弹幕
    /// </summary>
    protected virtual  void CheckSpell()
    {
        
    }

    /// <summary>
    /// 在一定时间内移动到某处
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator MoveObject(Vector3 startPos, Vector3 endPos, float time)
    {
        var dur = 0.0f;
        while (dur <= time)
        {
            dur += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, dur / time);
            yield return null;
        }
    }



    public float GetHp()
    {
        return hp.GetHp();
    }

    public float GetMaxHp()
    {
        return maxHp;
    }

    /// <summary>
    /// 被击破时游戏结束
    /// </summary>
    void OnDestroy()
    {
        //transform.parent.gameObject.AddComponent<GameWinScript>();
        SoundEffectHelper.Instance.MakeDefeatSound();
        SpecialEffectsHelper.Instance.ClearEnemyBullet();
        if (player != null) SpecialEffectsHelper.Instance.DefeatEnemy(this.transform.position);
    }


    protected void ShowSpellcardName(bool b,string name)
    {
        ui.ShowSpellName(b,name);
    }
}
