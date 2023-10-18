using System.Collections;
using System;
using UnityEngine;

public class VisualDetection : MonoBehaviour
{
    public event Action TargetSpotted = delegate { };

    public GameObject Target => _target;
    public LayerMask TargetMask => _targetMask;
    public LayerMask ObstructionMask => _obstructionMask;
    public Transform Origin => _origin;
    public float Radius => _radius;
    //public float AlertRadius => _alertRadius;
    public float CurrentRadius => _currentRadius;
    public float Angle => _angle;
    //public float AlertAngle => _alertAngle;
    public float CurrentAngle => _currentAngle;
    public float CheckInterval => _checkInterval;
    public bool CanSeeTarget => _canSeeTarget;

    // TODO - EWWWWWW TAGS GET RID OF IT
    // Instead of tags, maybe game objects could call an event upon creation to add themselves to a list of targets that can be accessed?
    // TODO - tags AND a Layermask??? condense to only one
    [Header("Visual Detection")]
    [Header("Target Information")]
    [SerializeField] string _targetTag = "Player";
    GameObject _target;

    [Header("Filtering")]
    [SerializeField] LayerMask _targetMask;
    [SerializeField] LayerMask _obstructionMask;

    [Header("Field of View - Neutral")]
    [Tooltip("The origin of the view. Use the head or the eyes.")]
    [SerializeField] Transform _origin;
    [SerializeField] float _radius = 5f;
    [SerializeField, Range(0, 360)] float _angle = 90f;
    protected float _currentRadius;
    protected float _currentAngle;
    float _checkInterval = 0.2f;

    /*
    [Header("Field of View - Alert")]
    [SerializeField] float _alertRadius = 10f;
    [SerializeField, Range(0, 360)] float _alertAngle = 360f;
    [Tooltip("How long (in seconds) line of sight must be broken to lose a target.")]
    [SerializeField] float _alertDuration = 3f;
    */

    bool _canSeeTarget;

    private void Start()
    {
        ResetFOV();
        _target = GameObject.FindGameObjectWithTag(_targetTag);
        StartCoroutine(LookForTargets());
    }

    public void SetFOV(float radius, float angle)
    {
        _currentRadius = radius;
        _currentAngle = angle;
    }

    public void ResetFOV()
    {
        _currentRadius = _radius;
        _currentAngle = _angle;
    }

    // TODO - should this be "while(true)"? Does this routine run no matter how far away the GO is from the player?
    private IEnumerator LookForTargets()
    {
        WaitForSeconds wait = new WaitForSeconds(_checkInterval);

        while (true)
        {
            yield return wait;
            CheckFOV();
        }

        void CheckFOV()
        {
            // Finds all objects within the OverlapShere that are within the _targetMask layer(s).
            Collider[] rangeChecks = Physics.OverlapSphere(_origin.position, _currentRadius, _targetMask);

            // TODO - Only acknowledges the first target in the array. Has no way to swap between targets.
            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - _origin.position).normalized; // .normalized caps the vector's length

                // if Target is within the viewing angle
                if (Vector3.Angle(_origin.forward, directionToTarget) < _currentAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(_origin.position, target.position);

                    // If the raycast does not hit the obstruction mask
                    // TODO - Needs additional raycasts for Top, Bottom, Sides, of target
                    if (!Physics.Raycast(_origin.position, directionToTarget, distanceToTarget, _obstructionMask))
                    {
                        if (!_canSeeTarget)
                            TargetSpotted?.Invoke();

                        _canSeeTarget = true;
                        return;
                    }
                }
            }
            _canSeeTarget = false;
        }
    }
}