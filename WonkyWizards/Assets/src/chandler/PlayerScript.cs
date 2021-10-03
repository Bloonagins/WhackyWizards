/*
 * PlayerScript.cs
 * This script contains all relevant information about the player that other people may need
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerScript : MonoBehaviour
{
    // cursor coordinates
    public static Vector3 screenCursorPoint;
    public static Vector3 worldCursorPoint;
    // angle between cursor and player
    public static float cursorAngle;
    // player's rigidbody component
    public Rigidbody2D rb;
    // speed of the player
    public float movementspeed;
    // player's health and mana point values
    private static int MAXHP = 1000;
    private static int hp;
    private static int mana = 0;
    // index number of which item is currently selected in the hotbar
    public static int spellIndex;
    public static int summonIndex;
    // determines whether the player places summons or casts spells with click
    public static bool inBuildMode;
    // keeps track of how long until the player is allowed to dash again
    public static float dashtimer;
    // temp variable that determines whether or not enemies are allowed to be spawned
    public static bool allowSpawn;

    // called when the game loads up
    void Awake()
    {
        // gets a link to this game object's rigid body component
        rb = GetComponent<Rigidbody2D>();
        // Confines the cursor to within the screen (NOTE: send this to Zach to put in GameManager)
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Start is called before the first frame update
    void Start()
    {
        resetPlayerHP();
        spellIndex = 0;
        summonIndex = 0;
    }

    // moves the camera to the middle of the screen when the game is alt-tabbed out
    void OnApplicationPause()
    {
        worldCursorPoint = transform.position;
        screenCursorPoint = Camera.main.WorldToScreenPoint(worldCursorPoint);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Enemy")
        {
            if (PlayerTimer.canDamage())
            {
                if (other.GetComponent<GoblinGrunt>())
                {
                    GoblinGrunt enemy = other.GetComponent<GoblinGrunt>();
                    damagePlayer(enemy.GetDamage());
                    rb.AddForce((other.transform.position - transform.position) * enemy.GetKnockBack() * -1.0f, ForceMode2D.Impulse);
                }
            }
        }
    }

    // Accessor functions

    // returns the player's max amoutn of HP
    public static int getMAXHP()
    {
        return MAXHP;
    }

    // returns the player's current HP
    public static int getHP()
    {
        return hp;
    }

    // returns the amount of mana the player currently has
    public static int getMana()
    {
        return mana;
    }

    // Mutator functions

    // sets the player's hp back to full
    public static void resetPlayerHP()
    {
        hp = MAXHP;
    }

    // decreases the player's health when damaged
    public static void damagePlayer(int damage)
    {
        hp -= damage;
        PlayerTimer.activateDamageCooldown();
    }

    // gives the player mana
    public static void giveMana(int m)
    {
        mana += m;
    }

    // requests the player to spend mana, returns false if the player cannot spend the amount requested
    public static bool spendMana(int m)
    {
        if (mana - m < 0)
        {
            return false;
        }
        else
        {
            mana -= m;
            return true;
        }
    }
}