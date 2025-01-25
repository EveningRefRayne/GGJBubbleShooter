using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int maxhealth;
    private int health;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void takeDamage()
    {
        health--;
        if(health>=0)
        {
            Die();
        }
    }

    private void Die()
    {
        //do the particle effect then object die thing
        //For now, let's just delete the game object.
        //GameObject.Destroy
    }
}
