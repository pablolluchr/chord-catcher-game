using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    
    public int damage;
    private bool recentlyInflictedDamage; //has recently attacked the character
    public float timeBetweenDamage;
    public bool isTouchingChar;
    public bool isAllyTouchingEnemy;
    private GameObject character;
    public bool isAlly;
    public Enemy enemyOnCollision;
    // Start is called before the first frame update
    void Start()
    {
        isAlly = false;
        character = PlayerManager.instance.player;
        recentlyInflictedDamage = false;
        isTouchingChar = false;
    }

    void Update()
    {
        //TODO: Put all  the logic below in EnemyTrigger and change EnemyTrigger to EnemyAttack. Also move EnemyTrigger to top level -instead of being at the level of the collider
        //TODO: put this code into different file as Enemy should have code that works for any kind (this logic doesn't work for ranged enemies)
        //an individual enemy can't trigger damage more than once in timeBetweenDamage seconds
        if (!isAlly && !recentlyInflictedDamage && isTouchingChar)
        {
            recentlyInflictedDamage = true;
            GetComponent<EnemySoundController>().Play(1);
            character.GetComponent<Player>().TakeDamage(damage);
            Invoke("SetrecentlyInflictedDamageFalse", timeBetweenDamage);
        }
        if (!recentlyInflictedDamage && isAllyTouchingEnemy)
        {
            if (enemyOnCollision.lifeState != 0)
            {
                isAllyTouchingEnemy = false;
            }
            else
            {
            enemyOnCollision.TakeDamage(damage);
            recentlyInflictedDamage = true;
            Invoke("SetRecentlyInflictedDamageFalse", timeBetweenDamage);
            }
        }
        if (!recentlyInflictedDamage) //is attacking an ally
        {

        }
    }
    override public void OnDeath()
    {
        lifeState = 2;
        if (isAlly) Destroy(gameObject); //if ally dies then kill them off completely
        GetComponent<EnemyDeath>().Die();
    }

    private void SetRecentlyInflictedDamageFalse()
    {
        recentlyInflictedDamage = false;
    }



}
