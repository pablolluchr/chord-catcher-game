using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //MOVES player according to joystick
    protected TouchDetector joystick;
    private Rigidbody character;
    public float moveSpeed = 10;
    public float rotationDamp= 10;



    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<TouchDetector>();
        character = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        character.velocity = new Vector3(joystick.Horizontal * moveSpeed,
                                           character.velocity.y,
                                        joystick.Vertical * moveSpeed);
        if (character.velocity.magnitude < Mathf.Epsilon) return;
        float angle = FindDegree(joystick.Direction.x, joystick.Direction.y);

        //rotate player according to joystick
        Quaternion target = Quaternion.Euler(0f, angle, 0f);

        // Dampen towards the target rotation - angle = 0 is understood to be the default position where no input is given
        if(angle > 0 || angle < 0)
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, target, Time.deltaTime * rotationDamp);

    }

    public static float FindDegree(float x, float y) //find the angle in degrees of the hypothenuse given the two legs x and y
    {
        float value = (float)((System.Math.Atan2(x, y) / System.Math.PI) * 180f);
        if (value < 0) value += 360f;
        return value;
    }
}
