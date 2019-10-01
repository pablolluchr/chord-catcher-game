  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    private PlayerAttack attack;
    private TouchDetector joystick;
    public float attackSpeed = 1f;


    // Start is called before the first frame update
    void Start()
    {
        animationState = 0;
        
        //set up private variables
        joystick = FindObjectOfType<TouchDetector>();
        anim = GetComponent<Animator>(); //set animator of parent
        attack = GetComponent<PlayerAttack>();
  

    }

    void FixedUpdate()
    {
        //look at enemy
        if (animationState == 0) attack.FindNearestEnemy();//check for enemies only when player is idle

        if (animationState != 1)
        {
            attack.canFire = true;
            attack.LookAtEnemy(); //look at enemy as long as player is not moving
        }

        if (animationState==1) attack.canFire = false; //if char is moving then stop fire

        //set up animation and state variables
        UpdateState();
    }

    //in order for the player to go into attack mode it first has to go through idle.
    private void UpdateState()
    {
        bool movingCondition = joystick.Horizontal > 0 || joystick.Horizontal < 0 || joystick.Vertical > 0 || joystick.Vertical < 0;


        if (movingCondition) //character is moving
        {
            //if just entered running state then play running animation
            if (animationState != 1) Run();
        }

        else //character not moving
        {
            if (animationState == 1) Idle();
            else if (animationState == 0 && GetTarget() != null) Attack(attackSpeed); //enemy appeared in player's range while player while player was idle
            else if (animationState == 2 && GetTarget() == null) Idle(); //enemies died or disappeared from range while char was firing

        }


    }
}
