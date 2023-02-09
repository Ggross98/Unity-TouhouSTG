using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarragePool : BaseObjectPool<Barrage>
{
    protected override Barrage OnCreateItem()
    {
        var barrage = base.OnCreateItem();
        barrage.SetDeactiveAction(delegate {
            Release(barrage);
        });
        return barrage;
    }
}
