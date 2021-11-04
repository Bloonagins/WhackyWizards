/**********************************************
| GoblinAssasin V1.0.0                        |
| Author: David Bush, T5                      |
| Description: This is the GoblinAssasin      |
| class that inherits from the Enemy          |
| superclass. This will contain all variables |
| and methods associtiated with the           |
| GoblinAssasin enemy type. Each GoblinAssasin| 
| has the unique dash ability. When it is     | 
| within a certain range from the player the  |
| Assasain dashes forward towards the player. |
| Bugs:                                       |
**********************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GoblinAssassin : Enemy
{
    // Used to store RigidBody2d Component
    private Rigidbody2D rb;
    // Used to store Agent component
    private NavMeshAgent agent;
    // The distance the GoblinAssasin needs to be in order to dash
    private float dashDistance;
    // The dash amount applied to the unit
    private float dashAmount;
    // The dash cool down
    private float dashCD;
    // The dash cool down timer
    private float dashTimer;

    // Constructor for GoblinAssasin
    public GoblinAssassin()
    {
        max_health = health = 325;
        damage = 60;
        move_speed = 20f;
        lowest_speed = 10f;
        highest_speed = 22f;
        attack_speed = attackTimer = 1.5f; // 2400 damage per minute
        dashDistance = 8f;
        dashAmount = 130f;
        dashCD = dashTimer = 3f;
        targetDistance = 25f;
        attackConnected = false;
        knock_back = 350f;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Gets the Rigid Body component
        rb = GetComponent<Rigidbody2D>();
        // Gets the Agent component
        agent = GetComponent<NavMeshAgent>();
    }
    // Called at a fixed interval (50 times / second)
    // Increments the timers if they're on a cooldown
    void FixedUpdate()
    {
        // Check timers, and increment time
        if (attackTimer <= attack_speed) { 
            attackTimer += Time.fixedDeltaTime;
        }
        if (dashTimer <= dashCD) {
            dashTimer += Time.fixedDeltaTime;
        }
    }
    // Update is called once per frame
    void Update()
    {
        // Check if unit has no health left
        if (health <= 0) {
            // SoundManagerScript.PlaySound("enemyDeath");
            Destroy(gameObject); // Destroy unit
        }
    }
    
    // Function that checks for collisions
    void OnTriggerEnter2D(Collider2D collision)
    {        
        GameObject other = collision.gameObject;
       
        if(other.tag == "Spell") { // Check if enemy collided with spell

            if(other.GetComponent<FireBall>()) { // Check if spell was Fireball
                RecieveDamage(other.GetComponent<FireBall>().getSpellDamage()); // Recieve damage 
                rb.AddForce((other.transform.position - transform.position) * other.GetComponent<FireBall>().getSpellKnockBack() * -1.0f, ForceMode2D.Impulse); // FireBall.getKnockback();
            }
            else if(other.GetComponent<MagicMissle>()) { // Check if spell was MagicMissle
                //Debug.Log("Collided Magic Missle");
                RecieveDamage(other.GetComponent<MagicMissle>().getSpellDamage()); // Recieve damage 
                rb.AddForce((other.transform.position - transform.position) * other.GetComponent<MagicMissle>().getSpellKnockBack() * -1.0f, ForceMode2D.Impulse); // FireBall.getKnockback();
            }
            else if(other.GetComponent<IceBeam>()) { // Check if spell was IceBeam
                //Debug.Log("Collided Ice Beam");
                ChangeMoveSpeed(other.GetComponent<IceBeam>().Freeze() * -1); // Recieve slow 
            }
            else if(other.GetComponent<AcidSpray>()) { // Check if spell was AcidSpray
                //Debug.Log("Collided Acid Spray");
                RecieveDamage(other.GetComponent<AcidSpray>().getSpellDamage()); // Recieve damage 
                rb.AddForce((other.transform.position - transform.position) * other.GetComponent<AcidSpray>().getSpellKnockBack() * -1.0f, ForceMode2D.Impulse); // FireBall.getKnockback();
            }
        }
        /*else if(other.tag == "SummonProjectile") {
            if(other.GetComponent<Sword>()) { // Check if collided with Sword
                RecieveDamage(other.GetComponent<Sword>().getProjDamage()); // Recieve damage
                //rb.AddForce((other.transform.position - transform.position) * other.GetComponent<Sword>().getProjKnockback() * -1.0f, ForceMode2D.Impulse); // Apply Knockback;
            }
            else if(other.GetComponent<DragonProj>()) { // Check if collided with Dragon projectile
                Debug.Log("COLLIDED Dragon");
                RecieveDamage(other.GetComponent<DragonProj>().getProjDamage()); // Recieve damage
            }
            else if(other.GetComponent<CrossbowProj>()) { // Check if collided with Crossbow projectile
                Debug.Log("COLLIDED Crosbow");
                RecieveDamage(other.GetComponent<CrossbowProj>().getProjDamage()); // Recieve damage
            }
            else if(other.GetComponent<ShrubberyProj>()) { // Check if collided with Shrubbery projectile
                Debug.Log("COLLIDED Shrubbery");
                RecieveDamage(other.GetComponent<ShrubberyProj>().getProjDamage()); // Recieve damage
            }
            else if(other.GetComponent<SvenProj>()) { // Check if collided with Sven projectile
                Debug.Log("COLLIDED Sven");
                RecieveDamage(other.GetComponent<SvenProj>().getProjDamage()); // Recieve damage
            }
            else if(other.GetComponent<ChunkProj>()) { // Check if collided with Chunk projectile
                Debug.Log("COLLIDED Chunk");
                RecieveDamage(other.GetComponent<ChunkProj>().getProjDamage()); // Recieve damage
            }
        }*/

    }
    void OnTriggerStay2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if(other.tag == "Goal") { // Checks if collided with Goal
            //rb.AddForce((other.transform.position - transform.position) * 50f * -1.0f, ForceMode2D.Impulse);
            if (attackConnected) { // Make sure attack is available and attack is successful
                attackTimer = 0.0f; // Reset timer
                attackConnected = false; // Reset attack 
                //Debug.Log("Attack");
            }
        }
    }


    //
    public void ApplyDash(Vector3 player_position) {
        dashTimer = 0f;
        rb.AddForce((player_position - transform.position) * dashAmount , ForceMode2D.Impulse);
    }
    //
    public bool canDash() {
        return dashTimer >= dashCD;
    }
    // Keeps track of when enemy can attack
    public bool canAttack()
    {
        return attackTimer >= attack_speed;
    }

    // Method to update health when enemy is dealt damage
    public void RecieveDamage(int damage_recieved)
    {
        health -= damage_recieved; // take away health from eneny

        if(health < 0) { // Check if health is below 0
            health = 0; // set to 0
        }

    }
    // Method that gives health to enemy
    public void AddHealth(int health_recieved)
    {
        health += health_recieved; // add health to enemy

        if(health > max_health) { // Check if health is above max
            health = max_health; // set to max
        }
    }
    // Function to confirm attack was sucessful
    public void SetAttack(bool success)
    {
        attackConnected = success;
    }
    // Function to return current position of GoblinGrunt unit
    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }
    // Function to change the enemy's damage by a flat amount
    public void ChangeDamage(int damage_amount){
        damage += damage_amount; // can be positive or negative
    }
    // Function to change the enemy's movespeed by a flat amount
    public void ChangeMoveSpeed(float speed_amount) {
        if (agent.speed >= lowest_speed && speed_amount< 0) { 
            agent.speed += speed_amount;
            agent.acceleration += speed_amount;
        }
        else if (agent.speed <= highest_speed && speed_amount > 0) {
            agent.speed += speed_amount;
            agent.acceleration += speed_amount;
        }
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
    public float GetAttackTimer()
    {
        return attackTimer;
    }
    public float GetDashTimer()
    {
        return dashTimer;
    }
    public float GetDashDistance() {
        return dashDistance;
    }
    public float GetTargetDistance() 
    {
        return targetDistance;
    }
}
