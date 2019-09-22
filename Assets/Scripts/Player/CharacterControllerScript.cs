    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    private bool isIdle; //character position is not changing (char is either not doing anything or shooting)
    private bool isRunning; 
    private bool isFiring; 

    private bool enemyInRange;

    private Animator anim;
    private Rigidbody rb;
    private Shooting shootingScript;
    private CharacterMovement movementScript;
    private Joystick joystick;


    // Start is called before the first frame update
    void Start()
    {

        Screen.fullScreen = !Screen.fullScreen;
        Screen.SetResolution(1080, 1920, false);
        //set up private variables
        joystick = FindObjectOfType<Joystick>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        shootingScript = GetComponent<Shooting>();
        movementScript = GetComponent<CharacterMovement>();
        isRunning = false;
        isIdle = true;
        isFiring = false;

    }


    void FixedUpdate()
    {

        //set up fire
        enemyInRange = false;   
        if (isIdle) //check for enemies only if player is idle
        {
            //look for enemies and potentially initiate fire
            enemyInRange = shootingScript.FindNearestEnemy();
            if(enemyInRange)
                shootingScript.LookAtEnemy();
        }
        else //if char is moving then stop fire
        {
            //stop fire 
            shootingScript.m_canFire = false;
        }

        //set up animation and state variables
        SetUpState();
    }

    private void SetUpState()
    {
        bool movingCondition = joystick.Horizontal > 0 || joystick.Horizontal < 0 || joystick.Vertical > 0 || joystick.Vertical < 0;


        if (movingCondition) //character is moving
        {
            //if just entered running state then play running animation
            if (!isRunning)
            {
                anim.CrossFade("Run", 0.1f);
                isRunning = true;
                isIdle = false;
                isFiring = false;
            }
        }
        else //character not moving
        {
            if (!isIdle) //if just entered idle state then play idle animation
            {
                anim.CrossFade("Idle", 0.1f);
                isRunning = false;
                isIdle = true;
                isFiring = false;
            }

            //enemy just got in range
            else if (!isFiring && enemyInRange)
            {
                anim.CrossFade("Attack", 0.1f);
                //anim.speed = 0.1;
                isFiring = true;
                isIdle = true;

            }

            //enemies died or disappear from range while char was firing
            else if (isFiring && !enemyInRange)
            {
                anim.CrossFade("Idle", 0.1f);
                isFiring = false;
                isIdle = true;

            }
        }


    }
}
