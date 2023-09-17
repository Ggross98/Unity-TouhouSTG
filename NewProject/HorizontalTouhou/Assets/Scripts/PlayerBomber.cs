using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomber : MonoBehaviour
{
    [SerializeField] ShotEraser eraser;
    [SerializeField] LineRenderer line;
    [SerializeField] ParticleSystem particles, beam;
    [SerializeField] private float startAnimationDuration;

    private bool firing;

    private void Awake() {
        Show(false);
        eraser.playerBomb = true;
    }

    private void Update() {
        if(firing){

            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position + Vector3.right * 20);

        }
        // else{
        //     line.enabled = false;

        //     particles.Stop();
        //     beam.Stop();

        //     eraser.erasing = false;
        // }
    }

    public void SetFire(bool f){
        firing = f;

        if(f){
            StartCoroutine(DelayShow(startAnimationDuration));
        }else{
            Show(false);
        }
    }

    private void Show(bool show){
        if(show){
            particles.Play();
            beam.Play();
            line.enabled = true;

            eraser.erasing = true;
        }else{
            particles.Stop();
            beam.Stop();
            line.enabled = false;

            eraser.erasing = false;
        }
    }

    private IEnumerator DelayShow(float delay){

        yield return new WaitForSeconds(delay);

        particles.Play();
        beam.Play();
        eraser.erasing = true;

        yield return new WaitForSeconds(0.1f);
        line.enabled = true;

        yield return null;

    }
}
