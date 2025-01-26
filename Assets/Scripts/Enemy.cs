using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int maxhealth;
    private int health;
    public GameObject shot;
    // Start is called before the first frame update
    void Start()
    {
        health = maxhealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void takeDamage()
    {
        health--;
        if(health<=0)
        {
            Die();
        }
    }

    private void Die()
    {
        GetComponent<EnemyController>().cTrigger.remove(gameObject);
        Destroy(this.gameObject);
    }

    public void shoot(Vector3 pos)
    {
        //Debug.Log("Shooting Called");
        Instantiate(shot, gameObject.transform.position, Quaternion.LookRotation(pos-gameObject.transform.position, Vector3.up));
    }
}
