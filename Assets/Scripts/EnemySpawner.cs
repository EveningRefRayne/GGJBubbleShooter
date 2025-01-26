using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public List<GameObject> beacons;
    public GameObject enemy;
    public int toSpawn = 5;
    private float lastSpawnTime;
    public float cooldown = 2;
    private GameObject spn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (toSpawn>0)
        {
            if (Time.time>lastSpawnTime+cooldown)
            {
                spn = Instantiate(enemy, transform);
                spn.GetComponent<EnemyController>().setupNav(beacons);
                lastSpawnTime = Time.time;
                toSpawn--;
            }
            if (toSpawn>=0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
