using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������պ�
public class SetSkyBox : MonoBehaviour
{
    public Material[] skyMaterials;
    private Skybox skybox;
    int index;

    void Start()
    {
        //��ȡ���
        skybox = GetComponent<Skybox>();
        //���ò���
        skybox.material = skyMaterials[index];
    }

     void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SetMaterial();
        }
    }
    //������պ�
    public void SetMaterial()
    {
        index++;
        if (index >= skyMaterials.Length)
        {
            index = 0;
        }
        skybox.material = skyMaterials[index];
    }
}
