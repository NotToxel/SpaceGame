// Inspired by kiwicoder ai playlist https://www.youtube.com/watch?v=TpQbqRNCgM0&list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Health health; // Reference to the Health component, to modify the health when hit

    // OnHit is called when this object is hit by an attack.
    // This method can be used to process damage or trigger effects on the health of the object.
    public void OnHit(PlayerController attack)
    {
        // This line is currently commented out, but if activated, it would apply damage from the attack to the health component.
        // health.TakeDamage(attack.attackDamage); 
    }
}
