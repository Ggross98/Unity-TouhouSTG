using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Lean.Gui;
using UnityEngine.EventSystems;

public class StageSelectMenu : MonoBehaviour
{   

    [SerializeField] private List<GameObject> stages;
    [SerializeField] private List<string> stageNames;

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            LoadScene("MainMenu");
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            for(int i = 0; i<stages.Count; i++){
                if(stages[i] == EventSystem.current.currentSelectedGameObject){
                    BGMManager.Instance.Stop();
                    LoadScene(stageNames[i]);
                    return;
                }
            }
        }
    }

}


