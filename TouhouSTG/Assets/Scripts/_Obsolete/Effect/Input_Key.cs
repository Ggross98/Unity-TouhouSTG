using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Key : MonoBehaviour {

    public ParticleSystem my_PS;

    public Transform player;

    private bool isBombing = false;

    private float time = 0f;

	// Use this for initialization
	void Awake () {
        if (GetComponent<ParticleSystem>() != null)
        {
            my_PS = GetComponent<ParticleSystem>();
        }
        else
        {
            Debug.LogError("这个脚本是挂在粒子物体身上的~");
        }

        //player = GetComponentInParent<PlayerScript>().transform;

        if (player != null)
        {
            my_PS.transform.position = new Vector3 (player.position .x+38 ,player.position .y,player.position .z);
        }
        
        my_PS.Stop();//初始化停止粒子

       // SpecialEffectsHelper.Instance.ClearEnemyBullet(new Bounds (my_PS.transform .position ,new Vector3 (10,10,10)));
    }
	
	// Update is called once per frame
	void Update () {
        if (player != null)
        {
            my_PS.transform.position = new Vector3(player.position.x + 38, player.position.y, player.position.z);
            GetComponentInChildren <Collider2D >().transform .position = new Vector3(player.position.x, player.position.y, player.position.z);
        }
        if (time>0)
        {
            if (!isBombing)
            {
                //my_PS.Play();
                isBombing = true;
            }

            time -= Time.deltaTime;
        }
        else
        {
            if(isBombing)
            { 
                //my_PS.Stop();
                isBombing = false;
            }
        }
    }

    public bool IsBombing()
    {
        return isBombing;
    }

    public void Bomb(float time)
    {
        if(!isBombing)
        {
            this.time = time;
        }
    }

    /*
    void OnParticleCollision(GameObject   other)
    {
        Debug.Log("发生了粒子碰撞，碰撞到的物体为："+other.name);
    }

    void OnTriggerEnter2D(Collider2D  collider)
    {
        ShotScript shot = collider.gameObject.GetComponent<ShotScript>();
        if (shot != null)
        {
            Destroy(shot.gameObject);
        }
    }*/
}
