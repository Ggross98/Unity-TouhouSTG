using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ggross.GameManagement;

public class SimpleImageManager : UISimpleManager<Image>
{
    public Color activeColor, inactiveColor;

    private void Awake() {
        // AddAll(10);
        // Show(2);
    }

    public void Show(int num){
        var size = list.Count;
        num = num > size ? size : num;
        for(int i = 0; i < num; i++){
            list[i].color = activeColor;
        }
        for(int i = num; i<size; i++){
            list[i].color = inactiveColor;
        }
    }
}
