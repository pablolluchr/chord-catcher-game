using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool canMove = true;
    public float timeBetweenMoves;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float rotationDamp = 0.1f;
    public Vector3 targetAngle;
    public float lookRadius = 10f;
    private Enemy oneself;

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        oneself = GetComponent<Enemy>();
        //IF ENEMIES ARE ALLIES TAKE TARGET FROM PLAYER
        target = PlayerManager.instance.player.transform;

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    private void FixedUpdate()
    {
        if (oneself.isAlly) //attack the enemy the playing is attacking
        {
            if (PlayerManager.instance.player.GetComponent<PlayerAttack>().lockedEnemy == null) //if player doesn't have a locked enemy 
            {//walk towards player

                target = PlayerManager.instance.player.transform;
                if (oneself.isTouchingPlayerIndirectly) target = null; //stop when touching player
                
            }
            else
            {
                target = PlayerManager.instance.player.GetComponent<PlayerAttack>().lockedEnemy.transform;
                if (target.GetComponent<Enemy>().lifeState != 0) target = null;//if target is not alive set to null

            }
        }

        if (target == null) return; //if despite the previous code the target is still null then don't move

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius && canMove)
        {
            //start chasing character
            var lookPos = target.position - transform.position;
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

    public void StopMoving(float time)
    {
        canMove = false;
        Invoke("SetBoolBack", time);
    }



    private void SetBoolBack()
    {
        canMove = true;
    }

}
