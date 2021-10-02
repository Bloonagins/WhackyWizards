/**********************************************
| GoblinGrunt V1.0.0                          |
| Author: David Bush, T5                      |
| Description: This is the GoblinGrunt class  |
| that inherits from the Enemy superclass.    |
| This will contain all variables and methods |
| associtiated with the GoblinGrunt enemy     |
| type. This also updates the health for each |
| GoblinGrunt and removes the object if health|
| is zero.                                    |
| Bugs:                                       |
**********************************************/
// To get Enemies damage
//    GoblinGrunt goblinGrunt;
//    goblinGrunt = FindObjectOfType<GoblinGrunt>();
//    goblinGrunt.GetDamage();

using System.Collections;
using UnityEngine;

public class GoblinGrunt : Enemy
{
    // Constructor for GoblinGrunt
    public GoblinGrunt()
    {
        health = 200;
        damage = 10;
        move_speed = 5;
        attack_speed = 1.5f;
    }

    // Method to update when enemy is dealt damage
    void UpdateHealth(int damage_recieved)
    {
        health = health - damage_recieved;
    }

    // Methods for retrieving stats
    public int GetHealth()
    {
        return health;
    }
    public int GetDamage()
    {
        return damage;
    }
    public float GetMoveSpeed()
    {
        return move_speed;
    }
    public float GetAttackSpeed()
    {
        return attack_speed;
    }
    
    // Update is called once per frame
    void Update()
    {
        // Check if unit has no health left
        if (health <= 0) {
            Destroy(gameObject); // Destroy unit
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if enemy hit with spell
        if(collision.gameObject.tag == "Spell")
        {   
            UpdateHealth(5); // Recieve damage
        }
        //if(collision.gameObject.tag == "Summon")
        //{
        //}
    }
}
