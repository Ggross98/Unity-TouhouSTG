using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 创建音效实例
/// </summary>
public class SoundEffectHelper : MonoBehaviour
{
    /// <summary>
    /// 静态实例
    /// </summary>
    public static SoundEffectHelper Instance;

    public AudioClip missSound;
    public AudioClip defeatSound;
    public AudioClip playerShotSound;
    public AudioClip enemyShotSound;
    public AudioClip damageSound;
    public AudioClip laserSound;

    public AudioClip chi, kan, pon, rich, ron, tumo, top, noting;

    void Awake()
    {
        // 注册静态实例
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of SoundEffectsHelper!");
        }
        Instance = this;
        
    }

    public void MakeMajSound(string str)
    {
        if (str == "chi")
        {
            MakeSound(chi);
            MakeSound(chi);
        }
        if (str == "pon")
        {
            MakeSound(pon);
            MakeSound(pon);
        }
        if (str == "kan")
        {
            MakeSound(kan);
            MakeSound(kan);
        }
        if (str == "rich")
        {
            MakeSound(rich);
            MakeSound(rich);
        }
        if (str == "ron")
        {
            MakeSound(ron);
            MakeSound(ron);
            MakeSound(ron);
        }
        if (str == "tumo")
        {
            MakeSound(tumo);
        }
        if (str == "top")
        {
            MakeSound(top);
                MakeSound(top);
            MakeSound(top);
        }
        if (str == "noting")
        {
            MakeSound(noting);
            MakeSound(noting);
        }
        
    }




    public void MakeMissSound()
    {
        MakeSound(missSound);
    }

    public void MakeDefeatSound()
    {
        MakeSound(defeatSound);
    }

    public void MakeEnemyDamageSound()
    {
        MakeSound(damageSound);
    }

    public void MakeLaserSound()
    {
        MakeSound(laserSound);
    }

    public void MakePlayerShotSound()
    {
        MakeSound(playerShotSound);
    }

    public void MakeEnemyShotSound()
    {
        MakeSound(enemyShotSound);
    }

    /// <summary>
    /// 播放给定的音效
    /// </summary>
    /// <param name="originalClip"></param>
    private void MakeSound(AudioClip originalClip)
    {
        // 做一个非空判断, 防止异常导致剩余操作被中止
        if (Instance.ToString() != "null")
        {
            // 因为它不是3D音频剪辑，位置并不重要。
            AudioSource.PlayClipAtPoint(originalClip, transform.position);
        }
    }
}

