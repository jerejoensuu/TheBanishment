using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maximumHealth = 100f;
    private float damageTaken = 0f;
    public bool dead = false;

    public float regenRate = 1f;
    public float regenDelay = 2f;
    private bool regenEnabled = true;

    void Update()
    {
        if (!dead && regenEnabled && damageTaken > 0f)
        {
            damageTaken -= Time.deltaTime * regenRate;
        }
    }

    public void TakeDamage(float amount)
    {
        damageTaken += amount;

        if (damageTaken > maximumHealth)
        {
            Death();
        }
        else
        {
            StartCoroutine(HealthRegeneration());
        }
    }

    private IEnumerator HealthRegeneration()
    {
        regenEnabled = false;

        yield return new WaitForSeconds(regenDelay);

        regenEnabled = true;
    }

    private void Death()
    {
        Debug.Log("Dead");
        dead = true;
    }
}
