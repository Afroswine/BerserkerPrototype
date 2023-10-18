using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// based on code from https://youtu.be/rs7xUi9BqjE?si=HRs5U7VpvlL6q-PK

// TODO - this script might be a trap...
// Why should other classes have to reference this script to access other components when it could be done directly?

[DisallowMultipleComponent]
public class EnemyReferences : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent { get; private set; }
    public Animator Animator { get; private set; }

    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
    }
}
