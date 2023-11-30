using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CrossbowMgr : MonoBehaviour
{
    Animator anim;
    //蓄力时间
    float downTime;
    //飞行的力
    float arrowForce;
    //是否蓄力
    bool isEnergyStorage;
    //箭预制体
    public GameObject arrowPrefab;
    //发射点
    public Transform firePoint;

    public RawImage arrowCameraRawImg;

    public bool isFire;
    //是否在发射点
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
            //蓄力功能
            if (isEnergyStorage)
            {   //蓄力阶段按下鼠标左键
                if (Input.GetMouseButtonDown(0))
                {
                    //进入蓄力动画
                    arrowCameraRawImg.enabled = false;
                    anim.SetTrigger("hold");
                }//鼠标按下阶段
                else if (Input.GetMouseButton(0))
                {
                    //计算蓄力时间
                    downTime += Time.deltaTime;
                    //设置蓄力动画
                    anim.SetFloat("hold_power", downTime);
                }//鼠标抬起阶段
                else if (Input.GetMouseButtonUp(0))
                {
                    //蓄力取消
                    isEnergyStorage = false;
                    //为飞行的力赋值
                    arrowForce = downTime;
                    //清空蓄力时间
                    downTime = 0;
                }
            }
            else//发射功能
            {   //蓄力完成发射
                if (Input.GetMouseButtonUp(0))
                {
                    //播放动画
                    anim.SetTrigger("shoot");
                    //设置可以重新蓄力
                    isEnergyStorage = true;
                    //清空所有箭
                    ResetArrow();
                    //发射弩箭
                    FireArrow();
                }
            }
        } 
    }
    
    //弩箭发射
    public void FireArrow()
    {
        firingPosition.count--;
        firingPosText.transform.GetComponent<Text>().text = "当前位置还可以发射次数为:" + firingPosition.count.ToString();
        //实例化箭
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        //获得刚体
        Rigidbody arrowRigidbody = arrow.GetComponent<Rigidbody>();
        //根据蓄力大小发射弩箭
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
    /// 检测全部靶子是否可以发射
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
