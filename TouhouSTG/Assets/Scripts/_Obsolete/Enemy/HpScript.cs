using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpScript : MonoBehaviour {
    #region 1 - 变量

    /// <summary>
    /// 总生命值
    /// </summary>
    public float hp = 1;

    /// <summary>
    /// 敌人标识
    /// </summary>
    public bool isEnemy = true;

    #endregion

    /// <summary>
    /// 对敌人造成伤害并检查对象是否应该被销毁
    /// </summary>
    /// <param name="damageCount"></param>
    public void Damage(float damageCount)
    {
        
        if(isEnemy)
        {
            if(!GetComponent <EnemyScript >().IsInvincible ())
                hp -= damageCount;

            SoundEffectHelper.Instance.MakePlayerShotSound();
        }

        if (hp <= 0)
        {
            // 死亡! 销毁对象!
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Shot shot = otherCollider.gameObject.GetComponent<Shot>();
        if (shot != null)
        {
            // 判断子弹归属,避免误伤
            if (shot.enemyShot != isEnemy)
            {
                Damage(shot.damage);
                SpecialEffectsHelper.Instance.Hit(shot.transform.position);

                // 销毁子弹
                // 记住，总是针对游戏的对象，否则你只是删除脚本
                Destroy(shot.gameObject);
            }
        }
    }

    public float GetHp()
    {
        return hp;
    }

    public void SetHp(float f)
    {
        hp = f;
    }
}
