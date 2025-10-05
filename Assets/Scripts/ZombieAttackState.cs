using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;

    public float stopAttackingDistance = 2.5f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      // --- Initialize Player --- //

      player = GameObject.FindGameObjectWithTag("Player").transform;
      agent = animator.GetComponent<NavMeshAgent>();
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      // --- Play Zombie Sound --- //
      if (SounManager.Instance.zombieChannel.isPlaying == false)
      {
        SounManager.Instance.zombieChannel.PlayOneShot(SounManager.Instance.zombieAttack);
      }

       LookAtPlayer();
       // --- Check if agent should stop attacking player --- //
       float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

       if (distanceFromPlayer > stopAttackingDistance)
       {
        animator.SetBool("isAttacking", false);
       }
    }


    private void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
