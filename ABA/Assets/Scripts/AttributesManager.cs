using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributesManager : MonoBehaviour
{
    [SerializeField] public int health = 100;
    [SerializeField] public int attack = 10;
    // Start is called before the first frame update

    public void TakeDamage(int amount)
    {
        health -= amount;

    }

    public void DealDamage(GameObject target)
    {
         var atm = target.GetComponent<AttributesManager>();
        if (atm != null)
        {
            atm.TakeDamage(attack);
        }
    }


}
