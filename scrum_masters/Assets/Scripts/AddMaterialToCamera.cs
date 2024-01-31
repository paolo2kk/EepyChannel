using UnityEngine;
using UnityEngine.SceneManagement;
public class AddMaterialToCamera : MonoBehaviour
{
    public Material customMaterial; 

    void Start()
    {
        if (customMaterial == null)
        {
            Debug.LogError("CustomMaterial not assigned. Please assign a material in the Unity Editor.");
            return;
        }

        Camera mainCamera = GetComponent<Camera>();

        if (mainCamera != null)
        {
            mainCamera.SetReplacementShader(customMaterial.shader, "RenderType");
        }
        else
        {
            Debug.LogError("No Camera component found on the GameObject.");
        }
        

    }
}