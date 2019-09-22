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
    public bool m_canFire;
    public float m_attackSpeed;
    public float rotationSpeed;

    private void Start()
    {
        m_canFire = false;
        InvokeRepeating("Fire", 0f, m_attackSpeed);

        //ignore collisions between projectiles and projectiles, and player and projectiles
        Physics.IgnoreLayerCollision(10, 10);
        Physics.IgnoreLayerCollision(8, 10);



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
        shellInstance.velocity = (m_lockedEnemy.GetComponent<Transform>().position - m_FireTransform.position).normalized * m_LaunchForce;

        Destroy(shellInstance.gameObject, m_destroyProjectileDelay);


    }

}
