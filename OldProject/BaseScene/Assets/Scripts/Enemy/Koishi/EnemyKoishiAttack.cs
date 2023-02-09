using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKoishiAttack : EnemyAttack  {
    Transform[] stars;
    void Start()
    {
        stars = new Transform[] { star_yellow, star_red, star_purple };
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

    #region 重写弹幕

    public override  void Attack()
    {
        if (barrage != null)
        {
            switch (flag)
            {
                case 1:
                    /*
                    if (cooldown1 <= 0)
                    {
                        StartCoroutine(barrage.FireSector(transform.position, new Vector3(0, transform.position.y, 0), star_big_green, 15, 80f, 5f));
                        cooldown1 = 0.8f;
                    }
                    if (cooldown2 <= 0)
                    {
                        StartCoroutine(barrage.FireSector(transform.position, new Vector3(0, transform.position.y, 0), star_big_green, 16, 86f, 5f));
                        cooldown2 = 0.8f;
                    }
                    if(cooldown3 <= 0)
                    {
                        shootCount++;
                        StartCoroutine(barrage.FireRandomSniper (transform .position ,(player.position -transform .position ).normalized ,0,5,12,stars[shootCount %3] ,29));
                        cooldown3 = 2f;
                    }*/
                    
                    if (cooldown1 <= 0)
                    {
                        StartCoroutine(barrage.FireTurbine(0, 30, transform.position, 1f, 1f, 8, oval_m0_green, 12, 8f, 0.15f));
                        cooldown1 = 1.5f;
                    }
                    break;
                case 2:
                    if (cooldown1 <= 0)
                    {
                        barrage.FireAround(transform.position, circle_m2_red, 24, 8f);
                        cooldown1 = 0.3f;
                    }
                    if (cooldown2 <= 0)
                    {
                        barrage.FireAroundDelaySniper(transform.position, 0.15f, 0f, 5, 12, 24, oval_big_blue);
                        cooldown2 = 3f;
                    }
                    break;
                case 3:
                    if (cooldown1 <= 0)
                    {
                        StartCoroutine(barrage.FireCircleGroup(transform.position, 24, 0.3f, 0.15f, 6, circle_m1_red, Random.Range(0, 360), 6, 4, 0.08f));
                        cooldown1 = 0.5f;
                    }
                    break;
                case 4:
                    if (cooldown1 <= 0)
                    {
                        StartCoroutine(barrage.FireAroundGroup(transform.position, Random.Range(0, 360), 8, oval_m0_green, 10, 4, 0.7f, 0.1f, 6f));
                        cooldown1 = 3f;
                    }
                    break;
                case 5:
                    if (cooldown1 <= 0)
                    {
                        StartCoroutine(barrage.FireCircleGroup(transform.position, 16, 0.3f, -0.1f, 5, circle_m2_blue, Random.Range(0, 360), 10, 4, 0.08f));
                        cooldown1 = 0.5f;
                    }
                    break;
                case 6:
                    if (cooldown2 <= 0)
                    {
                        barrage.FireAroundBounce(transform.position, Random.Range(0, 360), heart_red, 12, 5, 2);
                        cooldown2 = 3f;
                    }
                    if (cooldown3 <= 0)
                    {
                        barrage.FireAroundBounce(transform.position, Random.Range(0, 360), heart_orange, 12, 5, 2);
                        cooldown3 = 3f;
                    }
                    break;
                case 7:
                    if (cooldown1 <= 0)
                    {
                        StartCoroutine(barrage.FireTurbine(0, 30, transform.position, 1f, 1f, 8, oval_m0_green, 12, 8f, 0.15f));
                        cooldown1 = 3f;
                    }
                    if (cooldown2 <= 0)
                    {
                        StartCoroutine(barrage.FireTurbine(0, 30, transform.position, 1f, 1f, 8, oval_m0_blue, 12, 8f, 0.15f));
                        cooldown2 = 3f;
                    }
                    break;
                case 8:
                    if (cooldown1 <= 0)
                    {
                        Vector3 sniper = (player.position - transform.position).normalized;

                        StartCoroutine(barrage.FireBounceFirework(transform.position, sniper, heart_orange, star_yellow, 3, 1, 30, 0.8f, 0.05f));
                        cooldown1 = 2f;
                    }
                    if (cooldown2 <= 0)
                    {
                        Vector3 random = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector3.up;

                        StartCoroutine(barrage.FireBounceFirework(transform.position, random, heart_red, star_red, 3, 1, 40, 0.5f, 0.05f));
                        cooldown2 = 0.8f;
                    }
                    break;
                case 9:
                    if (cooldown1 <= 0)
                    {
                        Quaternion offset = Quaternion.AngleAxis(Random.Range(-10, 10), Vector3.forward);
                        Vector3 fireDir = offset * Vector3.up;
                        Vector3 moveDir = Quaternion.AngleAxis(90, Vector3.forward) * fireDir;


                        barrage.FireLine(transform.position, fireDir, moveDir, 1f, 8, oval_big_blue, 5);

                        barrage.FireLine(transform.position, -fireDir, moveDir, 1f, 8, oval_big_blue, 5);
                        cooldown1 = 0.6f;
                    }
                    break;
                case 10:
                    if (cooldown1 <= 0)
                    {
                        Quaternion offset = Quaternion.AngleAxis(Random.Range(-20, 20), Vector3.forward);
                        Vector3 fireDir = offset * Vector3.up;
                        Vector3 moveDir = Quaternion.AngleAxis(90, Vector3.forward) * fireDir;


                        barrage.FireLine(transform.position, fireDir, moveDir, 1f, 8, oval_big_blue, 5);

                        barrage.FireLine(transform.position, -fireDir, moveDir, 1f, 8, oval_big_blue, 5);
                        cooldown1 = 1f;
                    }

                    if (cooldown2 <= 0)
                    {
                        barrage.FireAroundDelaySniper(transform.position, 0.3f, 0f, 3, 8, 24, oval_big_red);
                        cooldown2 = 3f;
                    }
                    break;
                case 11:
                    if (cooldown1 <= 0)
                    {
                        StartCoroutine(barrage.FireCircleGroup(transform.position, 16, 0.3f, 0f, 5, oval_big_blue, Random.Range(0, 360), 10, 4, 0f));
                        cooldown1 = 0.5f;
                    }
                    break;
                case 12:
                    if (cooldown1 <= 0)
                    {
                        StartCoroutine(barrage.FireSector(transform.position, new Vector3(0, transform.position.y, 0), star_big_green, 15, 80f, 5f));
                        cooldown1 = 0.8f;
                    }
                    if (cooldown2 <= 0)
                    {
                        StartCoroutine(barrage.FireSector(transform.position, new Vector3(0, transform.position.y, 0), star_big_green, 16, 86f, 5f));
                        cooldown2 = 0.8f;
                    }
                    break;
                case 13:
                    if (cooldown1 <= 0)
                    {
                        StartCoroutine(barrage.FireSector(transform.position, new Vector3(0, transform.position.y, 0), star_big_green, 15, 80f, 5f));
                        cooldown1 = 0.8f;
                    }
                    if (cooldown2 <= 0)
                    {
                        StartCoroutine(barrage.FireSector(transform.position, new Vector3(0, transform.position.y, 0), star_big_green, 16, 86f, 5f));
                        cooldown2 = 0.8f;
                    }
                    if (cooldown3 <= 0)
                    {
                        shootCount++;
                        StartCoroutine(barrage.FireRandomSniper(transform.position, (player.position - transform.position).normalized, 0, 5, 12, stars[shootCount % 3], 29));
                        cooldown3 = 2f;
                    }
                    break;
                case 14:
                    if (cooldown1 <= 0)
                    {
                        //Vector3 _rightPoint = new Vector3(Screen.width ,0,transform.position .z);
                        Vector3 rightPoint = transform.position + new Vector3(5 + Random.Range(-1, 1), 5, 0);
                        Vector3 dir = Quaternion.AngleAxis(0f, Vector3.forward) * Vector3.left;
                        StartCoroutine(barrage.FireLineGroup(rightPoint, Vector3.right, Vector3.left, Vector3.down, 0.2f, 2f, 3, 20, circle_m2_blue, 3, 0.2f));
                        //barrage.FireLine(rightPoint  ,Vector3.up,dir,1.5f,24,circle_m2_red ,3);
                        cooldown1 = 1f;
                    }
                    if (cooldown2 <= 0)
                    {
                        barrage.FireAroundDelaySniper(transform.position, 0.1f, 0f, 3f, 5f, 4, circle_m2_green);
                        cooldown2 = 2f;
                    }
                    break;
                case 15:
                    if (cooldown1 <= 0)
                    {
                        Vector3 rightPoint = transform.position + new Vector3(5, -5, 0);
                        Vector3 dir = Quaternion.AngleAxis(0f, Vector3.forward) * Vector3.left;

                        barrage.FireLine(rightPoint, Vector3.up, dir, 1.5f, 24, circle_m2_red, 3);
                        cooldown1 = 1.2f;
                    }
                    if (cooldown2 <= 0)
                    {
                        Vector3 upPoint = transform.position + new Vector3(-15, 5, 0);
                        Vector3 dir = Quaternion.AngleAxis(0f, Vector3.forward) * Vector3.down;
                        barrage.FireLine(upPoint, Vector3.right, dir, 1.5f, 20, circle_m2_red, 3);
                        cooldown2 = 1.2f;
                    }
                    if (cooldown3 <= 0)
                    {
                        barrage.FireSector(transform.position, player.position, circle_m2_green, 3, 30, 4f);
                        cooldown3 = 3f;
                    }
                    break;
                case 16:
                    if (cooldown1 <= 0)
                    {

                        if (cooldown1 <= 0)
                        {

                            StartCoroutine(barrage.FireCircum(0f, transform.position, heart_red, 0f, 1f, 1f, 2.5f, 3.3f, 1f, 16, 16));
                            StartCoroutine(barrage.FireCircum(0f, transform.position, heart_orange, 0f, 1f, 1f, -2.5f, 3.3f, 1f, 16, 16));

                            cooldown1 = 99f;
                        }
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
