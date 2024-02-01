using UnityEngine;

public class ParallaxCameraFollow : MonoBehaviour
{
    public Transform player;  // Player's transform to follow
    public float smoothSpeed = 0.125f;  // Smoothness of camera movement

    public Transform[] parallaxLayers;  // Array of parallax layers
    public float[] parallaxFactors;    // Parallax factors for each layer

    private Vector3 previousPlayerPosition;

    void Start()
    {
        previousPlayerPosition = player.position;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Calculate the movement since the last frame
            Vector3 moveDelta = player.position - previousPlayerPosition;

            // Move the camera
            Vector3 desiredPosition = transform.position + moveDelta;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Move parallax layers
            for (int i = 0; i < parallaxLayers.Length; i++)
            {
                float parallaxX = moveDelta.x * parallaxFactors[i];
                float parallaxY = moveDelta.y * parallaxFactors[i];

                Vector3 layerPosition = parallaxLayers[i].position;
                layerPosition.x += parallaxX;
                layerPosition.y += parallaxY;

                parallaxLayers[i].position = layerPosition;
            }

            // Update the previous player position
            previousPlayerPosition = player.position;
        }
    }
}