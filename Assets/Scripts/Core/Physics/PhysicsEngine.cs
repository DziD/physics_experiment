using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsEngine : MonoBehaviour
{
    private const float BigG =  6.673e-11f; // [m^3 s^-2 kg^-1]

    public float Mass;
    public Vector3 VelocityVector; // average Velocity for FixedUpdate
    public Vector3 NetForceVector;

    [SerializeField]
    private List<Vector3> _forcesVectorList = new List<Vector3>();

    public PhysicsEngine[] PhysicsEngineArray;

    public void AddForce(Vector3 forceVector)
    {
        _forcesVectorList.Add(forceVector);
    }

    private void Start()
    {
        InitializeThrustTrails();
        PhysicsEngineArray = FindObjectsOfType<PhysicsEngine>();
    }
    
    private void FixedUpdate()
    {
        PhysicsUpdate(Time.deltaTime);
    }

    private void PhysicsUpdate(float deltaTime)
    {        
        UpdateThrustTrails();
        CalculateGravity();
        UpdatePosition(deltaTime);               
    }

    private void CalculateGravity()
    {
        foreach(var physicsEngineA in PhysicsEngineArray)
        {
            foreach (var physicsEngineB in PhysicsEngineArray)
            {
                if (physicsEngineA == physicsEngineB || physicsEngineA == this)
                    continue;

                var offset = physicsEngineA.transform.position - physicsEngineB.transform.position;
                var rSquared = offset.sqrMagnitude;
                var gravityMagnitude = BigG * physicsEngineA.Mass * physicsEngineB.Mass / rSquared;
                var gravityFeltVector = gravityMagnitude * offset.normalized;

                physicsEngineA.AddForce(-gravityFeltVector);
            }
        }
    }

    private void UpdatePosition(float deltaTime)
    {
        // sum the forces (compute net force) and clear list
        NetForceVector = Vector3.zero;
        for (var i = 0; i < _forcesVectorList.Count; i++)
        {
            NetForceVector += _forcesVectorList[i];
        }
        _forcesVectorList.Clear();

        // calculate position change due to net force
        var acceleration = NetForceVector / Mass;
        VelocityVector += acceleration * deltaTime;
        transform.position += VelocityVector * deltaTime;
    }

    #region Draw Trails
    public bool ShowTrails = false;    
    private LineRenderer _lineRenderer = null;

    private int _numberOfForces = 0;

    private void InitializeThrustTrails()
    {
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.startColor = Color.yellow;
        _lineRenderer.endColor = Color.yellow;
        _lineRenderer.startWidth = 0.2f;
        _lineRenderer.endWidth = 0.2f;
        _lineRenderer.useWorldSpace = false;
    }

    // Update is called once per frame
    private void UpdateThrustTrails()
    {
        if (ShowTrails)
        {
            _lineRenderer.enabled = true;
            _numberOfForces = _forcesVectorList.Count;
            _lineRenderer.positionCount = _numberOfForces * 2;
            for (int j = 0, i = 0; i < _numberOfForces; i += 1, j += 2)
            {
                var forceVector = _forcesVectorList[i];
                _lineRenderer.SetPosition(j, Vector3.zero);
                _lineRenderer.SetPosition(j + 1, -forceVector);
            }
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }
    #endregion
}
