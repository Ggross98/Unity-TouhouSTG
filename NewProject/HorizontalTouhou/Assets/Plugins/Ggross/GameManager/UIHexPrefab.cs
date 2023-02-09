using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ggross.GameManagement{
    public class UIHexPrefab : MonoBehaviour
{
    public int x,y;
    public int q,r,s;

    private RectTransform rect;
    private void Awake() {
        rect = GetComponent<RectTransform>();
    }

    public void SetLocalPosition(Vector2 p){
        rect.localPosition = p;
    }

    public void SetSizeDelta(Vector2 sd){
        rect.sizeDelta = sd;
    }
}

}
