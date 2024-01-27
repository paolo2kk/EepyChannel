using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Passcode : MonoBehaviour
{
    string code = "123";
    string Nr = null;
    int NrIndex = 0;
    string alpha;
    public Text UiText = null;
 
    public void CodeFunction(string Numbers)
    {
        NrIndex++;
        Nr = Nr + Numbers;
        UiText.text = Nr;
    }
}
