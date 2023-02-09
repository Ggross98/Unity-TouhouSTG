using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour {
    int targetFrameRate = 60;
	// Use this for initialization
	void Start () {
        Application.targetFrameRate = targetFrameRate;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
