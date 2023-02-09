using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIScript : MonoBehaviour {

    public PlayerScript player;

    public Texture lifestar;

    private bool showSpellName = false;

    private string SpellName = "123";


    //fps检测
    public float fpsMeasuringDelta = 2.0f;

    private float timePassed;
    private int m_FrameCount = 0;
    private float m_FPS = 0.0f;



    void OnGUI () {

        Refresh();
	}

    public void Refresh()
    {
        GameObject[] m_Desk = GameObject.FindGameObjectsWithTag("Lifestar");
        for (int i = 0; i < m_Desk.Length; i++)
        {
            Destroy(m_Desk[i]);
        }
       
        float startX = lifestar.width / 3;
        float startY = lifestar.height / 3;

        GUIStyle bb = new GUIStyle();
        bb.normal.background = null;
        bb.normal.textColor = Color.white;
        bb.fontSize = 22;
        GUI.Label(new Rect(startX, startY+5,30, lifestar.height ), "Player", bb);
        GUI.Label(new Rect(startX, startY+35, 30, lifestar.height), "Bomb", bb);
        startX += 70;

        for (int i = 1; i <= player.GetLife(); i++)
        {
            GUI.Label(new Rect(startX + (i - 1) * lifestar.width/2, startY, lifestar.width/2, lifestar.height/2), lifestar);
        }
        for (int i = 1; i <= player.GetBomb(); i++)
        {
            GUI.Label(new Rect(startX + (i - 1) * lifestar.width / 2, startY+30, lifestar.width / 2, lifestar.height / 2), lifestar);
        }

        //显示fps
        GUI.Label(new Rect(Screen.width - 80, startY, 200, 200), "FPS: " + m_FPS, bb);

        if(showSpellName)
        {
            float length = SpellName.Length * 22;
            float xx = (Screen.width - length) / 2;

            GUI.Label(new Rect(xx,startY,length ,50), SpellName , bb);
        }
    }
	
	// Update is called once per frame
	void Update () {
        m_FrameCount = m_FrameCount + 1;
        timePassed = timePassed + Time.deltaTime;

        if (timePassed > fpsMeasuringDelta)
        {
            m_FPS = m_FrameCount / timePassed;

            timePassed = 0.0f;
            m_FrameCount = 0;
        }

	}


    public void ShowSpellName(bool b,string name)
    {
        showSpellName = b;
        if (b)
        {
            SpellName = name;
        }
    }
    
}
