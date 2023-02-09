using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static Vector3 GetRandomDirection(){
        var angle = Random.Range(0, 360f);
        return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}
