using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    protected Joystick joystick;
    public Rigidbody character;
    public float speed;
    public float rotationDamp;


    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<Joystick>();

    }

    // Update is called once per frame
    void Update()
    {
        character.velocity = new Vector3(joystick.Horizontal * speed,
                                           character.velocity.y,
                                        joystick.Vertical * speed);


        float angle = FindDegree(joystick.Direction.x, joystick.Direction.y);

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler(0f, angle, 0f);
        // Dampen towards the target rotation
        character.transform.rotation = Quaternion.Slerp(character.transform.rotation, target, Time.deltaTime * rotationDamp);

    }

    public static float FindDegree(float x, float y)
    {
        float value = (float)((System.Math.Atan2(x, y) / System.Math.PI) * 180f);
        if (value < 0) value += 360f;
        return value;
    }



}
