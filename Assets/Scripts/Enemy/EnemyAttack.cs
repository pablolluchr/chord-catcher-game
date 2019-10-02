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
        GameObject target = GetComponent<ITarget>().GetTarget();
        if (recentlyInflictedDamage) return;
        if (target == null) return;
        if (self.isAlly && target.layer == LayerMask.NameToLayer("Player")) return; //allies shouldn't damage player

        float dist = Vector3.Distance(transform.position, target.transform.position); //distance between self and target

        if (dist < attackRange)
        {
            Debug.Log("Attack player");
            self.Attack(1);
            target.GetComponent<ITakeDamage>().TakeDamage(damage);
            //TODO: make player play sound when hit 
            recentlyInflictedDamage = true;
            Invoke("SetRecentlyInflictedDamageFalse", timeBetweenDamage);
        }
        else
        {
            if (self.animationState !=1)
            {
                self.Run();
            }
        }
    }

    private void SetRecentlyInflictedDamageFalse()
    {
        recentlyInflictedDamage = false;
    }
}
