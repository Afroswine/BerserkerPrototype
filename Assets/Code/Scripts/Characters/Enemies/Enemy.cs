using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    public event Action OnDamaged = delegate { };
    public event Action OnKilled = delegate { };

    public int Health => _health;

    [Header("Enemy")]
    [SerializeField] int _maxHealth = 1;
    int _health;

    private void Start()
    {
        _health = _maxHealth;
    }

    public void Damage(int amount)
    {
        amount = Mathf.Abs(amount);

        if (_health - amount <= 0)
        {
            _health = 0;
            Kill();
        }
        else
        {
            _health -= amount;
            OnDamaged?.Invoke();
        }
    }

    protected void Kill()
    {
        OnKilled?.Invoke();
        Debug.Log(gameObject.name + " has been killed.");
        Destroy(gameObject);
    }
}
