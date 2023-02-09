using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKoishi : EnemyScript  {

    // Update is called once per frame
    void Update()
    {
        if (_isPause) return;

        if (invincibleTime > 0)
        {
            invincibleTime -= Time.deltaTime;
        }

        CheckSpell();

    }

    /// <summary>
    /// 根据生命值改变弹幕
    /// </summary>
    protected override void CheckSpell()
    {
        if (hp != null)
        {
            if (hp.GetHp() <= 1620 && attack.GetFlag() == 1)
            {
                attack.SetFlag(2);
                attack.Rest(2f);
                SetInvincibleTime(2f);
            }
            if (hp.GetHp() <= 1520 && attack.GetFlag() == 2)
            {
                
                //attack.Rest(2.5f);
                attack.SetFlag(3);
                attack.Rest(2f);
                SetInvincibleTime(2f);
            }
            if (hp.GetHp() <= 1440 && attack.GetFlag() <= 3)
            {

                //attack.Rest(1f);
                attack.Rest(2f);
                SetInvincibleTime(2f);
                attack.SetFlag(4);
            }
            if (hp.GetHp() <= 1340 && attack.GetFlag() <= 4)
            {

                //attack.Rest(2.5f);
                attack.Rest(2f);
                SetInvincibleTime(2f);
                attack.SetFlag(5);
            }
            if (hp.GetHp() <= 1240 && attack.GetFlag() <= 5)
            {
                //attack.Rest(2f);
                attack.Rest(2f);
                SetInvincibleTime(5f);
                attack.SetFlag(6);
            }
            if (hp.GetHp() <= 1160 && attack.GetFlag() <= 6)
            {
                //attack.Rest(2f);
                SpecialEffectsHelper.Instance.ClearEnemyBullet();

                attack.Rest(2f);
                SetInvincibleTime(2f);
                attack.SetFlag(7);
            }
            if (hp.GetHp() <= 1010 && attack.GetFlag() <= 7)
            {
                //attack.Rest(2f);
                attack.Rest(2f);
                SetInvincibleTime(12f);
                attack.SetFlag(8);
            }
            if (hp.GetHp() <= 910 && attack.GetFlag() <= 8)
            {
                //attack.Rest(2f);
                attack.Rest(2f);
                SpecialEffectsHelper.Instance.ClearEnemyBullet();

                SetInvincibleTime(2f);
                attack.SetFlag(9);
            }
            if (hp.GetHp() <= 810 && attack.GetFlag() <= 9)
            {
                //attack.Rest(3f);
                attack.Rest(2f);
               SetInvincibleTime(2f);
                attack.SetFlag(10);
            }
            if (hp.GetHp() <= 730 && attack.GetFlag() <= 10)
            {
                //attack.Rest(3f);
                attack.Rest(2f);
                SetInvincibleTime(2f);
                SpecialEffectsHelper.Instance.ClearEnemyBullet();
                attack.SetFlag(11);
            }
            if (hp.GetHp() <= 650 && attack.GetFlag() <= 11)
            {
                //attack.Rest(3f);
                //attack.Rest(2f);
                SetInvincibleTime(2f);
                attack.SetFlag(12);
            }
            if (hp.GetHp() <= 550 && attack.GetFlag() <= 12)
            {
                //attack.Rest(3f);
                
                //attack.Rest(2f);
                //SetInvincibleTime(2f);
                attack.SetFlag(13);
            }
            if (hp.GetHp() <= 450 && attack.GetFlag() <= 13)
            {
                SpecialEffectsHelper.Instance.ClearEnemyBullet();

                attack.Rest(3f);
                attack.Rest(2f);
                SetInvincibleTime(1f);
                attack.SetFlag(14);
            }
            if (hp.GetHp() <= 350 && attack.GetFlag() <= 14)
            {
                //attack.Rest(3f);
                attack.Rest(2f);
                SetInvincibleTime(2f);
                attack.SetFlag(15);
            }
            if (hp.GetHp() <= 200 && attack.GetFlag() <= 15)
            {
                //attack.Rest(3f);
                attack.Rest(2f);
                SetInvincibleTime(2f);
                attack.SetFlag(16);
            }


        }
    }
    
}
