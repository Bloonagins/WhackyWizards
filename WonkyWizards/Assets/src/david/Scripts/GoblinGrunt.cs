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
    private Rigidbody2D rb;
    // Constructor for GoblinGrunt
    public GoblinGrunt()
    {
        max_health = 200;
        health = 200;
        damage = 10;
        move_speed = 5;
        attack_speed = 1.5f;
        knock_back = 300f;
    }

    // bool canAttack()
    // if (collision)
    //      canAttack = false;
    //      add timer*attackspeed;
    // canAttack = true

    // Method to update when enemy is dealt damage
    void UpdateHealth(int damage_recieved)
    {
        health = health - damage_recieved;
    }
    // Methods for retrieving stats
    public int GetMaxHealth()
    {
        return max_health;
    }
    public int GetHealth()
    {
        return health;
    }
    public int GetDamage()
    {
        return damage;
    }
    public override float GetMoveSpeed()
    {
        return move_speed;
    }
    public float GetAttackSpeed()
    {
        return attack_speed;
    }

    public float GetKnockBack()
    {
        return knock_back;
    }
        
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        GameObject other = collision.gameObject;
        if(collision.gameObject.tag == "Spell") // Check if enemy collided with spell
        {   if(other.GetComponent<FireBall>()) { // Check if spell was Fireball
                UpdateHealth(other.GetComponent<FireBall>().getSpellDamage()); // Recieve damage 
                rb.AddForce((other.transform.position - transform.position) * 200f * -1.0f, ForceMode2D.Impulse); // FireBall.getKnockback();
            }
        }
    }
}
