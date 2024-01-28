using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerScriptForBoxes : MonoBehaviour
{
    public GameObject objectToEnable;
    TriggerManager triggerManager;


    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Box")
        {
            triggerManager.counter += 1;
        }

    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Box")
        {
            triggerManager.counter -= 1;
        }

    }



}
