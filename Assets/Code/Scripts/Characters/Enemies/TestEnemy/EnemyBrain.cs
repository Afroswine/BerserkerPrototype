
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyBrain : MonoBehaviour
{
    // TODO - analyse the behaviors/states of Souls enemy to determine which states to make.
    //  1.  If a souls enemy sees the player it will (typically) run up to them, and once it is within a *certain range* it will change its behaviour
    //  2.  Now that "certain range" can vary, even within a single enemy. Some enemies prefer to use different attacks from different ranges
    //      So if the player is far enough away the enemy might use a distance-closing attack, or they may just close the distance by approaching normally
    //      There are almost ALWAYS multiple things a souls enemy could do in any given scenario.
    //  3.  Most enemies seem to have a "preferred range", a distance where the majority of their attacks should be able to hit. Not too close, but not too far.
    //  4.  Tougher enemies are typically ones that move around a lot, and force the player to do the same. Constant repositioning creates exciting gameplay.
    //      These enemies have ways of closing gaps AND creating them in order to use their moveset to the fullest. (Jumping backwards, charging forwards, etc)
    //  5.  Some enemies are designed to work well in groups, such as shield enemies or dog enemies which have a tendency to circle around the player and flank them.
    //  6.  Some enemies even react to specific player actions (drinking estus, casting a spell, etc). In these scenarios the enemy is much more likely to use-
    //      a ranged attack or gap closing move than it otherwise would.
    //      This means that enemies constantly weigh their options based on multiple factors.
    //  7.  If the player runs outside of the maximum range, the enemy will generally return to its original position, and continue its non-combat behaviors.
    //  8.  If stealth is being used (I.E. Sekiro), the enemies may enter an "investigative state" where they will wait around the last known location of the player for a time.
    //  9.  When not in combat, some enemies simply idle, just waiting for the player. Other enemies may follow a specific path, or pretend to perform a specific duty
    //      such as mining ore, eating a carcass, or even talk to other enemies. Extra-lively enemies will even loop between multiple duties.

    //  All enemies will probably have these states
    //  Idling state (when no threat is present)
    //  Attacking state (takes in an attack, enemy AI decides which attack to use by weighing relevant factors)
    //      An attack should have min/max distance from the target, and maybe prefer to avoid spamming the same attack
    //      Virtual stamina could be used to make an enemy stop attacking, and briefly circle the player occasionally
    //      Do souls enemies prefer to use their attacks in a certain order?
    //  Approaching state (approaches target until reaching a certain distance and begin circling, can transition to an attack at any time)
    //      Will all enemies need to approach the player? How is the approach distance determined?

    [Header("Enemy Brain")]
    [SerializeField] float _approachDistance = 2f;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private FOVPursuer _fov;
    //private StateMachine _stateMachine;

    public GameObject Target { get; set; }
    protected float ApproachDistance => _approachDistance;
/*
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _stateMachine = new StateMachine();

        // create states


        void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
        void Any(IState to, Func<bool> condition) => _stateMachine.AddAnyTransition(to, condition);
    }

    private void Update() => _stateMachine.Tick();

    private void OnDrawGizmos()
    {
        if (_stateMachine == null)
            return;

        Gizmos.color = _stateMachine.GetGizmoColor();
        Gizmos.DrawSphere(transform.position + Vector3.up * 3, 0.4f);
    }
*/
}
