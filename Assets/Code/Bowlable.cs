using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bowlable : MonoBehaviour
{
    private XRGrabInteractable grabScript;

    public bool isGrabbable;

    // Start is called before the first frame update
    void Start()
    {
        grabScript = GetComponent<XRGrabInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        grabScript.enabled = isGrabbable;
    }

    public void setGrabbable(bool state)
    {
        isGrabbable = state;
    }

}
