using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{

    public int m_MaxHealth;
    private int m_CurrentHealth;
    private bool m_Dead;
    public Image m_HealthBar;

    private void OnEnable()
    {
        // When the tank is enabled, reset the tank's health and whether or not it's dead.
        m_CurrentHealth = m_MaxHealth;
        m_Dead = false;

        // Update the health slider's value and color.
        SetHealthUI();
    }

    void SetHealthUI()
    {
        float currentHealthPct = (float)m_CurrentHealth / m_MaxHealth;
        m_HealthBar.fillAmount = currentHealthPct;
    }

    public void TakeDamage(int amount)
    {
        // Reduce current health by the amount of damage done.
        m_CurrentHealth -= amount;

        // Change the UI elements appropriately.
        SetHealthUI();  

        // If the current health is at or below zero and it has not yet been registered, call OnDeath.
        if (m_CurrentHealth <= 0f && !m_Dead)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        //die
        Destroy(gameObject);

    }

}
