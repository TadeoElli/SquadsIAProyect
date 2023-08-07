using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderSearchState : IState
{
    Leader _leader;

    FSM<LeaderStates> _fsm;

    List<Node> _pathToFollow;
    Pathfinding _pathfinding;

    private bool isEvading = false;
    private Transform _obstacle;
    public LeaderSearchState(FSM<LeaderStates> fsm, Leader leader, List<Node> pathToFollow, Pathfinding pathfinding)
    {
        _leader = leader;

        _fsm = fsm;
        _pathToFollow = pathToFollow;
        _pathfinding = pathfinding;

    }

    public void OnEnter()
    {
        _pathToFollow = _pathfinding.ThetaStar(_leader._startingNode, _leader._goalNode);
        
    }

    public void OnUpdate()
    {
        FollowPath();
        if(_pathToFollow.Count == 0)
        {
            _fsm.ChangeState(LeaderStates.Idle);
        }
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

        Collider[] isInRange = Physics.OverlapSphere(_leader.transform.position, 0.5f, _leader.obstacles);
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
            Vector3 dir = nextPos - _leader.transform.position;
            _leader.transform.forward = dir;
            _leader.transform.position += _leader.transform.forward * (_leader._maxSpeed * Time.deltaTime);
            //Debug.Log(_pathToFollow[0]);
            if (dir.magnitude < 1.5f)
            {
                _pathToFollow.RemoveAt(0);
            }
        }


        /*if (_leader.InFieldOfView(_pursuitTarget.transform.position))
        {
            if(!_leader._startSearch)  SetGoalNode(_pathToFollow.Peek(),_leader._allHunters);
            _fsm.ChangeState(HunterStates.Chase);
        }*/
    }

    void Evade(Vector3 dist)
    {
        var deltaPosition = Vector3.zero;
        for (int i = 0; i < _leader.numberOfRays; i++)
        {
            var rotation = _leader.transform.rotation;
            var rotationMod = Quaternion.AngleAxis((i / ((float)_leader.numberOfRays - 1)) * _leader.angle * 2 - _leader.angle, _leader.transform.up);
            var direction = rotation * rotationMod * Vector3.forward;
           
            var ray = new Ray(_leader.transform.position, direction);
            RaycastHit hitInfo;
            if(Physics.Raycast(ray, out hitInfo, 2))
                deltaPosition -= (1.0f / _leader.numberOfRays) * _leader._maxSpeed * direction;
            else
                deltaPosition += (1.0f / _leader.numberOfRays) * _leader._maxSpeed * direction;
        }
        _leader.transform.position += deltaPosition * Time.deltaTime;
        if(Vector3.Distance(_leader.transform.position, dist) < 1f)
            isEvading = false;
    }

}
