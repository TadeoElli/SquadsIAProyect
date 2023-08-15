using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerRetreatState : IState
{
    Follower _follower;

    FSM<FollowerStates> _fsm;
    List<Node> _pathToFollow;
    Pathfinding _pathfinding;
    private bool isEvading = false;
    private Transform _obstacle;
    public FollowerRetreatState(FSM<FollowerStates> fsm, Follower follower, List<Node> pathToFollow, Pathfinding pathfinding)
    {
        _follower = follower;

        _fsm = fsm;
        _pathToFollow = pathToFollow;
        _pathfinding = pathfinding;

    }

    public void OnEnter()
    {
        _follower.canShoot = false;
        _follower.isRetreating = true;
        _follower.SetStartingNode();
        //Debug.Log("Search");
        _pathToFollow = _pathfinding.ThetaStar(_follower._startingNode, _follower._baseNode);
    }

    public void OnUpdate()
    {
        FollowPath();
        if(_pathToFollow.Count == 0)
            _fsm.ChangeState(FollowerStates.Idle);
    }

    public void OnFixedUpdate()
    {
        
    }

    public void OnExit()
    {

    }

    void FollowPath()
    {
        if (_pathToFollow.Count == 0) return;
        Vector3 nextPos = _pathToFollow[0].transform.position;

        Collider[] isInRange = Physics.OverlapSphere(_follower.transform.position, 0.5f, _follower.obstacles);
        foreach (var obstacle in isInRange)
        {
            isEvading = true;
            _obstacle =  obstacle.GetComponent<Transform>();
        }

        if(isEvading)
        {
            Evade(_obstacle.position);
        }
        else
        {
            Vector3 dir = nextPos - _follower.transform.position;
            _follower.transform.forward = dir;
            _follower.transform.position += _follower.transform.forward * (_follower._maxSpeed * Time.deltaTime);
            //Debug.Log(_pathToFollow[0]);
            if (dir.magnitude < 1.5f)
            {
                _pathToFollow.RemoveAt(0);
            }
        }


        
    }

    void Evade(Vector3 dist)
    {
        var deltaPosition = Vector3.zero;
        for (int i = 0; i < _follower.numberOfRays; i++)
        {
            var rotation = _follower.transform.rotation;
            var rotationMod = Quaternion.AngleAxis((i / ((float)_follower.numberOfRays - 1)) * _follower.angle * 2 - _follower.angle, _follower.transform.up);
            var direction = rotation * rotationMod * Vector3.forward;
           
            var ray = new Ray(_follower.transform.position, direction);
            RaycastHit hitInfo;
            if(Physics.Raycast(ray, out hitInfo, 2))
                deltaPosition -= (1.0f / _follower.numberOfRays) * _follower._maxSpeed * direction;
            else
                deltaPosition += (1.0f / _follower.numberOfRays) * _follower._maxSpeed * direction;
        }
        _follower.transform.position += deltaPosition * Time.deltaTime;
        if(Vector3.Distance(_follower.transform.position, dist) < 1f)
            isEvading = false;
    }

}
