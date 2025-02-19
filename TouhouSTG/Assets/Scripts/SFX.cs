using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    private System.Action<SFX> overAction;

    private void Awake() {
        source.playOnAwake = false;
    }

    public void SetClip(AudioClip clip){
        source.clip = clip;
    }

    public void Play(){
        source.Play();
    }

    public void SetOverAction(System.Action<SFX> action){
        this.overAction = action;
    }

    public void SetVolume(float volume){
        source.volume = volume;
    }

    private void Update() {
        if(source != null && source.time > 0 && !source.isPlaying){
            if(overAction != null){
                overAction.Invoke(this);
            }
        }
    }

}
