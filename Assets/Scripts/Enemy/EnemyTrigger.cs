using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//sets up states in Enemy states according to what the enemy is interacting with.
//The trigger could be understood as the range of attack. A trigger can be arbitrarily large and doesn't need to match the size of the unit.
//TODO: When ranged enemies are introduced this whole thing should be changed, as movement related things should be determined by OnCollisionEnter
//and attack related things by a function that detects the enemies in range (in the same way that the player does it).
//This code shoould be put into Unit (for example the function that detects the units in range --regardless of the class) . Then this information will be handled within enemy.
//Melee and ranged units should have the same code just with a different value for range. Fuck trigger. Literally the same idea as the player's code. So all of this should lay within unit
public class EnemyTrigger : MonoBehaviour
{

    public Enemy self;
    public List<Collider> collidingAllies;

    private void Start()
    {
        self = GetComponent<Enemy>();
    }

    private void Update()
    {
        //isTouchingPlayerIndirectly defaults to false. Will be updated in LateUpdate otherwise
        self.isTouchingPlayerIndirectly = false;
       
        //if ally kills enemy then set the enemyoncollision to null and isTouchingEnemy to false
        if (self.enemyOnCollision != null)
        {
            if (self.enemyOnCollision.lifeState == 2) 
            {
                self.enemyOnCollision = null;
                self.isTouchingEnemy = false;
            }
        }
        
    }

    private void LateUpdate()
    {
        //if ally is touching player tell their their colliding allies to spread the word
        if (self.isTouchingPlayer)
        {
            UpdateTouchingPlayerIndirectly();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            self.enemyOnCollision = other.GetComponent<Enemy>();
            self.isTouchingEnemy = true;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            self.isTouchingPlayer = true;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Allies"))
        {
            self.isTouchingAlly = true;
            collidingAllies.Add(other);
        }
        
    }

    public void UpdateTouchingPlayerIndirectly()
    {
        //Tells unaware neighbours that they are touching the player indirectly
        if (self.isTouchingPlayerIndirectly) return;//if they already know it the chain stops (to avoid infinite loops in the spread of the word)

        self.isTouchingPlayerIndirectly = true;
        foreach(Collider c in collidingAllies)
        { 
            c.GetComponentInChildren<EnemyTrigger>().UpdateTouchingPlayerIndirectly();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemies")) self.isTouchingEnemy = false;
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            self.isTouchingPlayer = false;

        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Allies"))
        {
            self.isTouchingAlly = false;
            collidingAllies.Remove(other);

        }


    }


}
