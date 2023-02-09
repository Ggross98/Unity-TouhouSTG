using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserClearBullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public bool IsBombing()
    {
        Input_Key input = GetComponentInParent<Input_Key>();
        if (input != null)
        {

            return input.IsBombing();


        }
        
        
        return false;
        
    }
    /*
    public void Clear()
    {
        GameObject[] m_Desk = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for (int i = 0; i < m_Desk.Length; i++)
        {
            if (m_Desk[i].collider2D.(this.collider2D ))
            {
                Destroy(m_Desk[i]);
            }
        }
    }*/
    
    /*
    void OnTriggerEnter2D(Collider2D otherCollider)
    {
       // if (!Input.GetKey(KeyCode.Mouse0)) return;

        ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();

        if (shot != null && Input.GetKey(KeyCode.Mouse0))
        {
            Destroy(shot.gameObject );
        }
    }
    */
}
