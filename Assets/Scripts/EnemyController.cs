using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    IDLE, CHASE, FIGHT
}

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public GameObject player;
    public Vector3 playerTarget;
    public Vector3 moveTarget;
    public List<GameObject> moveBeacons;
    public EnemyState cState;

    public void Awake()
    {

        //navAgent.SetDestination
    }

    public EnemyState getState()
    {
        return cState;
    }

    public void enemyIdle()
    {
        cState = EnemyState.IDLE;
    }
    public void enemyEngage(GameObject trigger)
    {
        
    }

    public void setupNav(List<GameObject> bcn)
    {
        moveBeacons = bcn;
    }


}

