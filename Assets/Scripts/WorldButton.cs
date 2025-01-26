using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldButton : MonoBehaviour
{
    public GameObject connectedDoor;
    public GameObject teleportDestination;
    public int mode=1;
    private GameObject player;

    //modes: 1 makes the button open doors
    //Modes: 2 makes the button teleport the player

    public void setPlayer(GameObject pl)
    {
        player = pl;
    }

    public void activate()
    {
        player.GetComponent<Player>().playWhooshSound();
        switch (mode)
        {
            case 1:
                connectedDoor.SetActive(false);
                gameObject.SetActive(false);
                break;
            case 2:
                player.transform.position = teleportDestination.transform.position;
                player.GetComponent<Player>().getRespawnPoint().transform.position = teleportDestination.transform.position;
                gameObject.SetActive(false);
                break;
        }      
    }
}
