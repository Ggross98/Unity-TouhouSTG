using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWinScript : MonoBehaviour {

    private bool show = false;

    private GameObject _restart, _exit,_text;

    private MyButtonScript restart, exit;
    

    public GameObject controller;
    private GameControlScript gameControl;


    // Use this for initialization
    void Awake () {


        gameObject.SetActive(false);

        #region panel位置
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        float panelWidth = Screen.width / 3;
        //float panelHeight = Screen.height / 6 * 4;
        rectTransform.offsetMin = new Vector2(panelWidth, Screen.height / 6);
        rectTransform.offsetMax = new Vector2(-panelWidth, -Screen.height / 6);
        #endregion

        #region 按钮位置
        _restart = transform.Find("restart").gameObject;
        _exit = transform.Find("exit").gameObject;

        float fontSize = 22;
        

        _restart.GetComponent<RectTransform>().localPosition = new Vector2(0, Screen.height / 6 - fontSize * 3f);
        _restart.GetComponent<RectTransform>().sizeDelta = new Vector2(fontSize * 7, fontSize * 1.5f);

        _exit.GetComponent<RectTransform>().localPosition = new Vector2(0, Screen.height / 6 - fontSize * 6f);
        _exit.GetComponent<RectTransform>().sizeDelta = new Vector2(fontSize * 4, fontSize * 1.5f);
        #endregion
        
        restart = _restart.GetComponent<MyButtonScript>();
        exit = _exit.GetComponent<MyButtonScript>();

        gameControl = controller.GetComponent<GameControlScript>();

        exit.SetSelected(true);

        #region 文本

        _text = transform.Find("text").gameObject;
        _text.GetComponent<RectTransform>().localPosition = new Vector2(0, Screen.height / 6);
        _text.GetComponent<RectTransform>().sizeDelta = new Vector2(fontSize * 7, fontSize * 2f);


        #endregion

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (show)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Change();
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Execute();
            }
        }
	}

    void Change()
    {
        if(restart.IsSelected())
        {
            restart.SetSelected(false);
            exit.SetSelected(true);
        }
        else
        {
            restart.SetSelected(true);
            exit.SetSelected(false);
        }
    }

    void Execute()
    {
        if(restart.IsSelected())
        {
            gameControl.Restart();
        }
        else
        {
            gameControl.Exit();
        }
    }

    public void SetShow(bool s)
    {
        show = s;
    }






    void OnGUI()
    {
        /*
        const int buttonWidth = 120;
        const int buttonHeight = 60;
        
        GUIStyle bb = new GUIStyle();
        bb.normal.background = null; 
        bb.normal.textColor = new Color(1, 0, 0);  
        bb.fontSize = 60; 
        GUI.Label(new Rect(Screen.width / 2 - 120, (1 * Screen.height / 3) - (buttonHeight / 2), buttonWidth, buttonHeight), "游戏胜利",bb);
        */

        /*
        // 在x轴中心, y轴1/3处创建"重来"按钮
        if (GUI.Button(new Rect(Screen.width / 2 - (buttonWidth / 2), (1.5f * Screen.height / 3) - (buttonHeight / 2), buttonWidth, buttonHeight), "再来一次"))
        {
            // 重新加载游戏场景
            SceneManager.LoadScene("Scene01");
        }*/

        // x轴中心, y轴2/3处创建"返回菜单"按钮
        /*
        if (GUI.Button(new Rect(Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 3) - (buttonHeight / 2), buttonWidth, buttonHeight), "返回菜单"))
        {
            // 加载菜单场景
            SceneManager.LoadScene("Menu");
        }
        */
    }
}
