using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyNoteTest : MonoBehaviour {

    public Transform note;
    private CyNoteScript cy;
    float time;
    int flag=0;
	// Use this for initialization
	void Start () {
        cy = note.GetComponent<CyNoteScript>();
	}
	
	// Update is called once per frame
	void Update () {
        if (time > 1 && flag == 0)
        {
            cy.SetFlag(1);
            flag = 1;
        }
        if (time > 2 && flag == 1)
        {
            cy.SetFlag(2);
            flag = 2;
        }
        time += Time.deltaTime;
	}
}
