using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyYijiAttack : EnemyAttack{



    Transform[] amulets;

    new void Awake()
    {
        cooldown2 = 0.4f;
        barrage = transform.GetComponent<EnemyBarrage>();
        barrage.SetPlayer(player);

        
        amulets = new Transform[]{ amulet_red, amulet_purple, amulet_blue, amulet_green, amulet_azure, amulet_yellow, amulet_black, amulet_white };
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

    public override void Attack()
    {
        if (barrage != null)
        {
            switch (flag)
            {
               
                case 1:
                    
                    
                    if (cooldown1 <= 0)
                    {
                        StartCoroutine(barrage.FireSector(Random.Range(60f, 120f), transform.position, amulet_azure, 18, 15, 5));
                        cooldown1 = 0.8f;
                    }
                    if (cooldown2 <= 0)
                    {
                        StartCoroutine(barrage.FireSector(transform.position, player.position, amulet_azure, 18, 15, 5));
                        cooldown2 = 0.8f;
                    }
                    if (cooldown3 <= 0)
                    {
                        StartCoroutine(barrage.FireSector(-90, transform.position, amulet_azure, 15, 220, 5));
                        cooldown3 = 0.5f;
                    }

                    break;
                case 2:
                    if (cooldown1 <= 0)
                    {
                        //Quaternion q = Quaternion.LookRotation(player.position -transform .position );
                        barrage.FireAround(Vector3.Angle(Vector3.up, player.position - transform.position), transform.position, amulet_red, 24, 8);
                        cooldown1 = 0.2f;
                    }
                    if (cooldown2 <= 0)
                    {
                        barrage.FireAround(transform.position, amulet_red, 28, 5);

                        cooldown2 = 0.5f;
                    }
                    if(cooldown3 <= 0)
                    {
                        SoundEffectHelper.Instance.MakeMajSound("kan");
                        cooldown3 = 2f;
                    }
                    break;
                case 3:
                    if (cooldown1 <= 0)
                    {
                        shootCount++;
                        if(shootCount % 2 == 0)
                        {
                            SoundEffectHelper.Instance.MakeMajSound("pon");
                        }
                        else
                        {
                            SoundEffectHelper.Instance.MakeMajSound("chi");
                        }
                        StartCoroutine(barrage.FireRandomField(transform.position + new Vector3(10, -5), 5, 10, Vector3.left, 6, 12, amulet_azure, 40));
                        cooldown1 = 2;
                    }
                    break;
                case 4:
                    if (cooldown1 <= 0)
                    {
                        shootCount++;
                        StartCoroutine(barrage.FireCircleGroup(transform.position, 36, 0, 0.12f, 4f, amulet_purple, shootCount * 10, 0, 1, 0f));
                        cooldown1 = 0.8f;
                    }
                    if (cooldown2 <= 0)
                    {
                        StartCoroutine(barrage.FireCircleGroup(transform.position, 36, 0, -0.12f, 4f, amulet_purple, shootCount * 10, 0, 1, 0f));
                        cooldown2 = 0.8f;
                    }
                    break;
                case 5:
                    if (cooldown1 <= 0)
                    {
                        shootCount++;
                        int count = shootCount % 60;
                        float offset;
                        if (count > 30)
                        {
                            offset = 180 - 6 * count;
                        }
                        else
                        {
                            offset = 6 * count;
                        }
                        barrage.FireAround(offset, transform.position, amulets[shootCount % 8], 6, 6);
                        //barrage.FireAround(-shootCount * 5, transform.position + new Vector3(0, -3), amulet_white, 10, 6);

                        cooldown1 = 0.05f;
                    }

                    if (cooldown2 <= 0)
                    {
                        StartCoroutine(barrage.FireLineSniper(transform.position, (player.position - transform.position).normalized, amulet_azure, 7, 5, 0.1f));
                        cooldown2 = 1.5f;
                    }
                    break;
                case 6:
                    //cooldown2 = cooldown1 + 2f;
                    if (cooldown1 <= 0)
                    {
                        StartCoroutine(barrage.FireRandomField(new Vector3(-11, 5, 0), 22, 5, Vector3.down, 4, 10, amulet_blue, 80));
                        cooldown1 = 4;
                    }
                    if (cooldown2 <= 0)
                    {
                        StartCoroutine(barrage.FireRandomField(new Vector3(-11, -10, 0), 22, 5, Vector3.up, 4, 10, amulet_red, 80));
                        cooldown2 = 4;
                    }
                    break;
                case 7:

                    if (cooldown1 <= 0)
                    {
                        shootCount++;
                        Vector3 dir0 = (player.position - transform.position).normalized;
                        Vector3 dir1 = Quaternion.AngleAxis(90,Vector3 .forward )*dir0;
                        Vector3 dir2 = Quaternion.AngleAxis(-90, Vector3.forward) * dir0;

                        StartCoroutine(barrage.FireRandomSniper(transform.position, dir0, 10, 2, 8, amulet_blue, 40));

                        //StartCoroutine(barrage.FireLineSniper(transform.position,dir0, amulet_blue, 8f, 10, 0.1f));

                        //StartCoroutine(barrage.FireLineSniper(transform.position, dir0, amulet_blue, 6f, 10, 0.1f));

                        StartCoroutine(barrage.FireLineSniper(transform.position, dir1, amulet_blue, 4f, 10, 0.2f));
                        StartCoroutine(barrage.FireLineSniper(transform.position, dir2, amulet_blue, 4f, 10, 0.2f));


                        cooldown1 = 1.2f;
                    }
                    break;
                case 8:
                    if (cooldown1 <= 0)
                    {
                        shootCount++;
                        int count = shootCount % 12;

                        if (count < 10)
                        {
                            Vector3 firePoint = new Vector3(transform.position.x, count - 5, 0);
                            barrage.FireAround(firePoint, amulet_red, 36, Random.Range(3f, 6f));
                            //barrage.FireAround(firePoint, amulet_red, 36, Random.Range(3f, 6f));
                        }
                        cooldown1 = 0.3f;
                    }
                    break;
                case 9:
                    if (cooldown1 <= 0)
                    {
                        shootCount++;
                        int count = shootCount % 20;
                        float offset;
                        if (count > 10)
                        {
                            offset = 60 - 3 * count;
                        }
                        else
                        {
                            offset = 3 * count;
                        }
                        barrage.FireAround(offset, transform.position + new Vector3(0, 3, 0), amulet_purple, 12, 8);
                        //barrage.FireAround(-shootCount * 5, transform.position + new Vector3(0, -3), amulet_white, 10, 6);

                        cooldown1 = 0.1f;
                    }
                    if (cooldown2 <= 0)
                    {
                        barrage.FireAround(Random.Range(0, 360), transform.position + new Vector3(0, -3, 0), amulet_blue, 24, 8);
                        cooldown2 = 0.4f;
                    }
                    break;
                case 10:
                    //cooldown all 相等
                    if (cooldown1 <= 0)
                    {
                        StartCoroutine(barrage.FireSin(transform.position + new Vector3(-25, 2, 0), Vector3.right, 2, 0.4f, amulet_azure, 12, 0.1f, 9f, false));
                        StartCoroutine(barrage.FireSin(transform.position + new Vector3(-25, -2, 0), Vector3.right, 2, 0.4f, amulet_azure, 12, 0.1f, 9f, false));

                        cooldown1 = 99f;
                    }
                    if (cooldown3 <= 0)
                    {
                        StartCoroutine(barrage.FireSin(transform.position + new Vector3(15, -2, 0), Vector3.left, 1, 0.3f, amulet_blue, 10, 0.1f, 9f, true));
                        StartCoroutine(barrage.FireSin(transform.position + new Vector3(15, 2, 0), Vector3.left, 1, 0.3f, amulet_blue, 10, 0.1f, 9f, true));

                        cooldown3 = 99f;
                    }
                    if (cooldown4 <= 0)
                    {
                        StartCoroutine(barrage.FireSin(transform.position + new Vector3(15, -4, 0), Vector3.left, 0.5f, 0.2f, amulet_purple, 8, 0.1f, 9f, true));
                        StartCoroutine(barrage.FireSin(transform.position + new Vector3(15, 4, 0), Vector3.left, 0.5f, 0.2f, amulet_purple, 8, 0.1f, 9f, true));
                        cooldown4 = 99f;
                    }
                    break;
                case 11:
                    amulets = new Transform[] { amulet_white, amulet_green, amulet_red };
                    //中间区域
                    if (cooldown1 <= 0)
                    {
                        shootCount++;

                        Vector3 firePoint = new Vector3(transform.position.x + Random.Range(-2, 2), transform.position.y + Random.Range(-1, 1), 0);
                        Transform bullet = amulets[shootCount % 3];
                        barrage.FireAround(Random.Range(0, 360), firePoint, bullet, 24, 6);
                        cooldown1 = 0.3f;
                    }

                    //两边
                    if (cooldown2 <= 0)
                    {
                        Vector3 point1 = new Vector3(transform.position.x, transform.position.y - 4, 0);
                        Vector3 point2 = new Vector3(transform.position.x, transform.position.y + 4, 0);
                        barrage.FireAround(Random.Range(0, 360), point1, amulet_azure, 24, 3);
                        barrage.FireAround(Random.Range(0, 360), point2, amulet_azure, 24, 3);
                        cooldown2 = 0.8f;
                    }

                    //扇形蓝蛋
                    if (cooldown3 <= 0)
                    {
                        barrage.FireSector(transform.position, Vector3.left, amulet_blue, 8, 60, 4);
                        cooldown3 = 0.4f;
                    }
                    break;
                case 12:
                    if (cooldown1 <= 0)
                    {
                        StartCoroutine(barrage.FireRandomField(transform.position + new Vector3(10, -5), 5, 10, Vector3.left, 4, 8, amulet_blue_s, 25));
                        cooldown1 = 2f;
                    }
                    if (cooldown2 <= 0)
                    {
                        barrage.FireAround(Vector3.Angle(Vector3.up, player.position - transform.position), transform.position, amulet_azure, 12, 6);
                        cooldown2 = 0.2f;
                    }
                    if (cooldown3 <= 0)
                    {
                        barrage.FireAroundDelaySniper(transform.position, 0.2f, 0f, 4, 9, 6, amulet_red_x);
                        cooldown3 = 1f;
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
                cooldown2 = cooldown1+0.4f;
                break;
            case 5:
                //cooldown2 = cooldown1 + 0.4f;
                break;
            case 6:
                cooldown2 = 2f;
                //cooldown3 = 1.5f;
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
