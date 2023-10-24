using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocomotion : MonoBehaviour
{
    [Header("AI Locomotion")]
    [Tooltip("Start when at least this distance from the target position.")]
    [SerializeField] float _startingDistance = 5;    // start navigating when this far from the target destination
    [SerializeField] int _pathUpdateFrequency = 10;
    float _pathUpdateInterval;

    NavMeshAgent _agent;
    Animator _animator;

    int _speedHash = AnimatorHash.Speed;
    //public int SpeedHash => _speedHash;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _pathUpdateInterval = 1 / _pathUpdateFrequency;
    }

    private void Update()
    {
        _animator.SetFloat(_speedHash, _agent.velocity.magnitude);
    }

    public void StartPathing(Transform target)
    {
        StartCoroutine(PathToTarget(target));
    }

    public void StopPathing(Transform target)
    {
        StopAllCoroutines();
    }

    private IEnumerator PathToTarget(Transform target)
    {
        if (Vector3.Distance(transform.position, target.position) > _startingDistance)
            _agent.SetDestination(target.position);

        yield return new WaitForSeconds(_pathUpdateInterval);
        StartCoroutine(PathToTarget(target));
    }
}
