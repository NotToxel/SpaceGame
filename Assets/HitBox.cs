using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{

    public Health health;

    public void OnRaycastHit(PlayerController attack)
    {
        //health.TakeDamage(attack.attackDamage);

    }
    
   
}
