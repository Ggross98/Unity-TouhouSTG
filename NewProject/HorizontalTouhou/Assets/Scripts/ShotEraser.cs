using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotEraser : MonoBehaviour
{
    public bool erasing = false;
    public bool playerBomb = false;

    // private void OnTriggerStay2D(Collider2D other) {
    //     if(erasing){
    //         var shot = other.GetComponent<Shot>();
    //         if(shot!=null && shot.enemyShot){
    //             shot.Kill();
    //         }
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other) {
        if(erasing){
            var shot = other.GetComponent<Shot>();
            if(shot!=null && shot.enemyShot){
                shot.Kill();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other){
        if(erasing && playerBomb){
            var enemy = other.GetComponent<EnemyController>();
            if(enemy){
                enemy.Hit();
            }
        }
    }

    public void EraseScreen(Vector3 pos){
        StartCoroutine(DoErase(pos, 0f, 30f, 1f));
    }

    private IEnumerator DoErase(Vector3 pos, float minScale, float maxScale, float duration){
        transform.position = pos;
        erasing = true;
        
        var timer = duration;
        while(timer > 0f){
            timer -= Time.deltaTime;
            transform.localScale = Vector3.one * Mathf.Lerp(minScale, maxScale, (duration - timer)/duration);
            yield return new WaitForEndOfFrame();
        }

        transform.localScale = Vector3.zero;
        erasing = false;
        yield return null;
    }

    
}
