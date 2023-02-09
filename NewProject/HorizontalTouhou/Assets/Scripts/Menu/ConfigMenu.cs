using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfigMenu : MonoBehaviour {

	public void LoadScene(string name){
        SceneManager.LoadScene(name);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.X)){
            LoadScene("MainMenu");
        }
    }
}
