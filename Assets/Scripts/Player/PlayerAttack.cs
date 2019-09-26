using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : RangedAttack
{
    public LayerMask enemiesMask;
    public Player player;
    public float rotationSpeed;

    private void Start()
    {
        canFire = false;
        lockedEnemy = null;
        player = GetComponent<Player>();
        attackSpeed = player.attackSpeed;
        InvokeRepeating("FindEnemyAndFire", 0f, 1f / attackSpeed);
        //InvokeRepeating("CheckAttackSpeedChange", 0, 0.2f); //check if the attack speed changed every .2 seconds to update the shooting frequency

        //ignore collisions between projectiles and projectiles, and player and projectiles
        attackSpeed = player.attackSpeed;
        Physics.IgnoreLayerCollision(10, 10);
        Physics.IgnoreLayerCollision(8, 10);
    }


    public void UpdateAttackSpeed(int newAttackSpeed)
    {
        attackSpeed = newAttackSpeed;
        CancelInvoke("FindEnemyAndFire");
        InvokeRepeating("FindEnemyAndFire", 0f, 1f / attackSpeed);
    }


    public void LookAtEnemy()
    {
        //face enemy on the y axis. 
        if (lockedEnemy == null) return;
        var lookPos = lockedEnemy.GetComponent<Transform>().position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.time * rotationSpeed);
    }

    public void FindEnemyAndFire()
    {
        if (!canFire) return;
        FindNearestEnemy();
        Fire();
    }

    //nearest enemy will be set to null if no alive enemy was found within range.
    public bool FindNearestEnemy()
    {
        lockedEnemy = null;
        //Colliders of all the enemies within range
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionDistance, enemiesMask);
        //stop fire if there are no enemies
        // Go through all the colliders to find the nearest one. 
        detectionDistance = float.MaxValue;
        for (int i = 0; i < colliders.Length; i++)
        {
            // ... and find their rigidbody.
            Transform targetTransform = colliders[i].GetComponent<Transform>();

            // If they don't have a rigidbody, go on to the next collider.
            if (!targetTransform)
                continue;

            //calculate distance
            float dist = Vector3.Distance(targetTransform.position, transform.position);
            if (dist < detectionDistance)
            {
                detectionDistance = dist;
                lockedEnemy = targetTransform.GetComponent<Collider>();
            }

        }


        if (colliders.Length < 1)
        {
            canFire = false;
            return false;
        }
        else
        {
            canFire = true;
            return true;
        }

    }

    

}
