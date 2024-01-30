using UnityEngine;

public class MoveBackAndForth : MonoBehaviour
{
    public Transform pointA; // Assign the starting position in the Unity Editor
    public Transform pointB; // Assign the target position in the Unity Editor
    public float speed = 5f; // Adjust the speed as needed

    private bool movingToB = true; // Flag to track movement direction

    private void Update()
    {
        if (movingToB)
        {
            MoveToPoint(pointB.position);
        }
        else
        {
            MoveToPoint(pointA.position);
        }
    }

    private void MoveToPoint(Vector3 targetPosition)
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        // Check if the object has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            // Change direction when reaching the target
            movingToB = !movingToB;
        }
    }
}
