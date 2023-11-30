using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowMgr : MonoBehaviour
{
    private Rigidbody rb;
    private RawImage arrowCameraRawImg;

    private void Start()
    {
        arrowCameraRawImg = GameObject.Find("arrowCameraRawImg").GetComponent<RawImage>();
        rb = GetComponent<Rigidbody>();
    }

    //箭碰撞操作
    private void OnCollisionEnter(Collision collision)
    {
        //剑射在靶上
        if (collision.gameObject.name == "靶边缘")
        {
            //获得是否为移动靶
            var isMove = collision.gameObject.transform.parent.GetComponent<MilitarTargetMgr>().isMove;
            rb.isKinematic = true;
            if (isMove)
            {
                //移动靶加分
                ScoreAdd("移动靶边缘", 2);
            }
            else
            {
                //固定靶加分
                ScoreAdd("固定靶边缘", 1);
            }
            arrowCameraRawImg.enabled = true;
        }
        else if (collision.gameObject.name == "靶心")
        {
            var isMove = collision.gameObject.transform.parent.GetComponent<MilitarTargetMgr>().isMove;
            rb.isKinematic = true;
            transform.parent = collision.gameObject.transform;
            if (isMove)
            {
                //移动靶加分
                ScoreAdd("移动靶心", 3);
            }
            else
            {
                //固定靶加分
                ScoreAdd("固定靶心", 2);
            }
            arrowCameraRawImg.enabled = true;
        }

        if (collision.gameObject.tag == "Tree" || collision.gameObject.tag == "Terrain")
        {
            rb.isKinematic = true;
        }
    }

    //得分
    private void ScoreAdd(string str,int score)
    {
        Tips.Instance.SetText(str, score);
    }
}
