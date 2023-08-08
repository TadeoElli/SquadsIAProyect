using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cohesion
{
    Transform _myTransform;

    float _maxSpeed;
    float _maxForce;

    public Cohesion(Transform myTransform, float maxSpeed, float maxForce)
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

        foreach (Follower follower in FollowersManager.Instance.AllFollowers)
        {
            if(follower != null)
            {
                //Si soy este follower a chequear, ignoro y sigo la iteracion
                if (follower == gameObject) continue;

                //Saco la direccion hacia el follower
                Vector3 dirToFollower = follower.transform.position - _myTransform.position;

                //Si esta dentro del rango de vision
                if (dirToFollower.sqrMagnitude <= FollowersManager.Instance.ViewRadius)
                {
                    //Sumo la posicion de cada follower
                    desired += follower.transform.position;

                    //Sumo uno mas a mi contador para promediar
                    count++;
                }
            }
        }

        if (count == 0) return desired;

        //Promediamos todas las posiciones
        desired /= count;

        //Restamos nuestra posicion para que tambien sea parte de la cohesion
        desired -= _myTransform.position;

        return Vector3.ClampMagnitude((desired.normalized * _maxSpeed) - velocity, _maxForce);
    }
}
