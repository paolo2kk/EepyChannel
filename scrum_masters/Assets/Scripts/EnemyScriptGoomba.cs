using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private float t = 0f;

    void Update()
    {
        MoveEnemy();
    }

    void MoveEnemy()
    {
        t += Time.deltaTime * speed;

        if (t > 1.0f)
        {
            Transform temp = pointA;
            pointA = pointB;
            pointB = temp;

            t = 0f;
        }

        transform.position = Vector3.Lerp(pointA.position, pointB.position, t);
    }
}