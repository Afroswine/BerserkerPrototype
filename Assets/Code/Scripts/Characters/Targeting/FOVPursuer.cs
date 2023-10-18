using System;
using System.Collections;
using UnityEngine;

// TODO - is this script really necessary? FOVTargeter might be the only necessary one
// pursuit sounds more like a state that should be used by the targeter's state machine

public class FOVPursuer : VisualDetection
{
    public event Action PursuitBegan;
    public event Action PursuitEnded;
    
    public float MaxPursuitRadius => _maxPursuitRadius;
    public Vector3 PursuitOrigin => _pursuitOrigin;
    public bool InPursuit => _inPursuit;

    [Header("FOV Pursuer")]
    [Header("Pursuit Area")]
    [SerializeField] float _pursuitRadius = 10f;
    [SerializeField, Range(0, 360)] float _pursuitAngle = 360f;

    [Header("Pursuit End Conditions")]
    [SerializeField] float _maxPursuitRadius = 10f;
    [Tooltip("How long (in seconds) line of sight must be broken to stop pursuing a target.")]
    [SerializeField] float _pursuitConfidence = 3f;
    float _currentConfidence;

    Vector3 _pursuitOrigin; // Stores the transform of this object when it spawns. Used as the origin of the _maxPursuitRadius
    bool _inPursuit = false;

    private void Awake()
    {
        _pursuitOrigin = gameObject.transform.position;

        if (_maxPursuitRadius < CurrentRadius)
            Debug.LogWarning("EnemyTargeting: MaxPursuitRadius is smaller than inherited default radius from Targeting. " +
                "MaxPursuitRadius will be set to CurrentRadius from Targeting.");

        _maxPursuitRadius = Mathf.Max(_maxPursuitRadius, CurrentRadius);
    }

    private void OnEnable()
    {
        base.TargetSpotted += BeginPursuit;
    }

    private void OnDisable()
    {
        base.TargetSpotted -= BeginPursuit;
    }

    private void BeginPursuit()
    {
        if (!_inPursuit)
            StartCoroutine(Pursuit());
    }

    private IEnumerator Pursuit()
    {
        WaitForSeconds wait = new(CheckInterval);
        _inPursuit = true;
        _currentConfidence = _pursuitConfidence;
        PursuitBegan?.Invoke();
        SetFOV(_pursuitRadius, _pursuitAngle);

        while (_inPursuit)
        {
            yield return wait;
            PursuitCheck();
        }

        ResetFOV();
        PursuitEnded?.Invoke();

        void PursuitCheck()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(_pursuitOrigin, _maxPursuitRadius, TargetMask);

            // If the target leaves the max radius...
            if (rangeChecks.Length == 0)
            {
                _inPursuit = false;
                return;
            }

            // if we can see the target...
            if (CanSeeTarget)
            {
                _currentConfidence = _pursuitConfidence;
                return;
            }

            // If we CAN'T see the target, begin deteriorating confidence
            _currentConfidence -= Time.deltaTime + CheckInterval;
            if (_currentConfidence <= 0)
                _inPursuit = false;
        }
    }
}
