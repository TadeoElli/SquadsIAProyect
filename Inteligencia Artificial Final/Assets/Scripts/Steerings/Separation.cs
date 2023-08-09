using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Separation
{
    Transform _myTransform;

    float _maxSpeed;
    float _maxForce;

    public Separation(Transform myTransform, float maxSpeed, float maxForce)
    {
        _myTransform = myTransform;
        _maxForce = maxForce;
        _maxSpeed = maxSpeed;
    }

    public Vector3 Execute(Vector3 velocity, GameObject gameObject)
    {
        Vector3 desired = Vector3.zero;

        //Por cada follower
        foreach (Follower follower in FollowersManagerTeam1.Instance.AllFollowers)
        {
            if(follower != null)
            {
                //Si soy este follower a chequear, ignoro y sigo la iteracion
                if (follower == gameObject) continue;

                //Saco la direccion hacia el boid
                Vector3 dirToFollower = follower.transform.position - _myTransform.position;

                //Si esta dentro del rango de vision
                if (dirToFollower.sqrMagnitude <= FollowersManagerTeam1.Instance.ViewRadius)
                {
                    //En este caso me resto porque quiero separarme hacia el lado contrario
                    desired -= dirToFollower;
                }
            }
        }
        foreach (Follower follower in FollowersManagerTeam2.Instance.AllFollowers)
        {
            if(follower != null)
            {
                //Si soy este follower a chequear, ignoro y sigo la iteracion
                if (follower == gameObject) continue;

                //Saco la direccion hacia el boid
                Vector3 dirToFollower = follower.transform.position - _myTransform.position;

                //Si esta dentro del rango de vision
                if (dirToFollower.sqrMagnitude <= FollowersManagerTeam2.Instance.ViewRadius)
                {
                    //En este caso me resto porque quiero separarme hacia el lado contrario
                    desired -= dirToFollower;
                }
            }
        }

        if (desired == Vector3.zero) return desired;

        return Vector3.ClampMagnitude((desired.normalized * _maxSpeed) - velocity, _maxForce);
    }
}
