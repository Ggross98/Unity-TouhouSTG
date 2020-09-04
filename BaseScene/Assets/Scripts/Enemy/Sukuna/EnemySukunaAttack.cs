using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySukunaAttack : EnemyAttack  {

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
                        barrage.FireAround(Time.time*Time.time,transform .position ,knife_blue ,36,12);
                        cooldown1 = 0.3f;
                    }
                    if(cooldown2 <= 0)
                    {
                        barrage.FireSector(transform .position +new Vector3(0,1,0),player.transform.position  ,knife_white ,3,50,15);
                        barrage.FireSector(transform.position + new Vector3(0, -1, 0), player.transform.position, knife_white, 3, 50, 15);

                        cooldown2 = 0.1f;
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
