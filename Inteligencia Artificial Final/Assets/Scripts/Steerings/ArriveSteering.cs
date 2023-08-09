using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArriveSteering 
{
    Transform _myTransform;
    
    float _maxSpeed;
    float _maxForce;
    float _arriveRadius;

    public ArriveSteering(Transform myTransform, float maxSpeed, float maxForce, float arriveRadius)
    {
        _myTransform = myTransform;
        _maxSpeed = maxSpeed;
        _maxForce = maxForce;
        _arriveRadius = arriveRadius;
    }

    public Vector3 Execute(Vector3 currentFollowerVelocity,Transform target)
    {
        Vector3 desired = (target.position - (target.forward * 2f)) - _myTransform.position;

        float desiredMagnitude = desired.magnitude;
        float speed = _maxSpeed;

        desired.Normalize();

        if (desiredMagnitude <= _arriveRadius)
        {
            speed = _maxSpeed * ((desiredMagnitude+1) / _arriveRadius);

        }

        desired *= speed;

        Vector3 steering = desired - currentFollowerVelocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce);

        return steering;
    }
}
