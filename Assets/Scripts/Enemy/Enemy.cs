using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Unit
{
    
    public bool isTouchingPlayer;
    //public bool isTouchingEnemy;
    public bool isTouchingAlly;
    public bool isAlly;
    public bool isTouchingPlayerIndirectly; //is touching player through a chain of allies?

    public GameObject player;
    //public Enemy enemyOnCollision;
    public List<GameObject> collidingAllies;

    void Start()
    {
        isAlly = false;
        player = PlayerManager.instance.player;
        SetTarget(player);
        isTouchingPlayer = false;
    }

    private void Update()
    {
        isTouchingPlayerIndirectly = false; //default to false. Will be updated by LateUpdate to true if appropriate
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

    private void LateUpdate()
    {
        //if ally is touching player tell their their colliding allies to spread the word
        if (isTouchingPlayer) { UpdateTouchingPlayerIndirectly(); } else
        {
            if (isAlly && animationState != 1)
            {
                Run();
            }
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) { isTouchingPlayer = true; }
        if (other.gameObject.layer == LayerMask.NameToLayer("Allies")) { collidingAllies.Add(other.gameObject); }
    }

    public void UpdateTouchingPlayerIndirectly()
    {
        if (isAlly && animationState != 0)
        {
            Idle();
        }
        //Tells unaware neighbours that they are touching the player indirectly
        if (isTouchingPlayerIndirectly) return;//if they already know it the chain stops (to avoid infinite loops in the spread of the word)

        isTouchingPlayerIndirectly = true;
        foreach (GameObject c in collidingAllies) { c.GetComponentInChildren<Enemy>().UpdateTouchingPlayerIndirectly(); }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) { isTouchingPlayer = false; }
        if (other.gameObject.layer == LayerMask.NameToLayer("Allies")) { collidingAllies.Remove(other.gameObject); }
    }

    override public void OnDeath()
    {
        lifeState = 2;
        if (isAlly) Destroy(gameObject); //if ally dies then kill them off completely
        GetComponent<EnemyDeath>().Die();
    }


    

    



}
