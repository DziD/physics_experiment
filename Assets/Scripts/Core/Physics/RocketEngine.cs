using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketEngine : MonoBehaviour
{
    public float FuelMass; // [kg]
    public float MaxThrust; // kN [kg m s^-2]

    [Range(0f, 1f)]
    public float ThrustPercent;

    public Vector3 ThrustUnitVector;

    private PhysicsEngine _physicsEngine;
    private float _currentThrust; // N
        
    private void Start()
    {
        _physicsEngine = GetComponent<PhysicsEngine>();
        _physicsEngine.Mass += FuelMass;
    }
    
    private void FixedUpdate()
    {
        if (FuelMass > FuelThisUpdate(Time.deltaTime))
        {
            FuelMass -= FuelThisUpdate(Time.deltaTime);
            _physicsEngine.Mass -= FuelThisUpdate(Time.deltaTime);
            ExertForce();
        }
        else
        {
            Debug.LogWarning("Out of rocket fuel");
        }
    }

    private float FuelThisUpdate(float deltaTime)
    {
        var exhaustMassFlow = 0.0f;
        var effectiveExhaustVelocity = 4462f; // from wikipedia for liquid oxygen / liquid hydrogen;        

        // thrust = massFlow * exhaustVelocity

        exhaustMassFlow = _currentThrust / effectiveExhaustVelocity;

        return exhaustMassFlow * deltaTime;
    }

    private void ExertForce()
    {
        _currentThrust = ThrustPercent * MaxThrust * 1000f;
        var thrustVector = ThrustUnitVector.normalized * _currentThrust;
        _physicsEngine.AddForce(thrustVector);
    }

}
