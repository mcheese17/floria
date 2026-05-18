using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpdateShaderProperties : MonoBehaviour
{
 

    void Update()
    {
        Shader.SetGlobalVector("_LightDir", transform.forward);
    }
}
