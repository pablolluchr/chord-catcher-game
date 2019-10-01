using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask enemiesMask;
    public float rotationSpeed;
    public Transform fireTransform;
    public float detectionDistance;
    public Rigidbody projectile;
    public float launchForce;
    public float destroyProjectileDelay;
    [HideInInspector]
    public bool canFire;
    protected float attackSpeed;
    public int damage;

    private void Start()
    {
        canFire = false;
        GetComponent<ITarget>().SetTarget(null);
        attackSpeed = GetComponent<Player>().attackSpeed;
        InvokeRepeating("FindEnemyAndFire", 0f, 1f / attackSpeed);
        //InvokeRepeating("CheckAttackSpeedChange", 0, 0.2f); //check if the attack speed changed every .2 seconds to update the shooting frequency

        //ignore collisions between projectiles and projectiles, and player and projectiles
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
        if (GetComponent<ITarget>().GetTarget() == null) return;
        var lookPos = GetComponent<ITarget>().GetTarget().GetComponent<Transform>().position - transform.position;
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
        GetComponent<ITarget>().SetTarget(null);

        //Colliders of all the enemies within range
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionDistance, enemiesMask);
        //stop fire if there are no enemies
        // Go through all the colliders to find the nearest one. 
        float closestDistance = float.MaxValue;
        for (int i = 0; i < colliders.Length; i++)
        {
            // ... and find their rigidbody.
            Transform targetTransform = colliders[i].GetComponent<Transform>();

            // If they don't have a rigidbody, go on to the next collider.
            if (!targetTransform)
                continue;

            //calculate distance
            float dist = Vector3.Distance(targetTransform.position, transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                GetComponent<ITarget>().SetTarget(colliders[i].gameObject);
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
    


    public void Fire()
    {
        if (GetComponent<ITarget>().GetTarget() == null) return;
     
        // Create an instance of the projectile and store a reference to it's rigidbody.
        Rigidbody shellInstance = Instantiate(projectile, fireTransform.position, fireTransform.rotation) as Rigidbody;
        shellInstance.GetComponent<Projectile>().damage = damage;
        // Set the shell's velocity to the launch force in the fire position's forward direction.
        Vector3 enemyPosition = GetComponent<ITarget>().GetTarget().GetComponent<Transform>().position;

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
