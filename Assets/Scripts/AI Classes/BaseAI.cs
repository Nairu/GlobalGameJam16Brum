using UnityEngine;
using System.Collections;

public class BaseAI : MonoBehaviour {

    private int _health;
    public int Health
    {
        get { return Health; }
    }

    public void DealDamage(int damageToDeal)
    {
        _health -= damageToDeal;
        if (_health < 0)
            Die();
    }

    public void HealDamage(int amountToHeal)
    {
        _health += amountToHeal;
    }

    protected virtual void Attack()
    {

    }

    protected virtual void Die()
    {

    }


}
