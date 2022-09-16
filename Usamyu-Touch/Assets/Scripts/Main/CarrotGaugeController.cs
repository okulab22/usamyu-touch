using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotGaugeController : MonoBehaviour
{
    [SerializeField] private GameObject carrotObj;

    public void setCarrotGauge(int lifeCount)
    {
        for (int i = 0; i< transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        // set
        for (int i = 0; i < lifeCount; i++)
        {
            Instantiate(carrotObj, transform);
        }
    }
}
