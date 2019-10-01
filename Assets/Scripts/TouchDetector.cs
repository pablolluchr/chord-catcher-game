using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDetector : MonoBehaviour
{
    //FOR COMPUTER --CLICKS INSTEAD OF TOUCHES
    // Start is called before the first frame update
    private Vector2 baseClick;
    private Vector2 offset;
    //private float offsetLimit = 100f; //TODO: make this relative to the screen resolution??
    public RectTransform joystick;
    public RectTransform handle;
    public float Horizontal;
    public float Vertical;
    public Vector2 Direction;
    void Start()
    {
        
    }
    //TODO: center joystick properly

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.buttonBeingPressed) return; //if button is clicked then ignore everything

        if (Input.GetMouseButtonDown(0))
        {
            //first click
            baseClick = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            joystick.position = new Vector3(baseClick.x - joystick.rect.width / 2, baseClick.y - joystick.rect.height / 2);
        }

        if (!Input.GetMouseButton(0))//if button is not being held
        {

            joystick.position = new Vector2(-joystick.rect.width*2, -joystick.rect.height*2);
            Horizontal = 0;
            Vertical = 0;
            Direction = new Vector2(0f, 0f);
        }
        else
        {
            //mouse being held
            offset = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - baseClick;
            offset = new Vector2(offset.x / offset.magnitude, offset.y / offset.magnitude); //normalized offset
            //offset = new Vector2(Mathf.Clamp(offset.x, -1 * offsetLimit, offsetLimit), Mathf.Clamp(offset.y, -1 * offsetLimit, offsetLimit));
            Horizontal = offset.x;
            Vertical = offset.y;
            if (System.Single.IsNaN(Horizontal)) Horizontal = 0f;
            if (System.Single.IsNaN(Vertical)) Vertical = 0f;
            handle.position = new Vector2(joystick.position.x + joystick.rect.width / 2 + Horizontal * handle.rect.height,
                joystick.position.y + joystick.rect.height / 2 + Vertical * handle.rect.width);
            Direction = new Vector2(offset.x, offset.y);
        }


    }
}
