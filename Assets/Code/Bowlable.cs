using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;
using GizmoUtility = UnityEditor.Rendering.GizmoUtility;

public class Bowlable : MonoBehaviour {
    //Outlets
    private XRGrabInteractable grabScript;
    private Rigidbody rb;
    private Transform trans;
    private Collider[] col;

    private NavMeshAgent agent;
    //Config
    public bool isGrabbable = true;
    public bool isGrabbed = false;
    public bool hasTier = true;
    public bool startStatic = true;
    public int tier = 0;
    public float shrink = 1;
    public float shrinkSpeedFactor = 5;

    public bool startWithoutCollider = false;
    //State
    public bool hasBeenGrabbed = false;

    //Methods
    void Start() {
        grabScript = GetComponent<XRGrabInteractable>();
        grabScript.selectEntered.AddListener(HandleSelected);
        grabScript.selectExited.AddListener(HandleDropped);

        rb = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
        agent =GetComponentInChildren<NavMeshAgent>();
        col = GetComponentsInChildren<Collider>();
    }

    void HandleSelected(SelectEnterEventArgs args) {
       
        GameController.Instance.currentlyGrabbed++;
        isGrabbed = true;
        
        print("Has Been Grabbed");
        hasBeenGrabbed = true;
    // StartCoroutine(turnOnGrabbed());
}
    IEnumerator turnOnGrabbed()
    {
        yield return new WaitForSeconds(.1f);
        print("Has Been Grabbed");
        hasBeenGrabbed = true;
        
    } 

  
    void HandleDropped(SelectExitEventArgs args) {
        print("Has Been Dropped");
        GameController.Instance.currentlyGrabbed--;
        isGrabbed = false;
    }

    float calcScaleDelta(float start, float target) {
        var percentError = (start - target) / target;
        return -1 * shrinkSpeedFactor * Time.deltaTime * percentError;
    }

    // Update is called once per frame
    void Update()
    {
        //If it has start static and it hasn't been grabbed, physics won't start
        rb.isKinematic = startStatic && !hasBeenGrabbed;

        //If it has been grabbed before, the size will scale to fit the bowling alley
        if (hasBeenGrabbed)
        {
            //Scale each dimension separately for smooth locomotion
            var scale = trans.localScale;
            scale += new Vector3(
                calcScaleDelta(scale.x, shrink),
                calcScaleDelta(scale.y, shrink),
                calcScaleDelta(scale.z, shrink)
            );
            trans.localScale = scale;

            if (agent) agent.enabled = false;
        }


        //Check if the tier is enabled (if relevant)
        var unlocked = !hasTier || tier <= GameController.Instance.currentTier;

        if (false && !unlocked)
        {
            foreach (var c in col)
            {

                if (startWithoutCollider)
                {
                    c.enabled = startStatic && !hasBeenGrabbed;
                }
                else
                {
                    c.enabled = true;
                }
            }
        }

        if (isGrabbed) {
            grabScript.enabled = true;
        }
        else {
            grabScript.enabled = isGrabbable && unlocked && GameController.Instance.currentlyGrabbed == 0;
        }
    }

    private void OnDrawGizmos() {
        if (isGrabbable) {
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

    public void setGrabbable(bool state) {
        isGrabbable = state;
    }
}