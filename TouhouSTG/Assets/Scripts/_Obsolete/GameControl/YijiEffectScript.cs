using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YijiEffectScript : MonoBehaviour {

    public PlayerScript player;
    int nowPlayerLife;
    bool gameOver = false;
	// Use this for initialization
	void Start () {
        nowPlayerLife = player.GetLife();
	}
	
	// Update is called once per frame
	void Update () {
		if(player == null)
        {
            if(!gameOver)
            {
                SoundEffectHelper.Instance.MakeMajSound("top");
                gameOver = true;
            }
            
        }
        else
        {
            if(player.GetLife ()<nowPlayerLife)
            {
                nowPlayerLife = player.GetLife();
                SoundEffectHelper.Instance.MakeMajSound("ron");
            }
        }
	}
}
