using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ggross.Pattern;

public class GameUI : SingletonMonoBehaviour<GameUI>
{

    [SerializeField] private SimpleImageManager bombBar, lifeBar;
    [SerializeField] private FilledRing enemyHPRing;
    [SerializeField] private Text spellCardName;
    [SerializeField] private PlayerController player;
    [SerializeField] private EnemyController enemy;

    private bool showing = false;

    public FilledRing GetEnemyHPRing(){
        return enemyHPRing;
    }

    public void ShowSpellName(string name){
        spellCardName.text = name;
    }

    public void ShowHPRing(){
        enemyHPRing.Show();
    }

    public void FillHPRing(){
        enemyHPRing.Show();
        enemyHPRing.Fill();
    }

    public void HideHPRing(){
        enemyHPRing.Hide();
    }

    public void HideAll(){
        showing = false;
        // gameObject.SetActive(false);

        lifeBar.gameObject.SetActive(false);
        bombBar.gameObject.SetActive(false);
    }

    public void ShowAll(){
        lifeBar.gameObject.SetActive(true);
        bombBar.gameObject.SetActive(true);

        showing = true;
        gameObject.SetActive(true);
    }

    private void Start() {
        lifeBar.AddAll(10);
        bombBar.AddAll(10);
    }

    private void Update() {
        if(showing){
            lifeBar.Show(player.life);
            bombBar.Show(player.bomb);
            enemyHPRing.Refresh(enemy);
        }
    }
}
