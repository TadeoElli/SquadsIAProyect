using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alignment
{
    Transform _myTransform;

    float _maxSpeed;
    float _maxForce;

    public Alignment(Transform myTransform, float maxSpeed, float maxForce)
    {
        _myTransform = myTransform;
        _maxForce = maxForce;
        _maxSpeed = maxSpeed;
    }

    public Vector3 Execute(Vector3 velocity, GameObject gameObject)
    {
        //Variable donde vamos a recolectar todas las direcciones entre los flockmates
        Vector3 desired = Vector3.zero;

        //Contador para acumular cantidad de followers a promediar
        int count = 0;

        //Por cada follower
        foreach (Follower follower in FollowersManagerTeam1.Instance.AllFollowers)
        {
            if(follower != null)
            {
                //Si soy este follower a chequear, ignoro y sigo la iteracion
                if (follower == gameObject) continue;

                //Saco la direccion hacia el follower
                Vector3 dirToFollower = follower.transform.position - _myTransform.position;

                //Si esta dentro del rango de vision
                if (dirToFollower.sqrMagnitude <= FollowersManagerTeam1.Instance.ViewRadius)
                {
                    //Sumo la direccion hacia donde esta yendo el follower
                    desired += follower._velocity;

                    //Sumo uno mas a mi contador para promediar
                    count++;
                }
            }
        }
        foreach (Follower follower in FollowersManagerTeam2.Instance.AllFollowers)
        {
            if(follower != null)
            {
                //Si soy este follower a chequear, ignoro y sigo la iteracion
                if (follower == gameObject) continue;

                //Saco la direccion hacia el follower
                Vector3 dirToFollower = follower.transform.position - _myTransform.position;

                //Si esta dentro del rango de vision
                if (dirToFollower.sqrMagnitude <= FollowersManagerTeam2.Instance.ViewRadius)
                {
                    //Sumo la direccion hacia donde esta yendo el follower
                    desired += follower._velocity;

                    //Sumo uno mas a mi contador para promediar
                    count++;
                }
            }
        }

        if (count == 0) return desired;

        //Promediamos todas las direcciones
        desired /= count;

        return Vector3.ClampMagnitude((desired.normalized * _maxSpeed) - velocity, _maxForce);
    }
}
