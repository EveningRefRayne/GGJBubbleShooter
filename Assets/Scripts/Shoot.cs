using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    //RaycastResult rayHit;
    private bool fireDown;
    public float reFireTime = 0.5f;
    private float firedTime;
    private float flashTime = 0.25f;
    public GameObject cam;
    public GameObject firePart;
    public AudioSource aS;

    void Update()
    {
        switch (GetComponent<Player>().Mode)
        {
            case GameStates.TITLE:
                break;
            case GameStates.PLAY:
                UpdateGameplay();
                break;
        }
    }


    private void UpdateGameplay()
    {
        if (Time.time> firedTime + flashTime)
        {
            firePart.SetActive(false);
        }
        if (Time.time > firedTime+reFireTime && fireDown)
        {
            fireDown = false;
        }
        if (Input.GetAxis("Fire1") > 0 && !fireDown)
        {
            firedTime = Time.time;
            firePart.SetActive(true);
            aS.pitch = Random.Range(0.9f, 1.1f);
            aS.Play();
            fireDown = true;
            //Debug.Log("Shoot at: " + firedTime);
            Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.GetComponent<Enemy>() != null)
                {
                    hit.collider.gameObject.GetComponent<Enemy>().takeDamage();
                }
                else if (hit.collider.gameObject.GetComponent<WorldButton>() !=null)
                {
                    hit.collider.gameObject.GetComponent<WorldButton>().activate();
                }
            }
        }
    }
}
