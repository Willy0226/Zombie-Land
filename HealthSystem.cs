using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float healthAmount = 100f;
    public GameObject onDeath;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (healthAmount <= 0)
        {
            Die();
        }
    }

    //-------------- which heal do we want? self regeneration or collect first aid -----------------//
    public void Regeneration()
    {
        // if not being detecting by zombie more than 10 sec then self-cure (+1 health per second)
    }

    public void GetFirstAid()
    {
        // if get first aid then + 20 health
    }

    public void DamageToPlayer(float damage)
    {
        healthAmount = healthAmount - damage;
    }

    private void Die()
    {
        onDeath.SetActive(true);
    }
}
