using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Gui;
using Ggross.Pattern;
using UnityEngine.EventSystems;

public class GameOverPanel : SingletonMonoBehaviour<GameOverPanel>
{   
    [SerializeField] private GameObject panel;
    [SerializeField] private Text title;
    [SerializeField] private GameObject restartButton, exitButton;

    public void SetTitle(string content){
        title.text = content;
    }

    public void Show(){
        panel.SetActive(true);
        restartButton.GetComponent<LeanButton>().Select();
    }

    public void Hide(){
        panel.SetActive(false);
    }


    private void Update() {
        if(!panel.activeInHierarchy) return;

        if(Input.GetKeyDown(KeyCode.Z)){
            var selected = EventSystem.current.currentSelectedGameObject;
            
            if(selected == restartButton){
                GameMain.Instance.RestartGame();
            }
            else if(selected == exitButton)
            {
                GameMain.Instance.ExitGame();
            }
        }

    }
}
