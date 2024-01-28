using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerScript : MonoBehaviour
{
    public GameObject objectToEnable;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Box")
        {
            objectToEnable.SetActive(true);

        }

    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Box")
        {
            objectToEnable.SetActive(false);



        }

    }



}
