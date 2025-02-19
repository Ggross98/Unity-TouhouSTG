using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeScript : MonoBehaviour {

    public PlayerScript player;

    private bool isProtected = false;

    private bool collision = false;

    private float protectTime = 0;
    
    /// <summary>
    /// 对敌人造成伤害并检查对象是否应该被销毁
    /// </summary>
    /// <param name="damageCount"></param>
    public void Damage(float damageCount)
    {
        player.Miss();

        if (player.GetLife()  < 0)
        {
            // 死亡! 销毁对象!
            Destroy(gameObject);
        }
    }



    public void Protect(float time)
    {
        protectTime = time;
    }

    public float GetProtectTime()
    {
        return protectTime;
    }
    

    void Update()
    {
        if (protectTime > 0)
            protectTime -= Time.deltaTime;
        isProtected = (protectTime >0);

        if (collision&&(!isProtected ))
        {
            Damage(1);
            Protect(1.5f);
            player.Blink(1.5f);
            collision = false;
        }
    }




    void OnTriggerEnter2D(Collider2D otherCollider)
    {
       
        if (isProtected) return;
        if (collision) return;

        Shot shot = otherCollider.gameObject.GetComponent<Shot>();
        if (shot != null)
        {
            // 判断子弹归属,避免误伤
            if (shot.enemyShot)
            {
                SpecialEffectsHelper.Instance.Hit(shot.transform .position );
                collision = true;
            }
        }
        /*
        EnemyKoishi enemy = otherCollider.gameObject.GetComponent<EnemyKoishi>();
        if(enemy != null)
        {
            Damage(1);
            Protect(1.5f);
            player.Blink(1.5f);
        }*/
    }

    void OnColliderEnter2D(Collider2D otherCollider)
    {
        if (isProtected) return;
        if (collision) return;

        EnemyKoishi enemy = otherCollider.gameObject.GetComponent<EnemyKoishi>();
        if (enemy != null)
        {
           
            collision = true;
            
        }
    }

}
