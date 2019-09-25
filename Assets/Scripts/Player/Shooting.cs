using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{



    public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
    public int m_detectionRadius;
    public LayerMask m_enemiesMask;
    private Collider m_lockedEnemy;
    private float m_minDistance;
    public Rigidbody m_projectile;
    public float m_LaunchForce;
    public float m_destroyProjectileDelay;
    [HideInInspector]
    public bool m_canFire;
    private float attackSpeed;
    public CharacterControllerScript controllerScript;
    public float rotationSpeed;

    private void Start()
    {
        attackSpeed = controllerScript.attackSpeed;
        m_canFire = false;
        InvokeRepeating("Fire", 0f, 1f / attackSpeed);
        InvokeRepeating("CheckAttackSpeedChange", 0, 0.2f); //check if the attack speed changed every .2 seconds to update the shooting frequency

        //ignore collisions between projectiles and projectiles, and player and projectiles
        Physics.IgnoreLayerCollision(10, 10);
        Physics.IgnoreLayerCollision(8, 10);




    }
    private void CheckAttackSpeedChange()
    {
        //if attack speed is changed: change how frequent Fire is invoked
        if (!IsEqual(attackSpeed, controllerScript.attackSpeed))
        {
            attackSpeed = controllerScript.attackSpeed;
            CancelInvoke("Fire");
            InvokeRepeating("Fire", 0f, 1f / attackSpeed);

        }
    }

    //check equality between fl oas (up to epsilon)
     private bool IsEqual(float a, float b)
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_detectionRadius);
    }


    public void LookAtEnemy()
    {
            m_canFire = true;

            //face enemy on the y axis. 

            var lookPos = m_lockedEnemy.GetComponent<Transform>().position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.time * rotationSpeed);
    }


    public bool FindNearestEnemy()
    {
        //Colliders of all the enemies within range
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_detectionRadius, m_enemiesMask);
        //stop fire if there are no enemies
        // Go through all the colliders to find the nearest one. 
        m_minDistance = float.MaxValue;
        for (int i = 0; i < colliders.Length; i++)
        {
            // ... and find their rigidbody.
            Transform targetTransform = colliders[i].GetComponent<Transform>();

            // If they don't have a rigidbody, go on to the next collider.
            if (!targetTransform)
                continue;

            //calculate distance
            float dist = Vector3.Distance(targetTransform.position, transform.position);
            if (dist < m_minDistance)
            {
                m_minDistance = dist;
                m_lockedEnemy = targetTransform.GetComponent<Collider>();
            }

        }


        if (colliders.Length < 1)
        {
            m_canFire = false;
            return false;
        }
        else
        {
            m_canFire = true;
            return true;
        }

    }

    private void Fire()
    {
        //stop function if firing disabled
        if (!m_canFire)
            return;

        //IN CASE ITS NOT FOUND YET
        FindNearestEnemy();

        // Create an instance of the projectile and store a reference to it's rigidbody.
        Rigidbody shellInstance = Instantiate(m_projectile, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        Vector3 enemyPosition = m_lockedEnemy.GetComponent<Transform>().position;

        // the y component should be 0 as every projectile should be launched parallel to the floor (enemies should be hit if projectile goes above or below them
        //... so trigger boxes should be made accordingly.

        Vector3 velocityDirection = new Vector3(enemyPosition.x - m_FireTransform.position.x, 0, enemyPosition.z - m_FireTransform.position.z).normalized;
        shellInstance.velocity = velocityDirection * m_LaunchForce;

        Destroy(shellInstance.gameObject, m_destroyProjectileDelay);


    }

}
