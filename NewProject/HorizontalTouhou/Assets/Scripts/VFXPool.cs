using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXPool : BaseObjectPool<VFX>
{
    protected override VFX OnCreateItem()
    {
        var vfx = base.OnCreateItem();
        vfx.pool = this;
        
        return vfx;
    }
}
