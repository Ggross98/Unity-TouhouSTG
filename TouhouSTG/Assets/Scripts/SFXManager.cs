using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ggross.Pattern;

public class SFXManager : SingletonMonoBehaviour<SFXManager>
{
    [SerializeField] private List<AudioClip> clips;
    [SerializeField] private SFXPool pool;

    private void Awake() {
        pool.Init();
    }

    public void CreateSFX(int index, float volume = 1f){

        if(index < 0 || index >= clips.Count) return;

        var sfx = pool.Get();
        sfx.SetClip(clips[index]);
        sfx.SetVolume(volume);
        sfx.Play();
    }
}
