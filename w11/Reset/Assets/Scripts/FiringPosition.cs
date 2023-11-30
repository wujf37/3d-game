using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FiringPosition : MonoBehaviour
{
    //????
    public int count;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (count > 0)
            {
                CrossbowMgr crossbowMgr = other.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<CrossbowMgr>();
                crossbowMgr.isFire = true;
                crossbowMgr.firingPosition = this;
                crossbowMgr.firingPosText.SetActive(true);
                crossbowMgr.firingPosText.transform.GetComponent<Text>().text = "当前位置还可以发射次数为:" + crossbowMgr.firingPosition.count.ToString();
            }
            else
            {
                CrossbowMgr crossbowMgr = other.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<CrossbowMgr>();
                crossbowMgr.isFire = false;
                crossbowMgr.firingPosition = this;
                crossbowMgr.firingPosText.SetActive(true);
                crossbowMgr.firingPosText.transform.GetComponent<Text>().text = "当前位置还可以发射次数为:" + crossbowMgr.firingPosition.count.ToString();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CrossbowMgr crossbowMgr = other.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<CrossbowMgr>();
            crossbowMgr.isFire = false;
            crossbowMgr.firingPosition = null;
            crossbowMgr.firingPosText.SetActive(false);
        }
    }
}
