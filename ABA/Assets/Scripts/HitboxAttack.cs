using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HitboxAttack : NetworkBehaviour 
{
    
    [SerializeField] private int attackPower = 1;

    private bool canAttack;

    public void OnHit()
    {

    }
    private void OnEnable()
    {
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var hit = other.GetComponent<IDamageable>();
        
        if (hit != null )
        {

            hit.Damage(attackPower);
            
        }
    }

}
