using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IPauseMovement
{
    public bool canMove = true;
    public float timeBetweenMoves;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float rotationDamp = 0.1f;
    public Vector3 targetAngle;
    public float lookRadius = 10f;
    private Enemy self;
    public float distanceToWanderAroundPlayer;

    public Vector3 offsetFromPlayer; //TODO: Assign a random offset from player to each enemy so they spread around player evenly and not in an ugly line
    //Enemies always move towards their target. Enemies (non allies) always have the player as a target
    //Allies have either the player's target (if this is not null) or the player itself. IF the latter then they WanderAroundPlayer()

    // Start is called before the first frame update
    void Start()
    {
        //this offset needs to be smaller than the distance to wander around playe. make this evenly distributed (there's a limit to the team so it could be done by hand
        offsetFromPlayer = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f));
        
        self = GetComponent<Enemy>();
        //IF ENEMIES ARE ALLIES TAKE TARGET FROM PLAYER

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    private void FixedUpdate()
    {
        GameObject target = GetComponent<ITarget>().GetTarget();

        if (self.isAlly && target == PlayerManager.instance.player) //if the enemy is going after the player and the enemy is an ally 
        {
            //if ally is fairly close to player then wander around them
            float distanceToPlayer = Vector3.Distance(target.transform.position, transform.position);
            if (distanceToPlayer <= distanceToWanderAroundPlayer)
            {
                WanderAroundPlayer();
                return;

            }
        }

        try { if (target.GetComponent<Enemy>().lifeState != 0) target = self.player; }//if target dies go after player
        catch (NullReferenceException) { }

        
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance <= lookRadius && canMove)
        {
            //start chasing character
            var lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.time * rotationSpeed);
            GetComponent<Rigidbody>().velocity = transform.forward * moveSpeed;
        }
        else
        {
            //wander randomly
        }
    }
    public void WanderAroundPlayer() //called for allies to wander around the player when they dont have a target
    {
        Vector3 lookPos = (self.target.transform.position + offsetFromPlayer) - transform.position ; //TODO: transition smoothy to rotation
        lookPos.y = 0;
        if (lookPos.magnitude < 2f || (self.target.transform.position- transform.position).magnitude<2f) //if its already at its destination or very close to the player then stop
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            return;
        }
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.time * rotationSpeed);
        GetComponent<Rigidbody>().velocity = transform.forward * moveSpeed;
    }

    public void PauseMovement(float time) //pause move of the enemy. Used to stop the enemy from moving briefly when hit by projectile
    {
        canMove = false;
        Invoke("SetBoolBack", time);
    }



    private void SetBoolBack()
    {
        canMove = true;
    }

}
