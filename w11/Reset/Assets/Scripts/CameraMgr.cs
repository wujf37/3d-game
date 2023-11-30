using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : MonoBehaviour
{
    //鼠标x轴灵敏度
    public float mouseXSensitivity = 80f;
    //人物
    private Transform player;
    //旋转角度
    float xRotation = 0f;

    public bool isMove;
    private void Start()
    {
        player = transform.parent.transform;
        isMove = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseXSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseXSensitivity * Time.deltaTime;
            xRotation -= mouseY;
            //y轴最大旋转角度为正负90;
            xRotation = Mathf.Clamp(xRotation, -45f, 10f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            player.Rotate(Vector3.up * mouseX);
        } 
    }
}