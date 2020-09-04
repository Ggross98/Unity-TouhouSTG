using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyYiji : EnemyScript {
    void Start()
    {
        maxHp = 1800;
        hp.SetHp(maxHp);
        ShowSpellcardName(true,"速攻【断幺九nomi】");
    }
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
            if (hp.GetHp() <= 1650 && attack.GetFlag() == 1)
            {
                ShowSpellcardName(true, "杠精【大明杠】");
                attack.SetFlag(2);
                attack.Rest(2f);
                SetInvincibleTime(2f);
            }
            if (hp.GetHp() <= 1500 && attack.GetFlag() == 2)
            {
                ShowSpellcardName(true, "压迫【三副露no听】");
                //attack.Rest(2.5f);
                attack.SetFlag(3);
                attack.Rest(2f);
                SetInvincibleTime(2f);
                SpecialEffectsHelper.Instance.ClearEnemyBullet();

            }
            if (hp.GetHp() <= 1350 && attack.GetFlag() <= 3)
            {
                ShowSpellcardName(true, "一色【染手陷阱】");
                //attack.Rest(1f);
                attack.Rest(2f);
                SetInvincibleTime(2f);
                attack.SetFlag(4);
                
            }
            if (hp.GetHp() <= 1200 && attack.GetFlag() <= 4)
            {
                SpecialEffectsHelper.Instance.ClearEnemyBullet();

                ShowSpellcardName(true, "反牌效【得而复失的进张】");
                //attack.Rest(2.5f);
                attack.Rest(2f);
                SetInvincibleTime(2f);
                attack.SetFlag(5);
            }
            if (hp.GetHp() <= 1050 && attack.GetFlag() <= 5)
            {
                ShowSpellcardName(true, "初心【两家对日】");
                //attack.Rest(2f);
                attack.Rest(2f);
                SetInvincibleTime(5f);
                attack.SetFlag(6);
            }
            if (hp.GetHp() <= 900 && attack.GetFlag() <= 6)
            {
                SoundEffectHelper.Instance.MakeMajSound("rich");
                ShowSpellcardName(true, "对日【追立直】");
                //attack.Rest(2f);
                SpecialEffectsHelper.Instance.ClearEnemyBullet();

                attack.Rest(2f);
                SetInvincibleTime(2f);
                attack.SetFlag(7);
            }
            if (hp.GetHp() <= 750 && attack.GetFlag() <= 7)
            {
                ShowSpellcardName(true, "弃和【dora-观赏用】");
                //attack.Rest(2f);
                attack.Rest(2f);
                SetInvincibleTime(2f);
                attack.SetFlag(8);
            }
            if (hp.GetHp() <= 600 && attack.GetFlag() <= 8)
            {
                ShowSpellcardName(true, "【科学麻将vs魔法麻将】");
                //attack.Rest(2f);
                attack.Rest(2f);
                SpecialEffectsHelper.Instance.ClearEnemyBullet();

                SetInvincibleTime(2f);
                attack.SetFlag(9);
                SpecialEffectsHelper.Instance.ClearEnemyBullet();

            }
            if (hp.GetHp() <= 450 && attack.GetFlag() <= 9)
            {
                ShowSpellcardName(true, "流满【牌河奇迹】");
                //attack.Rest(3f);
                attack.Rest(2f);
                SetInvincibleTime(2f);
                attack.SetFlag(10);
            }
            if (hp.GetHp() <= 300 && attack.GetFlag() <= 10)
            {
                SpecialEffectsHelper.Instance.ClearEnemyBullet();
                attack.StopAllCoroutines();

                ShowSpellcardName(true, "纯符【纯粹的烧鸡地狱】");
                //attack.Rest(3f);
                attack.Rest(2f);
                SetInvincibleTime(2f);
                SpecialEffectsHelper.Instance.ClearEnemyBullet();
                attack.SetFlag(11);
            }
            if (hp.GetHp() <= 150 && attack.GetFlag() <= 11)
            {
                SoundEffectHelper.Instance.MakeMajSound("noting");
                SpecialEffectsHelper.Instance.ClearEnemyBullet();

                ShowSpellcardName(true, "【一姬的四位乱舞】");
                //attack.Rest(3f);
                //attack.Rest(2f);
                SetInvincibleTime(2f);
                attack.SetFlag(12);
            }
        }
    }
}
