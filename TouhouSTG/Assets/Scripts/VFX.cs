using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    private ParticleSystem.MainModule module;

    [HideInInspector] public VFXPool pool;

    private void Awake() {
        module = ps.main;
        module.stopAction = ParticleSystemStopAction.Callback;
        module.playOnAwake = false;
    }

    private void OnParticleSystemStopped() {
        if(pool == null){
            Destroy(gameObject);
        }
        else{
            pool.Release(this);
        }
    }

    public void SetPosition(Vector3  pos){
        transform.position = pos;
    }

    public void Play(){
        ps.Play();
    }
}
