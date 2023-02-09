using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyButtonScript : MonoBehaviour {
    private Sprite image0;
    public Sprite image1;

    private Image image;

    private bool isSelected = false;

	// Use this for initialization
	void Start () {
        image = this.GetComponent<Image>();
        image0 = image.sprite;
        
	}


    void Adjust() { }
	
	void OnGUI()
    {
        if(isSelected)
        {
            image.sprite = image1;
        }
        else
        {
            image.sprite = image0;
        }
    }

    public void SetSelected(bool s)
    {
        isSelected = s;
    }

    public bool IsSelected()
    {
        return isSelected;
    }
}
