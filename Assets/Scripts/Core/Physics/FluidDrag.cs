using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidDrag : MonoBehaviour
{
    [Range(1f, 2f)]
    public float VelocityExponent;
    public float DragConstant; //

    private PhysicsEngine _physicsEngine;

    private void Start()
    {
        _physicsEngine = GetComponent<PhysicsEngine>();        
    }

    private void FixedUpdate()
    {
        var velocityVector = _physicsEngine.VelocityVector;
        float speed = velocityVector.magnitude;
        float dragSize = CalculateDrag(speed);
        var dragVector = dragSize * -velocityVector.normalized;

        _physicsEngine.AddForce(dragVector);
    }

    private float CalculateDrag(float speed)
    {
        return DragConstant * Mathf.Pow(speed, VelocityExponent);
    }
}
