using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    //public Transform bullet1, bullet2, bullet3,bullet4,bullet5,bullet6;

    public Transform oval_m0_green, oval_m0_blue;
    public Transform oval_big_blue, oval_big_red, oval_big_green;
    public Transform circle_m0_red, circle_m0_blue, circle_m0_green;
    public Transform circle_m1_red, circle_m1_blue, circle_m1_green;
    public Transform circle_m2_red, circle_m2_blue, circle_m2_green;

    public Transform circle_big_red, circle_big_blue, circle_big_green;
    public Transform star_green, star_red, star_yellow, star_purple;
    public Transform star_big_green;
    public Transform knife_white, knife_blue;
    public Transform heart_red, heart_orange;

    public Transform amulet_red, amulet_purple, amulet_blue, amulet_green, amulet_azure, amulet_yellow, amulet_black, amulet_white;
    public Transform amulet_red_s, amulet_blue_s, amulet_red_x, amulet_blue_x;

    public Transform player;

    // private float rate1 = 0.5f, rate2 = 0.4f;

    protected float cooldown1=0f, cooldown2 = 0f, cooldown3, cooldown4, cooldown5;

    protected float rest = 0;

    protected int flag = 1;

    protected int shootCount = 0;

    protected EnemyBarrage barrage;

    protected Vector3 upleft, upright, downleft, downright;

    


    void Awake()
    {

        barrage = transform.GetComponent<EnemyBarrage>();
        // barrage.SetPlayer(player);

        downleft = Camera.main.ScreenToWorldPoint(new Vector3(0,0,0));
        downright = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width , 0, 0));
        upleft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height , 0));
        upright = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height , 0));

    }


    public void Rest(float time)
    {
        rest = time;
    }

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

    public virtual void Attack()
    {
       
    }

    public  virtual void SetFlag(int i)
    {
    }

    public int GetFlag()
    {
        return flag;
    }

}

