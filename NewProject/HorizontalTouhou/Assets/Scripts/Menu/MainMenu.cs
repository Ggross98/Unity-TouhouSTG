
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

    [SerializeField] private GameObject[] buttons;
    [SerializeField] private AudioClip bgm;


    private void Start() {
        BGMManager.Instance.SetClips(new AudioClip[]{bgm});
        BGMManager.Instance.PlayAt(0, false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("StageSelect");
    }

    public void LoadScene(string name){
        SceneManager.LoadScene(name);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Z)){
            var selected = EventSystem.current.currentSelectedGameObject;
            Debug.Log(selected);
            for(int i = 0; i<buttons.Length; i++){
                if(buttons[i] == selected){
                    ClickButton(i);
                    break;
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.X)){
            EventSystem.current.SetSelectedGameObject(buttons[2]);
        }
    }

    public void ClickButton(int index){
        switch(index){
            case 0:
                LoadScene("StageSelect");
                break;
            case 1:
                LoadScene("Config");
                break;
            case 2:
                QuitGame();
                break;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
