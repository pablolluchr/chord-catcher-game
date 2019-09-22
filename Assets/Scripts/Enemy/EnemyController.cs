using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int damage;
    private bool recentlyHurt; //has recently attacked the character
    public float timeBetweenDamage;
    public bool isTouchingChar;
    public GameObject character;
    // Start is called before the first frame update
    void Start()
    {
        recentlyHurt = false;
        isTouchingChar = false;
    }

    void Update()
    {
        //an individual enemy can't trigger damage more than once in timeBetweenDamage seconds
        if (!recentlyHurt && isTouchingChar)
        {
            recentlyHurt = true;

            CharacterHealth health = character.GetComponent<CharacterHealth>();
            health.TakeDamage(damage);
            Invoke("SetRecentlyHurtFalse", timeBetweenDamage);
        }
    }

    private void SetRecentlyHurtFalse()
    {
        recentlyHurt = false;
    }

}
