using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    public float bulletSpeed = 1f;
    public float life = 12;
    private float ctd;

    public void Awake()
    {
        ctd = Time.time;
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
    }

    // FixedUpdate is called once per physics frame
    void FixedUpdate()
    {
        if (Time.time >= ctd + life) Destroy(this.gameObject);
        //gameObject.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Player>()!=null)
        {
            collision.gameObject.GetComponent<Player>().Damage();
        }
        Destroy(this.gameObject);
    }
}
