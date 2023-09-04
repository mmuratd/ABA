using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HitboxAttack : NetworkBehaviour 
{

    [SerializeField] private int attackPower = 1;

    private bool canAttack;

    private void OnEnable()
    {
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var hit = other.GetComponent<IDamageable>();
        Debug.Log("burayagirdi ");
        if (hit != null && canAttack)
        {
            hit.Damage(attackPower);
            canAttack = false;
        }
    }
}
