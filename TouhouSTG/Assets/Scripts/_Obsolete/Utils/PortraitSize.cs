using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitSize : MonoBehaviour {

	// Use this for initialization
	void Start () {

        float _x = Screen.width / 2;
        float width = Screen.width / 2.2f;
        float height = width / 64 * 72;
        float _y = (Screen.height - height) / 2;

        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            GetComponent<RectTransform>().localPosition = new Vector2(_x, _y);
            GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
