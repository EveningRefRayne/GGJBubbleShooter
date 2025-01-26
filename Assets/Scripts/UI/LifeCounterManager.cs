using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounterManager : MonoBehaviour
{
    public GameObject lc1, lc2, lc3;
    public void updateLifeCounters(int livesLeft)
    {
        if(livesLeft == 3)
        {
            lc1.SetActive(true);
            lc2.SetActive(true);
            lc3.SetActive(true);
        } else if(livesLeft == 2)
        {
            lc1.SetActive(true);
            lc2.SetActive(true);
            lc3.SetActive(false);
        } else if(livesLeft == 1)
        {
            lc1.SetActive(true);
            lc2.SetActive(false);
            lc3.SetActive(false);
        } else if(livesLeft == 0)
        {
            lc1.SetActive(false);
            lc2.SetActive(false);
            lc3.SetActive(false);
        }
    }
}
