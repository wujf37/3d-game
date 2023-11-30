using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//设置天空盒
public class SetSkyBox : MonoBehaviour
{
    public Material[] skyMaterials;
    private Skybox skybox;
    int index;

    void Start()
    {
        //获取组件
        skybox = GetComponent<Skybox>();
        //设置材质
        skybox.material = skyMaterials[index];
    }

     void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SetMaterial();
        }
    }
    //设置天空盒
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
