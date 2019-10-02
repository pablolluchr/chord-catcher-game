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

    void OnEnable()
    {
        self = GetComponent<Enemy>();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    //TODO: only use interfaces when needed. Between classes of the same object self.XXX is enough
    void Update()
    {
        GameObject target = GetComponent<ITarget>().GetTarget();
        if (recentlyInflictedDamage) return;
        if (target == null) return;
        if (self.isAlly && target.layer == LayerMask.NameToLayer("Player")) {
            self.isAttacking = false;
            return; //allies shouldn't damage player
        }

        float dist = Vector3.Distance(transform.position, target.transform.position); //distance between self and target

        if (dist < attackRange)
        {
            Debug.Log("Attack player");
            self.isAttacking = true;
            target.GetComponent<ITakeDamage>().TakeDamage(damage);
            recentlyInflictedDamage = true;
            Invoke("SetRecentlyInflictedDamageFalse", timeBetweenDamage);
        }
        else
        {
            self.isAttacking = false;
        }
    }

    private void SetRecentlyInflictedDamageFalse()
    {
        recentlyInflictedDamage = false;
    }
}
