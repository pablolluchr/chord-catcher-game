using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{

    public EnemyController enemyController;
    public EnemyHealth lastEnemyCollision;
    private void OnTriggerEnter(Collider other)
    {
        //check if it's an ally and it's attacking another enemy
        if (enemyController.isAlly)
        {
            enemyController.isAllyTouchingEnemy |= other.gameObject.layer == LayerMask.NameToLayer("Enemies");
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            {
                lastEnemyCollision = other.GetComponentInParent<EnemyHealth>();
            }

        }
        else
        {
            //character touches enemy
            enemyController.isTouchingChar |= other.gameObject.layer == LayerMask.NameToLayer("Main character");

        }

        
    }

    private void OnTriggerExit(Collider other)
    {
        //character stops touching enemy
        enemyController.isTouchingChar &= other.gameObject.layer != LayerMask.NameToLayer("Main character");
    }


}
