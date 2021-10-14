/**********************************************
| GoblinWarrior V1.0.0                        |
| Author: David Bush, T5                      |
| Description: This is the GoblinWarrior class|
| that inherits from the Enemy superclass.    |
| This will contain all variables and methods |
| associtiated with the GoblinWarrior enemy   |
| type. Each GoblinWarrior has the unique     |
| ability Rage Mode. When it is hit by a spell|
| it'5s damage and speed increase by a flat   |
| ammount each time.                          |
| Bugs:                                       |
**********************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GoblinWarrior : Enemy
{
    // Used to store RigidBody2d Component
    private Rigidbody2D rb;
    // Used to store Agent component
    private NavMeshAgent agent;

    // Variable used for flat damage boost
    private int damage_boost;
    // Variable used for flat speed increase
    private float speed_boost;

    // TEST
    private RaycastHit2D raycastHit2D;

    // Constructor for GoblinGrunt
    public GoblinWarrior()
    {
        max_health = health = 350;
        damage = 50;
        damage_boost = 5; 
        move_speed = 14f;
        speed_boost = 2f;
        attack_speed = attackTimer = 1.75f; // 1714 damage per minute
        attackConnected = false;
        knock_back = 400f;
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
        if (attackTimer <= attack_speed) {
            attackTimer += Time.fixedDeltaTime;
        }
        Debug.Log("Current Damage: "+GetDamage());
    }
    // Update is called once per frame
    void Update()
    {
        // Check if unit has no health left
        if (health <= 0) {
            // SoundManagerScript.PlaySound("enemyDeath");
            Destroy(gameObject); // Destroy unit
        }
        raycastHit2D = Physics2D.CircleCast(gameObject.transform.position, 10f, new Vector3(1,0,0));
        Debug.Log("Test: "+raycastHit2D.distance);
    }
    // Function that checks for collisions
    void OnTriggerEnter2D(Collider2D collision)
    {        
        GameObject other = collision.gameObject;
        if(other.tag == "Spell") { // Check if enemy collided with spell

            // Add damage each time its hit by spell
            ChangeDamage(damage_boost);
            // Increase speed
            agent.speed += speed_boost;
            agent.acceleration += speed_boost;

            if(other.GetComponent<FireBall>()) { // Check if spell was Fireball
                RecieveDamage(other.GetComponent<FireBall>().getSpellDamage()); // Recieve damage 
                rb.AddForce((other.transform.position - transform.position) * other.GetComponent<FireBall>().getSpellKnockBack() * -1.0f, ForceMode2D.Impulse);
            }
        }
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
    public int GetDamageBoost()
    {
        return damage_boost;
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
}
