using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffectsHelper : MonoBehaviour {
    /// <summary>
    /// Singleton
    /// </summary>
    public static SpecialEffectsHelper Instance;
    
    public ParticleSystem hitEffect,explosionEffect,enemyOverEffect;

    void Awake()
    {
        // Register the singleton
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of SpecialEffectsHelper!");
        }

        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        //SpecialEffectsHelper.Instance.Hit(transform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 在给定位置创建爆炸特效
    /// </summary>
    /// <param name="position"></param>
    public void Hit(Vector3 position)
    {
        // 火焰特效
        instantiate(hitEffect, position);
    }

    public void Explosion(Vector3 position)
    {
        // 爆炸特效
        instantiate(explosionEffect, position);
    }

    public void DefeatEnemy(Vector3 position)
    {
        // 敌机爆炸特效
        instantiate(enemyOverEffect, position);
    }

    //清除敌人弹幕
    public void ClearEnemyBullet()
    {
        GameObject[] m_Desk = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for(int i=0;i<m_Desk.Length; i++)
        {
            m_Desk[i].GetComponent<Shot>().TouchCharacter();
        }
    }

    //清除一定区域内的敌人弹幕
    /*
    public void ClearEnemyBullet(Bounds bounds)
    {
        GameObject[] m_Desk = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for (int i = 0; i < m_Desk.Length; i++)
        {
            if(bounds.Contains (m_Desk [i].transform .position))
            {
                Destroy(m_Desk[i]);
            }
        }
    }*/

    //清除自机弹幕
    public void ClearPlayerBullet()
    {
        GameObject[] m_Desk = GameObject.FindGameObjectsWithTag("PlayerBullet");
        for (int i = 0; i < m_Desk.Length; i++)
        {
            Destroy(m_Desk[i]);
        }
    }

    /// <summary>
    /// 从预制体中实例化粒子特效
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
    {
        ParticleSystem newParticleSystem = Instantiate(prefab, position, Quaternion.identity) as ParticleSystem;

        // 确保它会被销毁
        //Destroy(newParticleSystem.gameObject, newParticleSystem.startLifetime);

        return newParticleSystem;
    }
}
