using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    [Header("AI Agent")]
    [SerializeField] float _targetSearchDuration = 3f;
    
    NavMeshAgent _navMeshAgent;
    AISensor _sensor;
    AILocomotion _locomotion;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _sensor = GetComponent<AISensor>();
        _locomotion = GetComponent<AILocomotion>();
    }

    private void OnEnable()
    {
        _sensor.TargetSpotted += OnTargetSpotted;
        _sensor.TargetLost += OnTargetLost;
    }

    private void OnDisable()
    {
        _sensor.TargetSpotted -= OnTargetSpotted;
        _sensor.TargetLost -= OnTargetLost;
    }

    void OnTargetSpotted(GameObject obj)
    {
        _locomotion.StartPathing(obj.transform);
    }

    // TODO - has to call WaitForSeconds(1f) at the start because SearchForTarget() runs every frame
    //  whereas AISensor.Scan() runs at a different pace
    void OnTargetLost(GameObject obj)
    {
        float timer = _targetSearchDuration;
        StartCoroutine(SearchForTarget());

        IEnumerator SearchForTarget()
        {
            yield return new WaitForSeconds(1f);
            
            bool isTargetLost = true;
            
            while(timer > 0)
            {
                timer -= Time.deltaTime;
                if (_sensor.VisibleTargets.Contains(obj))
                {
                    isTargetLost = false;
                    //Debug.Log("Regained vision");
                    yield return null;
                    break;
                }

                //Debug.Log("Still no Line of Sight");
                yield return new WaitForEndOfFrame();
            }

            if (isTargetLost)
                _locomotion.StopPathing(obj.transform);
        }
    }
}
