using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public List<GameObject> beacons;
    public GameObject enemy;
    public int toSpawn = 5;
    private float lastSpawnTime = 0;
    public float cooldown = 2;
    private GameObject spn;
    public bool active = false;
    private GameObject trg;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void activateSpawner(GameObject obj)
    {
        trg=obj;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (toSpawn>0 && active)
        {
            //Debug.Log("bah");
            if (Time.time>lastSpawnTime+cooldown)
            {
                spn = Instantiate(enemy, transform.position, Quaternion.identity);
                spn.GetComponent<EnemyController>().setupNav(beacons);
                trg.GetComponent<CombatTrigger>().register(spn);
                lastSpawnTime = Time.time;
                toSpawn--;
            }
            if (toSpawn<=0)
            {
                trg.GetComponent<CombatTrigger>().spawnerDying();
                Destroy(this.gameObject);
            }
        }
    }
}
