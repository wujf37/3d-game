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

    //����ײ����
    private void OnCollisionEnter(Collision collision)
    {
        //�����ڰ���
        if (collision.gameObject.name == "�б�Ե")
        {
            //����Ƿ�Ϊ�ƶ���
            var isMove = collision.gameObject.transform.parent.GetComponent<MilitarTargetMgr>().isMove;
            rb.isKinematic = true;
            if (isMove)
            {
                //�ƶ��мӷ�
                ScoreAdd("�ƶ��б�Ե", 2);
            }
            else
            {
                //�̶��мӷ�
                ScoreAdd("�̶��б�Ե", 1);
            }
            arrowCameraRawImg.enabled = true;
        }
        else if (collision.gameObject.name == "����")
        {
            var isMove = collision.gameObject.transform.parent.GetComponent<MilitarTargetMgr>().isMove;
            rb.isKinematic = true;
            transform.parent = collision.gameObject.transform;
            if (isMove)
            {
                //�ƶ��мӷ�
                ScoreAdd("�ƶ�����", 3);
            }
            else
            {
                //�̶��мӷ�
                ScoreAdd("�̶�����", 2);
            }
            arrowCameraRawImg.enabled = true;
        }

        if (collision.gameObject.tag == "Tree" || collision.gameObject.tag == "Terrain")
        {
            rb.isKinematic = true;
        }
    }

    //�÷�
    private void ScoreAdd(string str,int score)
    {
        Tips.Instance.SetText(str, score);
    }
}
