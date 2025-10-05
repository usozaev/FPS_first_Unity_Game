using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrolingState : StateMachineBehaviour
{
    float timer;
    public float patrolingTime = 10f;
    Transform player;

    NavMeshAgent agent;

    public float detectionArea = 18f;
    public float patrolSpeed = 2f;

    List<Transform> waypointsList = new List<Transform>();
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      // --- Initialize Player --- //

      player = GameObject.FindGameObjectWithTag("Player").transform;
      agent = animator.GetComponent<NavMeshAgent>();

      agent.speed = patrolSpeed;
      timer = 0;
      // --- Get all Waypoints and Move to First Point --- //

      GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");
      foreach(Transform t in waypointCluster.transform)
      {
        waypointsList.Add(t);
      }

      Vector3 nextPosition = waypointsList[Random.Range(0, waypointsList.Count)].position;
      agent.SetDestination(nextPosition);
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      // --- Play Zombie Sound --- //
      if (SounManager.Instance.zombieChannel.isPlaying == false)
      {
        SounManager.Instance.zombieChannel.clip = SounManager.Instance.zombieWalking;
        SounManager.Instance.zombieChannel.PlayDelayed(1f);
      }


       // --- Check if agent arrived at waypoint and move to the next --- //
       if (agent.remainingDistance <= agent.stoppingDistance)
       {
        agent.SetDestination(waypointsList[Random.Range(0, waypointsList.Count)].position);
       }


      // --- Transition to Idle State --- //
      timer += Time.deltaTime;
      if (timer >= patrolingTime)
      {
        animator.SetBool("isPatroling", false);
      }


       // --- Transition to Chase State --- //
       float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
       if (distanceFromPlayer < detectionArea)
       {
        animator.SetBool("isChasing",true);
       }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // --- Stop Agent --- //
       agent.SetDestination(agent.transform.position);
       
       SounManager.Instance.zombieChannel.Stop();
    }
}
