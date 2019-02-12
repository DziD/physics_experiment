using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    public Vector3 ForceVector;

    private PhysicsEngine _physicsEngine = null;
        
    void Start()
    {
        _physicsEngine = GetComponent<PhysicsEngine>();
    }
    
    void FixedUpdate()
    {
        _physicsEngine.AddForce(ForceVector);
    }
}
