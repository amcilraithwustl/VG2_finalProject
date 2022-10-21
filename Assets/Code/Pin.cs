using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    public bool isStandingUp(){
        
        if (gameObject.transform.rotation.x > -0.1 
            && gameObject.transform.rotation.x < 0.1 
            && gameObject.transform.rotation.z < 0.1
            && gameObject.transform.rotation.z > -0.1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
