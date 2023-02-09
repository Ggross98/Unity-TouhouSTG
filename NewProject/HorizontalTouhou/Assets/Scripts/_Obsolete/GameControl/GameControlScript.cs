using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControlScript : MonoBehaviour {

    public PlayerScript player;
    public EnemyScript enemy;
    public GameObject pausePanelObject,winPanelObject,overPanelObject;
    private PausePanelScript pausePanel;
    private GameWinScript winPanel;
    private GameOverScript overPanel;

    private bool pausing=false;
    private bool gaming = true;
    static PauseObject[] _pauseObjects;

    // Use this for initialization
    void Start () {
        pausePanel = pausePanelObject.GetComponent<PausePanelScript>();

        overPanel = overPanelObject.GetComponent<GameOverScript>();

        winPanel = winPanelObject.GetComponent<GameWinScript>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!gaming) return;

        if (enemy == null)
        {
            GameWin();
            return;
        }

        if (player == null)
        {
            GameOver();
            return;
        }

        if(Input.GetKeyUp (KeyCode .Escape))
        {
            if (!pausing)
            {
                Pause();
            }
        }
	}

    void GameOver()
    {
        //transform.gameObject.AddComponent<GameOverScript>();
        overPanelObject .SetActive(true);
        overPanel.SetShow(true);
        gaming = false;
    }

    void GameWin()
    {
        //transform.gameObject.AddComponent<GameWinScript>();
        winPanelObject.SetActive(true);
        winPanel.SetShow(true);
        gaming = false;
    }

    public void Pause()
    {
        pausing = true;
        Time.timeScale = 0;

        _pauseObjects = FindObjectsOfType<PauseObject>();
        foreach (PauseObject pauseObject in _pauseObjects)
        {
            pauseObject.Pause();
        }

        pausePanel.gameObject.SetActive(true);
        pausePanel.SetShow(true);
    }

    public void Resume()
    {
        pausing = false;
        Time.timeScale = 1;

        if (null == _pauseObjects) return;

        foreach (PauseObject pauseObject in _pauseObjects)
        {
            if (pauseObject != null) pauseObject.Resume();
        }


        pausePanel.gameObject.SetActive(false);
        pausePanel.SetShow(false);
    }

    public void Restart()
    {
        gaming = true;


        SceneManager.LoadScene(SceneManager .GetActiveScene().name );
        Resume();
    }

    public void Exit()
    {
        gaming = true;
        SceneManager.LoadScene("Menu");
        Resume();
    }
}
