using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// controls movement of Draugr enemies
[RequireComponent(typeof(NavMeshAgent))]
public class DraugrAgent : MonoBehaviour
{
    [Header("Draugr Controller")]
    [Header("Movement Parameters")]
    [SerializeField] float _runSpeed = 4f;
    
    [Header("Gravity")]
    //[SerializeField] float _gravity = 30.0f;
    //[SerializeField] float _groundedGravity = 0.5f;

    // movement variables
    bool _isMoving;
    bool _isGrounded;
    public bool IsMoving => _isMoving;
    public bool IsGrounded => _isGrounded;
    
    // component references
    NavMeshAgent _navMeshAgent;
    AISensor _aiSensor;
    public AISensor AISensor => _aiSensor;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _aiSensor = GetComponent<AISensor>();
    }

    private void Update()
    {
        ApproachTarget();
    }

    private void Reset()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _runSpeed;
    }

    private void ApproachTarget()
    {
        if (_aiSensor.VisibleTargets.Count == 0)
            return;

        if (Vector3.Distance(transform.position, _aiSensor.VisibleTargets[0].transform.position) > _navMeshAgent.stoppingDistance)
            _navMeshAgent.SetDestination(_aiSensor.VisibleTargets[0].transform.position);
    }
}
