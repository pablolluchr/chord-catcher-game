using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles the animation logic and the target management of enemies.
public abstract class Enemy : Unit
{
    
    public bool isAlly;
    public GameObject player;
    
    void Start()
    {
        isAlly = false;
        player = PlayerManager.instance.player;
        SetTarget(player);
    }

    private void Update()
    {

        //SET UP ANIMATION STATE
        if (GetComponent<Rigidbody>().velocity.magnitude < new Vector3(0.1f,0.1f,0.1f).magnitude) {
            if(animationState!=0 && !isAttacking) //TODO: CHANGE animationState to enums
            {
                Idle(); //TODO: Rename animation functions to IdleAnimation();
            }
        }
        else
        {
            if (animationState != 1 && !isAttacking)
            {
                Run(); 
            }
        }
        if(isAttacking && animationState != 2)
        {
            Attack(1);
        }

        if (target != null)
        {
            if (target.GetComponent<Unit>().lifeState == 2) { target = null; } //stop going after an enemy when they die
        }

        if (isAlly)
        {
            if (player.GetComponent<ITarget>().GetTarget() != null) SetTarget(player.GetComponent<ITarget>().GetTarget()); //if it's an ally then use the player's target as target
            else SetTarget(player); //if they don't have a target then follow player
        }
    }

    override public void OnDeath()
    {
        GetComponent<EnemyDeath>().Die(); //TODO: implement using interface so Enemy doesn't need to know what script is implementing the death function
    }


    

    



}
