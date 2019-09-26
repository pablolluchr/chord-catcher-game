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
    private EnemyController controller;

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<EnemyController>();
        //IF ENEMIES ARE ALLIES TAKE TARGET FROM PLAYER
        target = PlayerManager.instance.player.transform;

    }

    private void Update()
    {
        if (controller.isAlly)
        {
            //attack the enemy the player is attacking
            target = PlayerManager.instance.player.GetComponent<Shooting>().m_lockedEnemy.GetComponent<Transform>();
            if (PlayerManager.instance.player.GetComponent<Shooting>().m_lockedEnemy.GetComponentInParent<EnemyHealth>().isDead)
            {
                target = null;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    private void FixedUpdate()
    {
        if (target == null) return; //TODO: FOLLOW PLAYER INSTEAD

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
