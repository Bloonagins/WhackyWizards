/**********************************************
| FireBall V1.0.0                             |
| Author: Zach Heimbigner, T3                 |
| Description: This program manages the       |              
| fireball spell and is attached to a         |
| prefab, is is instatianted by player        |
| Bugs:                                       |
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall: Spells
{   
    //stores the value of the players position/rotation
    private Transform firePoint;
    //the desired prefab to cast
    public GameObject projectile;
    public bool isTriggeredBlast = false;

    public FireBall()
    {
        speed = 20.0f;
        DAMAGE = 70;
        COOL_DOWN = 0.75f;
    }

    //point in the direction of the player and fire
    void Awake()
    {
        var player = GameObject.FindWithTag("Player");
        firePoint = player.transform; //get player position/rotation
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * this.speed, ForceMode2D.Impulse);
    }

    public int getSpellDamage()
    {
        return DAMAGE;
    }

    //detect collision between anything that is collidable
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag !="Player" && collision.gameObject.tag != "Spell")
        {
            if(collision.gameObject.tag == "Enemy")
            {
                isTriggered = true;
            }
            Debug.Log(DAMAGE);
            Destroy(projectile);
        }
    }
}
