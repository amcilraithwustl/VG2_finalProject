using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.AI;
public class AI : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private Animator _animator;

    public Transform patroRoute;
    private int patroIndex;

    public Transform priorityTarget;

    public float chaseDistance;
    public Transform target;
    
    // Start is called before the first frame update
    void Start() {
        navAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (patroRoute) {
            target = patroRoute.GetChild(patroIndex);
            float d = Vector3.Distance(transform.position, target.position);
            //print("Distance: " + d);
            if (d <= 1.75f) {
                patroIndex++;
                if (patroIndex >= patroRoute.childCount) {
                    patroIndex = 0;
                }
            }
        }

        if (priorityTarget) {
            float pd = Vector3.Distance(transform.position, priorityTarget.position);
            if (pd <= chaseDistance) {
                target = priorityTarget;
                //GetComponent<Renderer>().material.color = Color.red;
            }
            else {
                //GetComponent<Renderer>().material.color = Color.white;
            }
        }
        if (target) {
            navAgent.SetDestination(target.position);
        }

        if (_animator)
        {
            _animator.SetFloat("velocity", navAgent.velocity.magnitude);
        }
        
        
    }
}
