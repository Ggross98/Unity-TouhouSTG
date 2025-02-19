using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseObject : MonoBehaviour {
    protected bool _isPause = false;

    public virtual bool Pause()
    {
        if (_isPause) return false;

        _isPause = true;

        return true;
    }

    public virtual bool Resume()
    {
        if (!_isPause) return false;

        _isPause = false;

        return true;
    }
}
