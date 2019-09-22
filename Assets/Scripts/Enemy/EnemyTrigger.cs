using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{

    public EnemyController enemyController;

    private void OnTriggerEnter(Collider other)
    {
        //character touches enemy
        enemyController.isTouchingChar |= other.gameObject.layer == LayerMask.NameToLayer("Main character");
    }

    private void OnTriggerExit(Collider other)
    {
        //character stops touching enemy
        enemyController.isTouchingChar &= other.gameObject.layer != LayerMask.NameToLayer("Main character");
    }


}
