using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript :PauseObject  {

    private Vector3 speed = new Vector3(10,10,0);

    private Vector3 direction = new Vector3(1, 0,0);

    private Vector3 movement;

    private Rigidbody2D rigidbody2D;

    void Awake()
    {
        rigidbody2D = this.GetComponent<Rigidbody2D>();
    }


    void Update()
    {

        if (_isPause) return;

        rigidbody2D.velocity = new Vector2(0, 0);

        float inputX=0;
        float inputY=0;

        bool right = Input.GetKey(KeyCode .RightArrow );
        bool left = Input.GetKey(KeyCode.LeftArrow);
        bool up = Input.GetKey(KeyCode.UpArrow);
        bool down = Input.GetKey(KeyCode.DownArrow);

        if (right) inputX = 1;
        if (left) inputX = -1;
        if (up) inputY = 1;
        if (down) inputY = -1;
        
        if (inputX * inputY != 0)
        {
            inputX *= Mathf.Sqrt (2)/2;
            inputY *= Mathf.Sqrt(2)/2;
        }
        

        //低速
        bool focus = Input.GetKey(KeyCode.LeftShift);
        //this.transform.Find("point").gameObject.SetActive(focus);

        if (focus)
        {
            movement = new Vector2(speed.x * inputX * 0.4f, speed.y * inputY*0.4f);
        }
        else
        {
            movement = new Vector2(speed.x * inputX, speed.y * inputY);

        }


        transform.position = transform.position + movement * Time.deltaTime;
        //rigidbody2D.position = transform.position;


        //限制不离开屏幕
        var dist = (transform.position - Camera.main.transform.position).z;
        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

        transform.position = new Vector3(
          Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
          Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
          transform.position.z
        );


    }

    void FixedUpdate()
    {
        rigidbody2D.velocity = new Vector2(0, 0);
    }
}
