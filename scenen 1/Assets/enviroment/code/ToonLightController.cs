using UnityEngine;

public class ToonLightController : MonoBehaviour
{
    public Light dirLight;

    [Range(0f, 10f)]
    public float lightMultiplier = 1f;

    void Update()
    {
        if (dirLight == null)
            return;

        Shader.SetGlobalVector(
            "_LightDir",
            -dirLight.transform.forward
        );

        float strength =
            dirLight.intensity * lightMultiplier;

        Shader.SetGlobalFloat(
            "_LightStrength",
            strength
        );
    }
}