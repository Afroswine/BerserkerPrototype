using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// based on code from https://youtu.be/rs7xUi9BqjE?si=HRs5U7VpvlL6q-PK

public class EnemyBrainStupid : MonoBehaviour
{
    public Transform Target => _target;
    public EnemyReferences EnemyReferences => _enemyReferences;

    [Header("Enemy Brain Stupid")]
    [SerializeField] Transform _target;
    [SerializeField] float _pathUpdateDelay;
    
    EnemyReferences _enemyReferences;
    float _pathUpdateDeadline;
    float _stoppingDistance;

    private void Awake()
    {
        _enemyReferences = GetComponent<EnemyReferences>();
    }

    private void Start()
    {
        _stoppingDistance = _enemyReferences.NavMeshAgent.stoppingDistance;
    }

    private void Update()
    {
        if (_target != null)
        {
            bool inRange = Vector3.Distance(transform.position, _target.position) <= _stoppingDistance;

            if (inRange)
                LookAtTarget();
            else
                UpdatePath();
        }
        //_enemyReferences.Animator.SetFloat("speed", _enemyReferences.NavMeshAgent.desiredVelocity.sqrMagnitude);
    }

    private void LookAtTarget()
    {
        Vector3 lookPos = _target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);  // TODO - change 0.2f to a modifiable value (and should it be deltaTime?)
    }

    private void UpdatePath()
    {
        if(Time.time >= _pathUpdateDeadline)
        {
            Debug.Log("Updating Path");
            _pathUpdateDeadline = Time.time + _pathUpdateDelay;
            _enemyReferences.NavMeshAgent.SetDestination(_target.position);
        }
    }
}