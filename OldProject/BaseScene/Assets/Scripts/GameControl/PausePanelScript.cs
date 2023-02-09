using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanelScript : MonoBehaviour {

    private bool show = false;

    private GameObject _resume, _restart, _exit;

    private MyButtonScript resume, restart, exit;

    public GameObject controller;
    private GameControlScript gameControl;

	// Use this for initialization
	void Start () {

        #region panel位置
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        float panelWidth = Screen.width / 3;
        //float panelHeight = Screen.height / 6 * 4;
        rectTransform.offsetMin = new Vector2(panelWidth ,Screen.height /6);
        rectTransform.offsetMax = new Vector2(-panelWidth , -Screen.height / 6);
        #endregion

        #region 按钮位置
        _resume = transform.Find("resume").gameObject ;
        _restart = transform.Find("restart").gameObject;
        _exit = transform.Find("exit").gameObject;

        float fontSize = 22;

        _resume.GetComponent<RectTransform>().localPosition = new Vector2(0,Screen.height /6);
        _resume.GetComponent<RectTransform>().sizeDelta  = new Vector2(fontSize *6, fontSize*1.5f );

        _restart.GetComponent<RectTransform>().localPosition = new Vector2(0, Screen.height / 6-fontSize *3f);
        _restart.GetComponent<RectTransform>().sizeDelta = new Vector2(fontSize * 7, fontSize * 1.5f);

        _exit.GetComponent<RectTransform>().localPosition = new Vector2(0, Screen.height / 6 - fontSize * 6f);
        _exit.GetComponent<RectTransform>().sizeDelta = new Vector2(fontSize * 4, fontSize * 1.5f);
        #endregion

        resume = _resume.GetComponent<MyButtonScript>();
        restart = _restart.GetComponent<MyButtonScript>();
        exit = _exit.GetComponent<MyButtonScript>();

        gameControl = controller.GetComponent<GameControlScript>();

        resume.SetSelected(true);

        gameObject.SetActive(false);
        
    }

    void OnGUI()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //this.gameObject.SetActive(show);
        if (!show) return;

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            SelectDown();
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            SelectUp();
        }
        else if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Escape))
        {
            Select(2);
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            Execute();
        }

    }

    private void Execute()
    {
        if (resume.IsSelected())
        {
            gameControl.Resume();
        }
        else if (restart.IsSelected())
        {
            gameControl.Restart();
        }
        else if (exit.IsSelected())
        {
            gameControl.Exit();
        }
    }

    private void Select(int index)
    {
        MyButtonScript[] buttons = { resume, restart, exit };
        for (int i = 0; i < buttons.Length; i++)
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
        MyButtonScript[] buttons = { resume, restart, exit };
        int index;
        for (index = 0; index < buttons.Length; index++)
        {
            if (buttons[index].IsSelected()) break;
        }

        if (index == buttons.Length - 1)
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
        MyButtonScript[] buttons = { resume, restart, exit };
        int index;
        for (index = 0; index < buttons.Length; index++)
        {
            if (buttons[index].IsSelected()) break;
        }

        if (index == 0)
        {
            Select(buttons.Length-1);
        }
        else
        {
            Select(index - 1);
        }
    }

    public void SetShow(bool s)
    {
        show = s;
    }

    public bool IsShow()
    {
        return show;
    }
}
