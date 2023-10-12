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

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

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

    private IEnumerator AttackCo(){
        attackAni = true;
        enemyAni.SetBool("Walking", false);
        enemyAni.SetBool("Attacking", true);
        yield return new WaitForSeconds(1.1f);
        enemyAni.SetBool("Attacking", false);
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
