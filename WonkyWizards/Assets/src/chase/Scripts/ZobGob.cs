using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZobGob : BossSuperClass
{
    // --- Start of Singleton Pattern --- \\
    private static readonly object obj = new object();
    private ZobGob()
    {
        maxHealth = health = 1200;
        damage = 90;
        moveSpeed = 8f;
        minimumSpeed = 0.5f;
        maximumSpeed = 1f;
        attackSpeed = attackTimer = 2f;
        targetDistance = 50f;
        stoppingDistance = 1f;
        attackConnected = false;
        knockBack = 1f;
    }
    private static ZobGob instance = null; // set the instance variable to null
    public static ZobGob GetInstance
    {
        get
        {
            if (instance == null)
            {
                lock (obj)
                {
                    if (instance == null)
                        instance = new ZobGob();
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }
    // --- End of Singleton Pattern --- \\
    // Used to store RigidBody2d Component
    private Rigidbody2D rb;
    // Used to store Agent component
    private NavMeshAgent agent;

    void Start()
    {
        //Rigid body component is grabbed
        rb = GetComponent<Rigidbody2D>();
        // NavMesh Agent Component is grabbed
        agent = GetComponent<NavMeshAgent>();
    }
    void FixedUpdate()
    {
        // Time is incremented and checked
        if (attackTimer <= attackSpeed)
        {
            attackTimer += Time.fixedDeltaTime;
        }
    }
    void Update()
    {
        //Checks if the zob gob has any health left if not it destroys the game object
        if (health <= 0)
        {
            Destroy(gameObject); // Game Object gets destroyed
        }
    }

    // Checks to see if the zob Gob is hit by anything
    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.tag == "Spell")
        { // Check if enemy collided with spell

            if (other.GetComponent<FireBall>())
            { // Check if spell was Fireball
                DamageRecieved(other.GetComponent<FireBall>().getSpellDamage()); // Gets Damaged when hit by the Spell
                rb.AddForce((other.transform.position - transform.position) * other.GetComponent<FireBall>().getSpellKnockBack() * -1.0f, ForceMode2D.Impulse); // FireBall.getKnockback();
            }
            else if (other.GetComponent<MagicMissle>())
            { // Check if spell was MagicMissle
                DamageRecieved(other.GetComponent<MagicMissle>().getSpellDamage()); // Gets Damaged when hit by the Spell
                rb.AddForce((other.transform.position - transform.position) * other.GetComponent<MagicMissle>().getSpellKnockBack() * -1.0f, ForceMode2D.Impulse);
            }
            else if (other.GetComponent<IceBeam>())
            { // Check if spell was IceBeam
                ChangeMoveSpeed(other.GetComponent<IceBeam>().Freeze() * -1); // Gets Damaged when hit by the Spell
            }
            else if (other.GetComponent<AcidSpray>())
            { // Check if spell was AcidSpray
                DamageRecieved(other.GetComponent<AcidSpray>().getSpellDamage()); // Gets Damaged when hit by the Spell
                rb.AddForce((other.transform.position - transform.position) * other.GetComponent<AcidSpray>().getSpellKnockBack() * -1.0f, ForceMode2D.Impulse);
            }
            else if (other.GetComponent<Slimeball>())
            { // Check if spell was SlimeBall
                DamageRecieved(other.GetComponent<Slimeball>().getSpellDamage()); //Recieve damage
                rb.AddForce((other.transform.position - transform.position) * other.GetComponent<Slimeball>().getSpellKnockBack() * -1.0f, ForceMode2D.Impulse);
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Goal")
        { // Sees if the zob gob hit the goal
            if (attackConnected)
            { // Make sure attack is available and attack is successful
                attackTimer = 0.0f; // Reset timer
                attackConnected = false; // Reset attack 
            }
        }
    }
    public void ChangeMoveSpeed(float amountOfSpeed)
    {
        if (agent.speed >= minimumSpeed && amountOfSpeed < 0)
        {
            agent.speed += amountOfSpeed;
            agent.acceleration += amountOfSpeed;
        }
        else if (agent.speed <= maximumSpeed && amountOfSpeed > 0)
        {
            agent.speed += amountOfSpeed;
            agent.acceleration += amountOfSpeed;
        }
    }
}

