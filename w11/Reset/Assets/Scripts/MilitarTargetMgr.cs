using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType
{
    None,
    StraightLine,
    Curve,
}

public class MilitarTargetMgr : MonoBehaviour
{
    
    public bool isMove;


    void Start()
    {

    }

    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Arrow")
        {
            other.gameObject.transform.parent = transform;
        }
    }
}
