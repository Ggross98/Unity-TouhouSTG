using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        var shot = other.GetComponent<Shot>();
        if(shot!=null){
            shot.TouchWall();
        }
    }
}
