using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectScript : MonoBehaviour {

    Image image;


	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI()
    {
        int boxWidth = Screen.width / 2;
        int boxHeight = (int)(Screen.height * 0.8f);
        int buttonWidth = (int)(boxWidth * 0.8f)/2;
        int buttonHeight = (int)(boxHeight / 6.5f);

        //GUI.Box(new Rect(Screen.width / 2, Screen.height * 0.1f,boxWidth ,boxHeight ),"");

        // 在开始游戏界面绘制一个按钮
        if (
          // Center in X, 2/3 of the height in Y
          GUI.Button(new Rect(Screen.width / 10, Screen.height / 13f, buttonWidth, buttonHeight), "古明地 恋")
        )
        {
            SceneManager.LoadScene("Stage_Koishi");
        }

        if (
          // Center in X, 2/3 of the height in Y
          GUI.Button(new Rect(Screen.width / 10, Screen.height / 6.5f * 2, buttonWidth, buttonHeight), "秦 心")
        )
        {
            //SceneManager.LoadScene("Help");
        }
        if (
          // Center in X, 2/3 of the height in Y
          GUI.Button(new Rect(Screen.width / 10, Screen.height / 6.5f * 3.5f, buttonWidth, buttonHeight), "藤原 妹红")
        )
        {
            //SceneManager.LoadScene("Options");
        }
        if (
         // Center in X, 2/3 of the height in Y
         GUI.Button(new Rect(Screen.width / 10, Screen.height / 6.5f * 5, buttonWidth, buttonHeight), "博丽 灵梦")
       )
        {
            //SceneManager.LoadScene("Menu");
        }

        // 在开始游戏界面绘制一个按钮
        if (
          // Center in X, 2/3 of the height in Y
          GUI.Button(new Rect(Screen.width / 10 + buttonWidth, Screen.height / 13f, buttonWidth, buttonHeight), "少名 针妙丸")
        )
        {
            SceneManager.LoadScene("Stage_Sukuna");
        }

        if (
          // Center in X, 2/3 of the height in Y
          GUI.Button(new Rect(Screen.width / 10+buttonWidth , Screen.height / 6.5f * 2, buttonWidth, buttonHeight), "一姬")
        )
        {
            SceneManager.LoadScene("Stage_Yiji");
        }
        if (
          // Center in X, 2/3 of the height in Y
          GUI.Button(new Rect(Screen.width / 10 + buttonWidth, Screen.height / 6.5f * 3.5f, buttonWidth, buttonHeight), "RB")
        )
        {
            SceneManager.LoadScene("Stage_RB");
        }
        if (
         // Center in X, 2/3 of the height in Y
         GUI.Button(new Rect(Screen.width / 10 + buttonWidth, Screen.height / 6.5f * 5, buttonWidth, buttonHeight), "返回")
       )
        {
            SceneManager.LoadScene("Menu");
        }

    }

}


