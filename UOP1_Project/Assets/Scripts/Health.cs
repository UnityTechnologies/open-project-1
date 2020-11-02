using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    private float tempHealth;
    private bool isDied = false;

    void Start()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(float damage)
    {
        tempHealth = currentHealth - damage;
        if (tempHealth > 0)
        {
            Debug.Log("[Health]: " + gameObject.name + " took " + damage + " damage.", gameObject);
            Debug.Log("[Health]: "+ gameObject.name + " health is " + tempHealth, gameObject);
            isDied = false;
        }
        else if (tempHealth == 0)
        {
            Debug.Log("[Health]: " + gameObject.name + " took " + damage + " damage.", gameObject);
            Debug.Log("[Health]: "+ gameObject.name + " health is " + tempHealth, gameObject);
            isDied = true;
        } else {
            Debug.Log("[Health]: " + gameObject.name + " took " + damage + " damage.", gameObject);
            Debug.Log("[Health]: "+ gameObject.name + " health is " + tempHealth, gameObject);
            isDied = true;
        }

        if (isDied != true)
        {
            currentHealth = tempHealth;
            tempHealth = 0;
        } else {
            Debug.Log("[Health]: " + gameObject.name + " Died.", gameObject);
            currentHealth = tempHealth;
        }
    }
}
