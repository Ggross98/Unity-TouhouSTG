using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotPool : BaseObjectPool<Shot>
{

    protected override Shot OnCreateItem()
    {
        var shot = base.OnCreateItem();
        shot.SetDeactiveAction(delegate {
            Release(shot);
        });
        return shot;
    }

    // protected override void OnGetItem(Shot item)
    // {
    //     base.OnGetItem(item);
    //     item.SetDeactiveAction(delegate {
    //         Release(item);
    //     });
    // }
}
