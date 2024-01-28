using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    public int counter = 0;
    public int desiredcounter = 1;
    public GameObject gb1;
    public GameObject gb2;


    private void Update()
    {
        if (counter == desiredcounter) { 
        
            GameObject.Destroy(gb1);
            gb2.SetActive(true);
        }
        else
        {
        
        }
    }



}
