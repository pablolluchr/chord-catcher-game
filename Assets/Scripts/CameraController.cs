using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform character;
    public float smoothSpeed;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - character.transform.position;
    }

    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        Vector3 desiredPosition = character.transform.position + offset;
        Vector3 smoothPosition = Vector3.Slerp(transform.position, desiredPosition,smoothSpeed * Time.deltaTime);
        transform.position = smoothPosition;
    }
}
