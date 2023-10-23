using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AISensor : MonoBehaviour
{
    [Header("AI Sensor")]
    [SerializeField] bool _drawSensor = true;
    [Header("FOV and Position")]
    [SerializeField] Transform _originOfView;
    [SerializeField] float _distance = 10f;
    [SerializeField, Range(0, 180)] float _angle = 30f;
    //[SerializeField] float _height = 1f;
    [SerializeField] float _ceiling = 1f;
    [SerializeField] float _floor = -1f;
    [SerializeField] Color _meshColor = Color.cyan;
    Mesh _mesh;

    [Header("Scanning")]
    [SerializeField] int _scansPerSecond = 10;
    [SerializeField] LayerMask _scanLayers;
    [SerializeField] LayerMask _occlusionLayers;
    Collider[] _colliders = new Collider[50];
    int _count;
    float _scanInterval;
    float _scanTimer;

    List<GameObject> _visiblePoints = new List<GameObject>();   // the visible AI Sensor Points
    List<GameObject> _visibleTargets = new List<GameObject>();  // The parent objects stored in the visible AI Sensor Points
    public List<GameObject> VisiblePoints
    {
        get
        {
            _visiblePoints.RemoveAll(obj => !obj);
            return _visiblePoints;
        }
    }
    public List<GameObject> VisibleTargets
    {
        get
        {
            _visibleTargets.RemoveAll(obj => !obj);
            return _visibleTargets.Distinct().ToList();
        }
    }

    private void Start()
    {
        _scanInterval = 1.0f / _scansPerSecond;
        _ceiling = Mathf.Abs(_ceiling);
        _floor = -Mathf.Abs(_floor);
    }

    private void Update()
    {
        _scanTimer -= Time.deltaTime;
        if(_scanTimer <= 0)
        {
            _scanTimer += _scanInterval;
            Scan();
        }
    }

    // look for game objects with colliders and an AI Sensor Point script
    private void Scan()
    {
        _count = Physics.OverlapSphereNonAlloc(_originOfView.position, _distance, _colliders, _scanLayers, QueryTriggerInteraction.Collide);

        _visiblePoints.Clear();
        _visibleTargets.Clear();
        for(int i = 0; i < _count; i++)
        {
            GameObject obj = _colliders[i].gameObject;
            if (IsInSight(obj) && obj.TryGetComponent<AISensorPoint>(out AISensorPoint point))
            {
                _visiblePoints.Add(obj);
                _visibleTargets.Add(point.ParentObject);
            }
        }
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = _originOfView.position;
        Vector3 destination = obj.transform.position;
        Vector3 direction = destination - origin;

        // if the target is vertically outside of the FOV...
        //if (direction.y < 0 || direction.y > _height)
        //if (direction.y < -(_height/2) || direction.y > (_height/2))
        if (direction.y < _floor || direction.y > _ceiling)
            return false;

        // if the target is horizontally outside of the FOV...
        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, _originOfView.forward);
        if (deltaAngle > _angle)
            return false;

        // if another object is blocking line of sight...
        if (Physics.Linecast(_originOfView.position, destination, _occlusionLayers))
            return false;

        // otherwise, the target must be in sight!
        return true;
    }

    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero + Vector3.up * _floor;  // - Vector3.up * (_height / 2);   //= Vector3.zero;
        Vector3 bottomLeft = (Quaternion.Euler(0, -_angle, 0) * Vector3.forward * _distance) + Vector3.up * _floor; // - Vector3.up * (_height / 2); //= Quaternion.Euler(0, -_angle, 0) * Vector3.forward * _distance;
        Vector3 bottomRight = (Quaternion.Euler(0, _angle, 0) * Vector3.forward * _distance) + Vector3.up * _floor; // - Vector3.up * (_height / 2);//= Quaternion.Euler(0, _angle, 0) * Vector3.forward * _distance;

        Vector3 topCenter = bottomCenter + (Vector3.up * _ceiling) - (Vector3.up * _floor);   // Vector3.up * _height;
        Vector3 topRight = bottomRight + (Vector3.up * _ceiling) - (Vector3.up * _floor);     // Vector3.up * _height;
        Vector3 topLeft = bottomLeft + (Vector3.up * _ceiling) - (Vector3.up * _floor);       // Vector3.up * _height;

        int vert = 0;

        // left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        // right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -_angle;
        float deltaAngle = (_angle * 2) / segments;
        for(int i = 0; i < segments; i++)
        {
            bottomLeft = (Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * _distance) + Vector3.up * _floor;    // - Vector3.up * (_height / 2);//= Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * _distance;
            bottomRight = (Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * _distance) + Vector3.up * _floor;  // - Vector3.up * (_height / 2);//= Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * _distance;

            topRight = bottomRight + (Vector3.up * _ceiling) - (Vector3.up * _floor);   // + Vector3.up * _height;
            topLeft = bottomLeft + (Vector3.up * _ceiling) - (Vector3.up * _floor);     // + Vector3.up * _height;

            // far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            // top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            // bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }

        for(int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        if (_originOfView == null)
            Debug.Log(gameObject.name + "'s 'Origin Of View' has not been set in its AI Sensor script.");

        _scanInterval = 1.0f / _scansPerSecond;
        _ceiling = Mathf.Abs(_ceiling);
        _floor = -Mathf.Abs(_floor);

        if(_drawSensor)
            _mesh = CreateWedgeMesh();
    }

    private void OnDrawGizmos()
    {
        if (_originOfView == null)
            return;

        if (_drawSensor == false)
            return;

        // draws the FOV mesh
        Gizmos.color = _meshColor;
        Gizmos.DrawMesh(_mesh, _originOfView.position, _originOfView.rotation);

        // draws the entire possible FOV range (bounds)
        Gizmos.color = _meshColor;
        Gizmos.DrawWireSphere(_originOfView.position, _distance);

        /*
        // marks colliders that are in bounds with red
        Gizmos.color = Color.red;
        for (int i = 0; i < _count; i++)
        {
            Gizmos.DrawSphere(_colliders[i].transform.position, 0.2f);
        }
        */

        // marks over objects that are in bounds AND 'IsInSight()' with green
        Gizmos.color = Color.green;
        foreach (var obj in VisiblePoints)
        {
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }
    }
}
