using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRBAttack : EnemyAttack  {

    public Transform cytus_arrow;
    public Transform cytus_up, cytus_down, cytus_link;
    public Transform note0_red, note0_blue;
    public Transform note1_red, note1_blue;

    
    void Update()
    {
        if (player == null) return;
        //停火
        if (rest > 0) rest -= Time.deltaTime;
        if (rest > 0) return;

        //弹幕冷却
        if (cooldown1 > 0) cooldown1 -= Time.deltaTime;
        if (cooldown2 > 0) cooldown2 -= Time.deltaTime;
        if (cooldown3 > 0) cooldown3 -= Time.deltaTime;
        if (cooldown4 > 0) cooldown4 -= Time.deltaTime;
        if (cooldown5 > 0) cooldown5 -= Time.deltaTime;

        //弹幕设计
        Attack();


    }

    #region 重写弹幕

    public override void Attack()
    {
        if (barrage != null)
        {
            switch (flag)
            {
                case 1:
                    if (cooldown1 <= 0)
                    { 
                        
                        barrage.FireAround(Time.time * Time.time*Time.time , transform.position, cytus_up, 24,6);
                        //barrage.FireAround(-Time.time * Time.time * Time.time, transform.position, note1_blue, 16, 6);

                        //StartCoroutine(barrage.FireSectorBounceGroup (transform .position ,transform .position+new Vector3 (-2,-5,0),note1_blue ,6,40,8,12,3,0.05f));
                        //StartCoroutine(barrage.FireSectorBounceGroup(transform.position, new Vector3 (-2,-1,0), note1_red, 2,15, 8, 20,0, 0.1f));

                        //barrage.FireSectorBounce(transform.position, new Vector3(-1, -1, 0), cytus_down, 3, 15, 8);
                        //barrage.FireSectorBounce(transform.position, new Vector3(-1, 1, 0), cytus_up, 3, 15, 8);

                        cooldown1 = 0.1f;
                    }

                    if(cooldown2 <= 0)
                    {
                        //StartCoroutine(barrage.FireSectorBounceGroup(transform.position, new Vector3(-2 ,1, 0), note1_blue, 2,15, 8, 20, 0, 0.1f));

                        cooldown2 = 8f;
                    }

                    break;

            }
        }
    }
    #endregion

    public override void SetFlag(int i)
    {
        Debug.Log(i);
        //SpecialEffectsHelper.Instance.ClearEnemyBullet();
        //Rest(2f);


        flag = i;
        cooldown1 = 0;
        cooldown2 = 0;
        cooldown3 = 0;
        cooldown4 = 0;
        cooldown5 = 0;
        switch (i)
        {
            case 2:
                //cooldown2 = cooldown1 + 0.325f;
                break;
            case 3:
                //cooldown2 = cooldown1 + 0.35f;
                break;
            case 4:
                //cooldown2 = cooldown1;
                break;
            case 5:
                //cooldown2 = cooldown1 + 0.4f;
                break;
            case 6:
                cooldown2 = 0;
                cooldown3 = 1.5f;
                break;
            case 7:
                cooldown2 = cooldown1 + 1.5f;
                break;
            case 8:
                cooldown2 = cooldown1 + 2f;
                break;
            case 9:
                cooldown2 = cooldown1 + 0.2f;
                break;
            case 10:
                break;
            case 11:
                break;
            case 12:
                cooldown2 = cooldown1 + 0.4f;
                break;
            case 13:
                cooldown2 = 0.4f;
                break;
            case 14:
                break;
            case 15:
                cooldown2 = 0;
                break;
            case 16:
                break;
        }
        //SpecialEffectsHelper.Instance.ClearEnemyBullet();

    }
}
