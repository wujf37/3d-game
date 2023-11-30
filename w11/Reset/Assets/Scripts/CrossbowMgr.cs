using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CrossbowMgr : MonoBehaviour
{
    Animator anim;
    //����ʱ��
    float downTime;
    //���е���
    float arrowForce;
    //�Ƿ�����
    bool isEnergyStorage;
    //��Ԥ����
    public GameObject arrowPrefab;
    //�����
    public Transform firePoint;

    public RawImage arrowCameraRawImg;

    public bool isFire;
    //�Ƿ��ڷ����
    public bool isFirePos;

    public FiringPosition firingPosition;

    public GameObject firingPosText;

    public GameObject gameOver;

    void Start()
    {
        anim = GetComponent<Animator>();
        isEnergyStorage = true;
    }

    void Update()
    {
        if (isFire)
        {
            //��������
            if (isEnergyStorage)
            {   //�����׶ΰ���������
                if (Input.GetMouseButtonDown(0))
                {
                    //������������
                    arrowCameraRawImg.enabled = false;
                    anim.SetTrigger("hold");
                }//��갴�½׶�
                else if (Input.GetMouseButton(0))
                {
                    //��������ʱ��
                    downTime += Time.deltaTime;
                    //������������
                    anim.SetFloat("hold_power", downTime);
                }//���̧��׶�
                else if (Input.GetMouseButtonUp(0))
                {
                    //����ȡ��
                    isEnergyStorage = false;
                    //Ϊ���е�����ֵ
                    arrowForce = downTime;
                    //�������ʱ��
                    downTime = 0;
                }
            }
            else//���书��
            {   //������ɷ���
                if (Input.GetMouseButtonUp(0))
                {
                    //���Ŷ���
                    anim.SetTrigger("shoot");
                    //���ÿ�����������
                    isEnergyStorage = true;
                    //������м�
                    ResetArrow();
                    //�������
                    FireArrow();
                }
            }
        } 
    }
    
    //�������
    public void FireArrow()
    {
        firingPosition.count--;
        firingPosText.transform.GetComponent<Text>().text = "��ǰλ�û����Է������Ϊ:" + firingPosition.count.ToString();
        //ʵ������
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        //��ø���
        Rigidbody arrowRigidbody = arrow.GetComponent<Rigidbody>();
        //����������С�������
        arrowRigidbody.velocity = transform.forward * arrowForce * 30f;

        Invoke("AllFirePosCount", 3f);
    }


    public void ResetArrow()
    {
       var arrowObjs = GameObject.FindGameObjectsWithTag("Arrow");

        for (int i = 0; i < arrowObjs.Length; i++)
        {
            Destroy(arrowObjs[i]);
        }
    }

    /// <summary>
    /// ���ȫ�������Ƿ���Է���
    /// </summary>
    public void AllFirePosCount()
    {
        var firePosObjs = GameObject.FindGameObjectsWithTag("FirePos");
        int tempInt = 0;
        for (int i = 0; i < firePosObjs.Length; i++)
        {
            if (firePosObjs[i].transform.GetComponent<FiringPosition>().count <= 0)
            {
                tempInt++;
            }
        }
        if (tempInt >= 5)
        {
            gameOver.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
