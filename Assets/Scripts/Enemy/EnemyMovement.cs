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

    //public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        //self.Idle();
        self = GetComponent<Enemy>();
        self.Run();
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
        if (self.isAlly && self.isTouchingPlayerIndirectly)//stop when touching player indirectly
        {
            GetComponent<ITarget>().SetTarget(null);
            return;
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
            //self.Run();
        }
        else
        {
            //self.Idle();
            //wander randomly

        }
    }

    public void PauseMovement(float time)
    {
        canMove = false;
        Invoke("SetBoolBack", time);
    }



    private void SetBoolBack()
    {
        canMove = true;
    }

}
