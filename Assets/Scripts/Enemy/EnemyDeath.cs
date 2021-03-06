﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public Canvas deathCanvas;
    public Enemy self;
    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Enemy>();
        deathCanvas.worldCamera = FindObjectOfType<Camera>();
        deathCanvas.gameObject.SetActive(false);
    }

    public void Die()
    {
        self.lifeState = 2;
        self.animationState = 0;
        if (self.isAlly) Destroy(gameObject); //if ally dies then kill them off completely

        GetComponent<Enemy>().Idle();
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        //show collect/recruit options
        deathCanvas.gameObject.SetActive(true);

        GetComponent<EnemyMovement>().enabled = false;
        GetComponent<Enemy>().enabled = false;
        GetComponent<EnemyAttack>().enabled = false;


        //TODO: Do something better than just deactivating the capsule collider lol
        GetComponent<CapsuleCollider>().enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Limbo");

        //play death animation --or just overlay something to indicate it died


    }

    public void Harvest()
    {
        GameManager.instance.HarvestNotes(1);
        GetComponent<Enemy>().lifeState = 1;
        deathCanvas.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void Recruit()
    {
        gameObject.layer = LayerMask.NameToLayer("Allies");
        deathCanvas.gameObject.SetActive(false);
        GetComponent<EnemyAttack>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<EnemyMovement>().enabled = true; 
        GetComponent<Enemy>().enabled = true;//runs OnEnable() so it resets health and lifeState
        GetComponent<Enemy>().isAlly = true;
        GetComponent<EnemyMovement>().lookRadius = 100;
        GetComponent<Enemy>().healthBar.color = PlayerManager.instance.player.GetComponent<Unit>().healthBar.color;
    }

}
