using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Enemy self;
    public int damage;
    public bool recentlyInflictedDamage; //has recently attacked the character
    public float timeBetweenDamage;
    public float attackRange;
    // Start is called before the first frame update

    void Start()
    {
        self = GetComponent<Enemy>();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    //public abstract void Attack();
    void Update()
    {
        if (recentlyInflictedDamage) return;
        if (self.target == null) return;
        if (self.isAlly && self.target.layer == LayerMask.NameToLayer("Player")) return; //allies shouldn't damage player

        float dist = Vector3.Distance(transform.position, self.target.transform.position); //distance between self and target

        if (dist < attackRange)
        {
            if (self.target.GetComponent<Player>() != null)
            {
                self.target.GetComponent<Player>().TakeDamage(damage);
                GetComponent<EnemySoundController>().Play(1);

            }
            else
            {
                self.target.GetComponent<Enemy>().TakeDamage(damage);
            }
            recentlyInflictedDamage = true;
            Invoke("SetRecentlyInflictedDamageFalse", timeBetweenDamage);
        }
    }

    private void SetRecentlyInflictedDamageFalse()
    {
        recentlyInflictedDamage = false;
    }
}
