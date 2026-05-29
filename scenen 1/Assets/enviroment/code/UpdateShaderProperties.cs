using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Light))]
public class UpdateShaderProperties : MonoBehaviour
{
    private Light myLight;

    void Start()
    {
        myLight = GetComponent<Light>();
    }

    void Update()
    {
        // 1. Truyền hướng đèn
        Shader.SetGlobalVector("_LightDir", transform.forward);

        // 2. Truyền Cường độ đèn 
        Shader.SetGlobalFloat("_LightIntensity", myLight.intensity);

    }
}