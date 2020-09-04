using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public GameObject _start, _help, _options, _exit;

    private MyButtonScript start, help, options, exit;


    // Use this for initialization
    void Start()
    {
        this.SetResolution();

        /*
        #region 按钮大小和位置

        float fontSize = 30;

        _start.GetComponent<RectTransform>().localPosition = new Vector2(Screen.width /5, Screen.height / 6*2);
        _start.GetComponent<RectTransform>().sizeDelta = new Vector2(fontSize * 5, fontSize * 1.5f);
        _help.GetComponent<RectTransform>().localPosition = new Vector2(Screen.width / 5, Screen.height / 6);
        _help.GetComponent<RectTransform>().sizeDelta = new Vector2(fontSize * 4, fontSize * 1.5f);
        _options.GetComponent<RectTransform>().localPosition = new Vector2(Screen.width / 5, 0);
        _options.GetComponent<RectTransform>().sizeDelta = new Vector2(fontSize * 7, fontSize * 1.5f);
        _exit.GetComponent<RectTransform>().localPosition = new Vector2(Screen.width / 5, -Screen.height / 6);
        _exit.GetComponent<RectTransform>().sizeDelta = new Vector2(fontSize * 4, fontSize * 1.5f);

        #endregion

        #region 初始化按钮
        start = _start.GetComponent<MyButtonScript>();
        help = _help.GetComponent<MyButtonScript>();
        options = _options.GetComponent<MyButtonScript>();
        exit = _exit.GetComponent<MyButtonScript>();

        start.SetSelected(true);
        help.SetSelected(false);
        options.SetSelected(false);
        exit.SetSelected(false);
        #endregion*/

    }

    // Update is called once per frame
    /*
    void Update()
    {
        if(Input.GetKeyUp (KeyCode .DownArrow))
        {
            SelectDown();
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            SelectUp();
        }
        else if (Input.GetKeyUp(KeyCode.X)||Input.GetKeyUp (KeyCode.Escape))
        {
            Select(3);
        }
        else if(Input.GetKeyUp (KeyCode.Z))
        {
            Execute();
        }

    }
    */
    private void Execute()
    {
        if(start.IsSelected())
        {
            SceneManager.LoadScene("Scene01");
        }
        else if(exit.IsSelected())
        {
            Application.Quit();
        }
    }

    private void Select(int index)
    {
        MyButtonScript[] buttons = { start, help, options, exit };
        for(int i = 0; i < buttons.Length; i++)
        {
            if (i == index)
            {
                buttons[i].SetSelected(true);
            }
            else
            {
                buttons[i].SetSelected(false);
            }
        }
    }

    private void SelectDown()
    {
        MyButtonScript[] buttons = { start, help, options, exit };
        int index;
        for(index=0; index < buttons.Length; index++)
        {
            if (buttons[index].IsSelected()) break;
        }

        if(index==buttons.Length - 1)
        {
            Select(0);
        }
        else
        {
            Select(index + 1);
        }
    }

    private void SelectUp()
    {
        MyButtonScript[] buttons = { start, help, options, exit };
        int index;
        for (index = 0; index < buttons.Length; index++)
        {
            if (buttons[index].IsSelected()) break;
        }

        if (index == 0)
        {
            Select(buttons.Length );
        }
        else
        {
            Select(index - 1);
        }
    }

    void SetResolution()
    {
        float heightScal = 9.0f;
        float widthScal = 16.0f;
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;
        int width = Screen.width;
        int height = Screen.height;
        if (((widthScal * height) / heightScal) > screenWidth)
        {
            int h = (int)((heightScal * screenWidth) / widthScal);
            int w = (int)((widthScal * h) / heightScal);
            Screen.SetResolution(w, h, true);
        }
        else
        {
            int w = (int)((widthScal * screenHeight) / heightScal);
            int h = (int)((heightScal * screenWidth) / widthScal);
            Screen.SetResolution(w, h, true);
        }
    }
    
    void OnGUI()
    {
        int boxWidth = Screen.width / 2;
        int boxHeight = (int)(Screen.height * 0.8f);
        int buttonWidth = (int)(boxWidth * 0.8f);
        int buttonHeight = (int)(boxHeight / 6.5f);

        //GUI.Box(new Rect(Screen.width / 2, Screen.height * 0.1f,boxWidth ,boxHeight ),"");

        // 在开始游戏界面绘制一个按钮
        if (
          // Center in X, 2/3 of the height in Y
          GUI.Button(new Rect(Screen.width / 2,Screen.height /13f,buttonWidth ,buttonHeight ), "开始游戏")
        )
        {
            SceneManager.LoadScene("StageSelect");
        }

        if (
          // Center in X, 2/3 of the height in Y
          GUI.Button(new Rect(Screen.width / 2, Screen.height / 6.5f*2, buttonWidth, buttonHeight), "操作帮助")
        )
        {
            SceneManager.LoadScene("Help");
        }
        if (
          // Center in X, 2/3 of the height in Y
          GUI.Button(new Rect(Screen.width / 2, Screen.height / 6.5f * 3.5f, buttonWidth, buttonHeight), "游戏设置")
        )
        {
            SceneManager.LoadScene("Options");
        }
        if (
         // Center in X, 2/3 of the height in Y
         GUI.Button(new Rect(Screen.width / 2, Screen.height / 6.5f * 5, buttonWidth, buttonHeight), "退出游戏")
       )
        {
            Application.Quit();
        }

    }

}
