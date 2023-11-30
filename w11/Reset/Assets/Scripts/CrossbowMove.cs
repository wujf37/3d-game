using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowMove : MonoBehaviour
{
    //人物控制器
    private CharacterController controller;
    //人物移动速度
    public float speed = 2f;
    public float gravity = -15f;
    Vector3 velocity;
    //是否可以移动摄像机
    bool isMove;

    private CameraMgr cameraMgr;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraMgr = transform.GetChild(0).GetComponent<CameraMgr>();
        isMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //按键Q隐藏 或者显示鼠标
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isMove = !isMove;
            isMove = cameraMgr.isMove;
            if (isMove == false)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        if (isMove)
        {
            Move();
        }
    }

    //移动
    public void Move()
    {
        //键盘输入
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }


}
