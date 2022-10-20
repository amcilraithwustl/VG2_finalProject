using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bowlable : MonoBehaviour
{
    private XRGrabInteractable grabScript;

    public bool isGrabbable = true;
    public bool hasTier = true;

    public int tier = 0;

    // Start is called before the first frame update
    void Start()
    {
        grabScript = GetComponent<XRGrabInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        var unlocked = !hasTier || tier <= GameController.Instance.currentTier;
        grabScript.enabled = isGrabbable && unlocked;
    }

    public void setGrabbable(bool state)
    {
        isGrabbable = state;
    }
}