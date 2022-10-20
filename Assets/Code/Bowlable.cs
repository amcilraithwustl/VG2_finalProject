using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using GizmoUtility = UnityEditor.Rendering.GizmoUtility;

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

    private void OnDrawGizmos()
    {
        if (isGrabbable)
        {
            // Gizmos.color = Color.blue;
            // Gizmos.DrawWireSphere(gameObject.transform.position, 1);
            var style = GUIStyle.none;
            style.normal.textColor = Color.blue;
            style.fontSize = 20;
            style.alignment = TextAnchor.MiddleCenter;
            var text = hasTier ? "" + tier : "--";
            Handles.Label(transform.position, text, style);
        }
    }

    public void setGrabbable(bool state)
    {
        isGrabbable = state;
    }
}