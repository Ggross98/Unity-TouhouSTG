using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowardsScript : MonoBehaviour {

    private Vector3 direction;
    private Shot shot;
    private float angle;

	// Use this for initialization
	void Start () {
        shot = transform.GetComponent<Shot>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        direction = shot.direction;
        if (direction.x> 0)
        {
            angle = -Vector3.Angle(Vector3.up, direction.normalized);
        }
        else
        {
            angle = Vector3.Angle(Vector3.up, direction.normalized);
        }

        transform.eulerAngles = new Vector3(0, 0, angle);
        //transform.Rotate(0, 0, angle);
        //transform.rotation = new Vector3(0, 0, angle);
    }
}
