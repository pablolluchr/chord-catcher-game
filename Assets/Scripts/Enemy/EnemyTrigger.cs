using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//sets up states in Enemy according to what the enemy is interacting with
public class EnemyTrigger : MonoBehaviour
{

    public Enemy oneself;
    public Enemy lastCollisionedWith;
    private void OnTriggerEnter(Collider other)
    {
        //check if it's an ally and it's attacking another enemy
        if (oneself.isAlly)
        {
            //oneself.isAllyTouchingEnemy |= other.gameObject.layer == LayerMask.NameToLayer("Enemies");
            if(other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            {
                oneself.isAllyTouchingEnemy = true;
                oneself.enemyOnCollision = other.GetComponent<Enemy>();
            }

        }
        else
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Main character")) oneself.isTouchingChar = true;

        }

        
    }

    private void OnTriggerExit(Collider other)
    {
        //enemy stops touching character
        Debug.Log("stop touching char");
        if (other.gameObject.layer == LayerMask.NameToLayer("Main character")) oneself.isTouchingChar = false;

        Debug.Log(other.gameObject.layer == LayerMask.NameToLayer("Main character"));

        if (oneself.isAlly && other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            Debug.Log("Stopped touching enemy");
            oneself.isAllyTouchingEnemy = false;
            
        }
        
    }


}
