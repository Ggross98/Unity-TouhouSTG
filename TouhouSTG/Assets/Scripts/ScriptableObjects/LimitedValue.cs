using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LimitedValue
{
    public float value = 0;
    public float min = 0, max = 0;

    public LimitedValue(float v, float min, float max){
        value = v;
        this.min = min;
        this.max = max;
    }
}
