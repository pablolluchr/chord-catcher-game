using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    
    
    public bool isTouchingPlayer;
    public bool isTouchingEnemy;
    public bool isTouchingAlly;
    public bool isAlly;
    public bool isTouchingPlayerIndirectly; //is touching player through a chain of allies?
    public GameObject target; 

    public GameObject player;
    public Enemy enemyOnCollision;
    // Start is called before the first frame update
    private void Update()
    {
        if (isAlly)
        {
            if (player.GetComponent<PlayerAttack>().lockedEnemy != null)
            {
                target = player.GetComponent<PlayerAttack>().lockedEnemy;
            }
            else
            {
                target = player;
            }
        }
    }
    void Start()
    {
        isAlly = false;
        player = PlayerManager.instance.player;
        target = player;
        isTouchingPlayer = false;
    }
    
    override public void OnDeath()
    {
        lifeState = 2;
        if (isAlly) Destroy(gameObject); //if ally dies then kill them off completely
        GetComponent<EnemyDeath>().Die();
    }


    

    



}
