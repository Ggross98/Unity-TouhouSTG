using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBarScript : MonoBehaviour {

    public EnemyScript enemy;

    private Image hpBar;


	// Use this for initialization
	void Start () {
        hpBar = this.GetComponent<Image>();
        hpBar.fillAmount = 1;

	}
	
	// Update is called once per frame
	void Update () {
        if (enemy == null) return;

        this.transform.position = enemy.transform.position;
        
	}

    void OnGUI()
    {
        if(enemy != null)
        {
            hpBar.fillAmount = enemy.GetHp() / enemy.GetMaxHp();
        }
       
    }


}
