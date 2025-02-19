using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Ggross.Pattern;
using Ggross.Dialogue;

public class GameMain : SingletonMonoBehaviour<GameMain>
{   
    [SerializeField] private DialogueManager dialogue;
    [SerializeField] private PlayerController player;
    [SerializeField] private EnemyController enemy;
    [SerializeField] private GameOverPanel overPanel;
    [SerializeField] private AudioClip bgm;

    public bool getSpellBonus;
    private int spellBonus;

    public void StartDanmaku(){
        getSpellBonus = true;
    }

    public void EndDanmaku(bool isSpellcard){
        if(getSpellBonus){
            spellBonus += (isSpellcard ? 5000 : 1000);
        }
    }


    public DialogueData dialogueData;

    public enum GameState {
        Intro,
        Pausing,
        Playing,
        Over
    }
    public GameState state = GameState.Intro;

    private void Awake() {
        
    }

    private void Start() {
        
        overPanel.Hide();
        GameUI.Instance.HideAll();

        BGMManager.Instance.SetClips(new AudioClip[]{bgm});

        player.GetComponent<PlayableDirector>().Play();


        

    }


    private void Update() {
        Application.targetFrameRate = 120;
        
        switch(state){
            case GameState.Intro:
                if(dialogue.IsShowing()){
                    if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X)){
                        dialogue.ShowNextSentence();
                    }
                    if(Input.GetKeyDown(KeyCode.Escape)){
                        dialogue.EndSentence();
                    }
                }

                break;

            case GameState.Playing:
                if(Input.GetKeyDown(KeyCode.Escape)){
                    ExitGame();
                }
                break;

        }
    }

    public void ShowIntroDialogue(){
        dialogue.LoadDialogue(dialogueData);
        dialogue.endAction = RestartGame;
        dialogue.StartDialogue();
    }

    public void RestartGame(){
        // player.SetInteractive(true);
        // player.gameObject.SetActive(true);
        player.StartBattle();

        // enemy.SetInteractive(true);
        // enemy.gameObject.SetActive(true);
        enemy.StartBattle();

        overPanel.Hide();
        GameUI.Instance.ShowAll();

        state = GameState.Playing;

        BGMManager.Instance.PlayAt(0, false);

        spellBonus = 0;
    }

    public void GameOver(bool win = false){

        if(state == GameState.Over) return;

        overPanel.SetTitle(win ? "Stage Clear" : "Game Over");
        overPanel.Show();
        overPanel.ShowScore(win ? 20000 : 2000, player.life * 5000, spellBonus);

        GameUI.Instance.HideAll();

        player.SetInteractive(false);
        enemy.SetInteractive(false);
        enemy.gameObject.SetActive(!win);

        state = GameState.Over;
    }

    public void ExitGame(){
        SceneManager.LoadScene("StageSelect");
    }


}
