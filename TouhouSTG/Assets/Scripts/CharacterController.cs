using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterController : MonoBehaviour
{
    [SerializeField] protected Animator animator;

    protected bool interactive = false;
    public void SetInteractive(bool b){
        interactive = b;
        // gameObject.SetActive(b);
    }

    public virtual void StartBattle(){}

    protected IEnumerator WaitForSeconds(float duration, System.Action action){
        yield return new WaitForSeconds(duration);
        action.Invoke();
    }
}
