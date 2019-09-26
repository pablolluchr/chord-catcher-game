using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public Transform fireTransform;
    public float detectionDistance;
    public Collider lockedEnemy;
    public Rigidbody projectile;
    public float launchForce;
    public float destroyProjectileDelay;
    [HideInInspector]
    public bool canFire;
    public float attackSpeed;


    public void Fire()
    {
        if (lockedEnemy == null) return;
        // Create an instance of the projectile and store a reference to it's rigidbody.
        Rigidbody shellInstance = Instantiate(projectile, fireTransform.position, fireTransform.rotation) as Rigidbody;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        Vector3 enemyPosition = lockedEnemy.GetComponent<Transform>().position;

        // the y component should be 0 as every projectile should be launched parallel to the floor (enemies should be hit if projectile goes above or below them
        //... so trigger boxes should be made accordingly.

        Vector3 velocityDirection = new Vector3(enemyPosition.x - fireTransform.position.x, 0, enemyPosition.z - fireTransform.position.z).normalized;
        shellInstance.velocity = velocityDirection * launchForce;

        Destroy(shellInstance.gameObject, destroyProjectileDelay);


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
    }

    //check equality between floats (up to epsilon)
    protected bool IsEqual(float a, float b)
    {
        if (a >= b - Mathf.Epsilon && a <= b + Mathf.Epsilon)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
