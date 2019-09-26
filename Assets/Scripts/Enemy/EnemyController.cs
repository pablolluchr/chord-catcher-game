using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int damage;
    public bool isDead; //TODO: Instead of disabling scripts use this var to do stuff
    private bool recentlyHurt; //has recently attacked the character
    public float timeBetweenDamage;
    public bool isTouchingChar;
    public bool isAllyTouchingEnemy;
    private GameObject character;
    public bool isAlly;
    // Start is called before the first frame update
    void Start()
    {
        isAlly = false;
        character = PlayerManager.instance.player;
        recentlyHurt = false;
        isTouchingChar = false;
    }

    void Update()
    {
        //an individual enemy can't trigger damage more than once in timeBetweenDamage seconds
        if (!recentlyHurt && isTouchingChar)
        {
            recentlyHurt = true;
            GetComponent<EnemySoundController>().Play(1);
            CharacterHealth health = character.GetComponent<CharacterHealth>();
            health.TakeDamage(damage);
            Invoke("SetRecentlyHurtFalse", timeBetweenDamage);
        }
        if (!recentlyHurt && isAllyTouchingEnemy)
        {
            recentlyHurt = true;
            EnemyHealth health = GetComponentInChildren<EnemyTrigger>().lastEnemyCollision;
            //TODO: Make health a base class and extend it too enemy health and player health. Also rename it to lifeStat or something
            health.TakeDamage(damage);
            Invoke("SetRecentlyHurtFalse", timeBetweenDamage);

        }
    }

    private void SetRecentlyHurtFalse()
    {
        recentlyHurt = false;
    }

}
