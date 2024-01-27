using UnityEngine;

public class ObjectSynchronization : MonoBehaviour
{
    public Transform counterpartObject;

    void Update()
    {
        SynchronizePosition();

    }

    void SynchronizePosition()
    {
        counterpartObject.position = transform.position;
    }

   
}