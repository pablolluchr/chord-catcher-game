using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public float knockoutTime = 0.2f;
    //TODO: constructor that when called new Projectile(position,damage,target) spawn

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {

            Debug.Log("Enemy hit");
            Unit enemy = collision.gameObject.GetComponent<Enemy>();
            EnemyMovement move = collision.gameObject.GetComponent<EnemyMovement>();
            move.StopMoving(knockoutTime);
            enemy.TakeDamage(damage);
            Destroy(gameObject);

        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player") ||
                collision.gameObject.layer == LayerMask.NameToLayer("Character projectile")) { }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
            Destroy(gameObject);
        else
            Destroy(gameObject);


    }

}
