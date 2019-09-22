using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int m_Damage;
    public float knockoutTime = 0.2f;
   

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {

            Debug.Log("Enemy hit");
            EnemyHealth health = collision.gameObject.GetComponent<EnemyHealth>();
            EnemyMovement move = collision.gameObject.GetComponent<EnemyMovement>();
            move.StopMoving(knockoutTime);
            health.TakeDamage(m_Damage);
            Destroy(gameObject);

        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Main character") ||
                collision.gameObject.layer == LayerMask.NameToLayer("Character projectile")) { }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
            Destroy(gameObject);
        else
            Destroy(gameObject);


    }

}
