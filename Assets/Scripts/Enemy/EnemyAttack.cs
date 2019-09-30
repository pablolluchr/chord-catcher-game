using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Enemy self;
    public int damage;
    public bool recentlyInflictedDamage; //has recently attacked the character
    public float timeBetweenDamage;
    // Start is called before the first frame update

    void Start()
    {
        self = GetComponent<Enemy>();
    }

    void Update()
    {
        if (recentlyInflictedDamage) return;
        

        if (self.isAlly)
        {
            if (self.isTouchingEnemy)
            {
                Debug.Log("Ally attacking enemy");
                self.enemyOnCollision.TakeDamage(damage);
                recentlyInflictedDamage = true;
                Invoke("SetRecentlyInflictedDamageFalse", timeBetweenDamage);
            }
        }
        else
        {
            if (self.isTouchingPlayer)
            {
                Debug.Log("Enemy attacking player");
                GetComponent<EnemySoundController>().Play(1);
                self.player.GetComponent<Player>().TakeDamage(damage);
                recentlyInflictedDamage = true;
                Invoke("SetRecentlyInflictedDamageFalse", timeBetweenDamage);
            }
        }
    }

    private void SetRecentlyInflictedDamageFalse()
    {
        recentlyInflictedDamage = false;
    }
}
