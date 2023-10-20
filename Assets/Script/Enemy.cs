using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int maxHealth, currentHealth;
    public float chaseRange = 20f; // Range at which the enemy starts chasing the player.
    public float attackRange = 4f; // Range at which the enemy attacks the player.
    public float chaseSpeed = 5f; // Chase movement speed.
    public LayerMask whatIsGround, whatIsPlayer;
    private Transform player; // Reference to the player's transform.
    [SerializeField]
    private NavMeshAgent navMeshAgent;
    public Animator enemyAni;
    public bool attackAni, isTakingDmg;
    public bool playerInSightRange, playerInAttackRange;
    private float nextAttackTime;
    private bool isCoolingDown => Time.time < nextAttackTime;    
    private void StartCoolDown(float cooldownTime) => nextAttackTime = Time.time + cooldownTime;
    public Rigidbody rb;
    public float knockbackForce = 10f;
    public PlayerMovement1 playerReference;
    public BoxCollider Enemycollider;
    public Vector3 knockbackDirection = new Vector3(0,0,-1); // Adjust the direction as needed
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        Enemycollider = GetComponentInChildren<BoxCollider>();
        Enemycollider.enabled = false;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if(playerReference.isDead) return;
        if (distanceToPlayer <= chaseRange && distanceToPlayer > attackRange && isTakingDmg == false)
        {
            ChasePlayer();
        }
        if(distanceToPlayer <= attackRange && attackAni == false){
            if(isCoolingDown) return;
            AttackPlayer();
        }
        if(distanceToPlayer > chaseRange){
            enemyAni.SetBool("Walking", false);
            navMeshAgent.speed = 0;
            navMeshAgent.SetDestination(transform.position);
        }
        
    }

    public void KnockedBack(){
        rb.velocity = Vector3.zero;
        rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode.Impulse);
    }
    void ChasePlayer()
    {
        // Set the player's position as the destination for chasing.
        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(player.position);
        enemyAni.SetBool("Walking", true);
       
    }

    void AttackPlayer()
    {
        navMeshAgent.SetDestination(transform.position);
        transform.LookAt(player);
        StartCoroutine(AttackCo());
        StartCoolDown(1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        
        // Check if the collision is with the target object
        if (other.CompareTag("Player"))
        {
            playerReference.TakeDamage();
        }
        
    }

    private IEnumerator AttackCo(){
        attackAni = true;
        enemyAni.SetBool("Walking", false);
        enemyAni.SetBool("Attacking", true);
        yield return new WaitForSeconds(.55f);
        Enemycollider.enabled = true;
        yield return new WaitForSeconds(.9f);
        enemyAni.SetBool("Attacking", false);
        Enemycollider.enabled = false;
        attackAni = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0){
            StartCoroutine(DeathCo());
            return;
        }
        StartCoroutine(TakeDmgAni());
    }

    private IEnumerator DeathCo(){
        attackAni = true;
        enemyAni.SetTrigger("Death");
        enemyAni.SetBool("Walking", false);
        enemyAni.SetBool("Attacking", false);
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(1.1f);
        Destroy(gameObject);
        
    }

    private IEnumerator TakeDmgAni(){
        enemyAni.SetBool("takingDmg", true);
        isTakingDmg = true;
        enemyAni.SetBool("Walking", false);
        navMeshAgent.speed = 0;
        navMeshAgent.SetDestination(transform.position);
        enemyAni.SetBool("Attacking", false);
        yield return new WaitForSeconds(.5f);
        enemyAni.SetBool("takingDmg", false);
        isTakingDmg = false;
    }
}
