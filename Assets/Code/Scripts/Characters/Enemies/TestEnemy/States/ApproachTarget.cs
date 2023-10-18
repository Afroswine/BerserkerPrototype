using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ApproachTarget : IState
{
    private readonly EnemyBrain _enemyBrain;
    private readonly NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;

    private Vector3 _lastPosition = Vector3.zero;

    public float TimeStuck;

    public ApproachTarget(EnemyBrain enemyBrain, NavMeshAgent navMeshAgent, Animator animator)
    {
        _enemyBrain = enemyBrain;
        _navMeshAgent = navMeshAgent;
        _animator = animator;
    }

    public void Tick()
    {
        if (Vector3.Distance(_enemyBrain.transform.position, _lastPosition) <= 0f)
            TimeStuck += Time.deltaTime;

        _lastPosition = _enemyBrain.transform.position;
    }

    // TODO - doesn't yet utilize approach distance
    public void OnEnter()
    {
        TimeStuck = 0f;
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(_enemyBrain.Target.transform.position);
        //_animator.SetFloat(Speed, 1f);
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;
        //_animator.SetFloat(Speed, 0f);
    }

    public Color GizmoColor()
    {
        throw new System.NotImplementedException();
    }
}
