using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critters : EnemyBase
{
    public Transform homePos;
    public string playerTag = "Player";
    
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log(other.name + " entered", gameObject);
            transform.position = Vector3.MoveTowards(transform.position, other.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log(other.name + " leaved", gameObject);
            transform.position = Vector3.MoveTowards(transform.position, homePos.position, moveSpeed * Time.deltaTime);
        }
    }

    public void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            Debug.Log(other.gameObject.name + "Collided", gameObject);
            Health playerHealth = other.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                StartCoroutine( DoAttack(playerHealth) );
            }
        }
    }

    public void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            StopAllCoroutines();
        }
    }

    IEnumerator DoAttack(Health playerHealth)
    {
        playerHealth.TakeDamage(attackAmount);
        yield return new WaitForSecondsRealtime(3f);

    }
}
