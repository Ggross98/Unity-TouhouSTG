using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyNoteScript : MonoBehaviour {

    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetFlag(int i)
    {
        animator.SetInteger("flag",i);
        
        if (i == 2)
        {
            StartCoroutine(OverDestroy());
        }
    }

    private IEnumerator OverDestroy()
    {
        AnimatorStateInfo info;
        while (this!=null )
        {
            info = animator.GetCurrentAnimatorStateInfo(0);
            if(info.normalizedTime >1f&&info.IsName ("over"))
            {
               Destroy(gameObject);
            }
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }
}
