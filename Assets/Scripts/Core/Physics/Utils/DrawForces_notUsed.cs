using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawForces : MonoBehaviour
{
    public bool ShowTrails = false;

    private List<Vector3> _forceVectorList = new List<Vector3>();
    private LineRenderer _lineRenderer = null;

    private int _numberOfForces = 0;

    private void Start()
    {
        //_forceVectorList = GetComponent<PhysicsEngine>()._forcesVectorList;

        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.startColor = Color.yellow;
        _lineRenderer.endColor = Color.yellow;
        _lineRenderer.startWidth = 0.2f;
        _lineRenderer.endWidth = 0.2f;
        _lineRenderer.useWorldSpace = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (ShowTrails)
        {
            _lineRenderer.enabled = true;
            _numberOfForces = _forceVectorList.Count;
            _lineRenderer.positionCount = _numberOfForces * 2;
            for (int j = 0, i = 0; i < _numberOfForces; i += 1, j += 2)
            {
                var forceVector = _forceVectorList[i];
                _lineRenderer.SetPosition(j, Vector3.zero);
                _lineRenderer.SetPosition(j + 1, -forceVector);
            }
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }
}
