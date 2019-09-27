using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Unit : MonoBehaviour
{
    public int animationState;//0: idle, 1: running, 2: attacking
    protected Animator anim;
    public float animationLength;
    // Use this for initialization
    public int maxHealth;
    public int currentHealth;
    public int lifeState; //0:alive, 1:dead, 2: limbo
    public Image healthBar;

    private void OnEnable()
    {
        currentHealth = maxHealth;
        lifeState = 0;

        // Update the health slider's value and color.
        SetHealthUI();
    }

    void SetHealthUI()
    {
        float currentHealthPct = (float)currentHealth / maxHealth;
        healthBar.fillAmount = currentHealthPct;
    }

    public void TakeDamage(int amount)
    {
        // Reduce current health by the amount of damage done.
        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);
        // Change the UI elements appropriately.
        SetHealthUI();

        // If the current health is at or below zero and it has not yet been registered, call OnDeath.
        if (currentHealth <= 0f && lifeState==0)
        {
            OnDeath();
        }
    }

    public void Idle()
    {
        anim.CrossFade("Idle", 0.1f);
        anim.speed = 1f;
        animationState = 0;
    }

    public void Run()
    {
        anim.CrossFade("Run", 0.1f);
        anim.speed = 1f;
        animationState = 1;
    }

    public void Attack(float attackSpeed)
    {
        anim.CrossFade("Attack", 0.1f);
        anim.speed = attackSpeed / animationLength;
        animationState = 2;
    }

    public virtual void OnDeath() { }  //potentially to be extended by subclass
   

}
