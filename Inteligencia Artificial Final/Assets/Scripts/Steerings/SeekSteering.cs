using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekSteering 
{
    Transform _myTransform;
    
    float _maxSpeed;
    float _maxForce;

    public SeekSteering(Transform myTransform, float maxSpeed, float maxForce)
    {
        _myTransform = myTransform;
        _maxSpeed = maxSpeed;
        _maxForce = maxForce;
    }

    public Vector3 Execute(Vector3 currentFollowerVelocity,Vector3 targetPosition)
    {
        //Action-Selection
        Vector3 desired = targetPosition - _myTransform.position;
        desired.Normalize();
        desired *= _maxSpeed;

        //Steering
        Vector3 steering = desired - currentFollowerVelocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce);

        //Locomotion
        return steering;
    }
}
