using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering.PostProcessing;


public class LSDController : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    private ColorGrading colorGrading;

    private void Start()
    {
        postProcessVolume.profile.TryGetSettings(out colorGrading);

        StartCoroutine(ChangeColorGrading());
    }

    public IEnumerator ChangeColorGrading()
    {
        while (true)
        {
            colorGrading.temperature.value += 0.1f;
            colorGrading.tint.value += 0.1f;


            yield return new WaitForSeconds(1.0f);
        }
    }
}
