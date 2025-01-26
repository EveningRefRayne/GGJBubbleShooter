using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    public GameObject spawner;
    public List<GameObject> enemyList;
    public GameObject connectedButton;
    public GameObject pl;

    public void spawnerDying()
    {
        spawner = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        pl = other.gameObject;
        if (spawner!=null) spawner.GetComponent<EnemySpawner>().activateSpawner(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        foreach (GameObject enm in enemyList)
        {
            enm.GetComponent<EnemyController>().enemyEngage(other.gameObject);
        }

    }

    private void Update()
    {
        if ((pl != null))
        {


            if (Vector3.Distance(transform.position, pl.transform.position) > 60f)
            {
                foreach (GameObject enm in enemyList)
                {
                    enm.GetComponent<EnemyController>().enemyIdle();
                }
            }
        }
    }

    public void register(GameObject enm)
    {
        enemyList.Add(enm.GetComponent<EnemyController>().registerCombatTrigger(gameObject));
    }

    public void remove(GameObject enm)
    {
        enemyList.Remove(enm);
        Destroy(enm);
        if (enemyList.Count == 0)
        {
            if (connectedButton != null)
            {
                connectedButton.SetActive(true);
                connectedButton.GetComponent<WorldButton>().setPlayer(pl);
            }
        }
    }
}
