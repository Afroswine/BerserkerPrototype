using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebugNavMeshAgent : MonoBehaviour
{
    [SerializeField] bool _velocity;
    [SerializeField] bool _desiredVelocity;
    [SerializeField] bool _path;

    NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        if (_velocity)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + _agent.velocity);
        }

        if (_desiredVelocity)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + _agent.desiredVelocity);
        }

        if (_path)
        {
            Gizmos.color = Color.black;
            var agentPath = _agent.path;
            Vector3 prevCorner = transform.position;
            foreach(var corner in agentPath.corners)
            {
                Gizmos.DrawLine(prevCorner, corner);
                Gizmos.DrawSphere(corner, 0.1f);
                prevCorner = corner;
            }
        }
    }
}
