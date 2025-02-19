using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPool : BaseObjectPool<SFX>
{
    protected override SFX OnCreateItem()
    {
        var sfx = base.OnCreateItem();
        sfx.SetOverAction(delegate {
            Release(sfx);
        });
        return sfx;
    }
}
