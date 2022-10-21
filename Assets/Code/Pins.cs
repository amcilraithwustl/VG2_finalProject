using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pins : MonoBehaviour
{
    public GameObject[] pins;

    public int numPinsRemaining()
    {
        var pinsleft = 10;
        //TODO: Improving counting left be a call function for each pin
        for (int i = 0; i < 10; i++) {
            GameObject pin = pins[i];
            if (pin.transform.rotation.x > -0.1 
                && pin.transform.rotation.x < 0.1 
                && pin.transform.rotation.z < 0.1
                && pin.transform.rotation.z > -0.1) {
                //pin is standing up (do nothing)
            }
            else {
                //pin is knocked over
                pinsleft--;
            }
        }
        return pinsleft;
    }
}
