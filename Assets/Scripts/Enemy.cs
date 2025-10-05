using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;

    private NavMeshAgent navAgent;

    public bool isDead;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            int randomValue = Random.Range(0,2); // 0 or 1
            if(randomValue == 0)
            {
               animator.SetTrigger("DIE1"); 
            }
            else
            {
              animator.SetTrigger("DIE2");   
            }
            isDead = true;
            // Zombie Dead Sound //
            SounManager.Instance.zombieChannel.PlayOneShot(SounManager.Instance.zombieDeath);

            
        }
        else
        {
            animator.SetTrigger("DAMAGE");

            // Hurt Sound //
            SounManager.Instance.zombieChannel2.PlayOneShot(SounManager.Instance.zombieHurt);

        }
    }

    private void OnDrawGizmos()
    {
       Gizmos.color = Color.red;
       Gizmos.DrawWireSphere(transform.position, 2.5f); //Attacking // Stop Attacking

       Gizmos.color = Color.blue;
       Gizmos.DrawWireSphere(transform.position, 100f); //Detection (Start Chasing)

       Gizmos.color = Color.green;
       Gizmos.DrawWireSphere(transform.position, 101f); //StopChasing
    }
}
