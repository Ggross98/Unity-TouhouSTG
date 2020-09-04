using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI()
    {
        if (
            GUI.Button(new Rect(0, 0, Screen.width, Screen.height), "没做好！单击返回")
            )
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
