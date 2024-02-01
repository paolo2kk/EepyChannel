using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restarti : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            SceneManager.LoadScene(0);
        }
    }
}
