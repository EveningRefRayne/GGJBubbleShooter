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
    public CombatTrigger cTrigger;
    public float enemyAIUpdateTime;
    private float lastAIUpdateTime=0;
    private float shootingDistance;
    public float shootingTime;
    private float lastShotTime=0;

    public void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        randomizeNextUpdateTime();
        shootingDistance = Random.Range(10f, 25.0f);
        shootingTime = Random.Range(2.0f, 5.0f);
    }

    public void randomizeNextUpdateTime()
    {
        enemyAIUpdateTime = Random.Range(0.5f, 2f);
    }

    public GameObject registerCombatTrigger(GameObject trg)
    {
        cTrigger = trg.GetComponent<CombatTrigger>();
        return gameObject;
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
        player = trigger;
        if (cState!=EnemyState.CHASE || cState !=EnemyState.FIGHT)
        {
            cState = EnemyState.CHASE;
        }
    }

    public void setupNav(List<GameObject> bcn)
    {
        moveBeacons = bcn;
    }




    //AI
    public void Update()
    {
        if (cState==EnemyState.FIGHT && Time.time >= lastShotTime + shootingTime)
        {
            //Debug.Log("Shooting at " + player.transform.position);
            GetComponent<Enemy>().shoot(player.transform.position);
            lastShotTime = Time.time;
        }
        else if (cState==EnemyState.FIGHT)
        {
            //Debug.Log("last shot " + lastShotTime + " and shooting every " + shootingTime);
        }
        if (Time.time>=lastAIUpdateTime+enemyAIUpdateTime)
        {
            runEnemyAI();
            lastAIUpdateTime = Time.time;
        }
    }


    public void runEnemyAI()
    {
        switch (cState)
        {
            case (EnemyState.IDLE):
                navAgent.SetDestination(moveBeacons[Random.Range(0, (moveBeacons.Count - 1))].transform.position);
                return;
            case (EnemyState.CHASE):
                navAgent.SetDestination(player.transform.position);
                if(Vector3.Distance(transform.position,player.transform.position)<=shootingDistance)
                {
                    cState = EnemyState.FIGHT;
                }
                return;
            case (EnemyState.FIGHT):
                navAgent.SetDestination(Quaternion.AngleAxis(Random.Range(-60f,60f), Vector3.up)*Vector3.Lerp(transform.position,player.transform.position,0.5f));
                if (Vector3.Distance(transform.position, player.transform.position) > shootingDistance)
                {
                    cState = EnemyState.CHASE;
                }
                break;
        }
    }

}

