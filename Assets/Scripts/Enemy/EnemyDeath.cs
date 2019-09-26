using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public Canvas deathCanvas;
    // Start is called before the first frame update
    void Start()
    {

        deathCanvas.worldCamera = FindObjectOfType<Camera>();
        deathCanvas.gameObject.SetActive(false);
    }

    public void Die()
    {
        //show collect/recruit options
        Debug.Log("Dead");
        deathCanvas.gameObject.SetActive(true);

        GetComponent<EnemyMovement>().enabled = false;
        GetComponent<EnemyController>().enabled = false;

        //TODO: Do something better than just deactivating the capsule collider lol
        GetComponent<CapsuleCollider>().enabled = false;
        gameObject.layer = LayerMask.NameToLayer("DeadEnemies");

        //play death animation --or just overlay something to indicate it died


    }

    public void Harvest()
    {
        GameManager.instance.HarvestNotes(1);
        deathCanvas.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void Recruit()
    {
        deathCanvas.gameObject.SetActive(false);

        GetComponent<EnemyController>().isAlly = true;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<EnemyMovement>().enabled = true;
        GetComponent<EnemyController>().enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Allies");




    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //public void ButtonPressed()
    //{
    //    GameManager.instance.PressButton();
    //}
    //public void ButtonReleased()
    //{
    //    GameManager.instance.ReleaseButton();
    //}
}
