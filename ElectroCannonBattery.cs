using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroCannonBattery : MonoBehaviour
{
    [Header("Build Setting")]
    public bool isBuilt = true;

    [Header("Charge Settings")]
    public int maxChargeCount = 5;
    public int chargeCount = 0;

    public ElectroCannonBattery connectedBattery;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public void setConnectedBattery(GameObject battery)
    {
        connectedBattery = battery.GetComponent<ElectroCannonBattery>();
    }

    public void IncrementChargeCount()
    {
        if (chargeCount >= maxChargeCount)
            chargeCount++;
        else
            Debug.Log("Battery Full");

        Debug.Log(chargeCount);
    }

    public void DeincrementChargeCount()
    {
        chargeCount--;
    }
}
