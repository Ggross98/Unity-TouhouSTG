using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ggross.GameManagement{
    public class UIGridPrefab : MonoBehaviour
{
    
    public int x, y;
    public void SetPosition(int _x, int _y){
        this.x = _x;
        this.y = _y;
    }
}

}
