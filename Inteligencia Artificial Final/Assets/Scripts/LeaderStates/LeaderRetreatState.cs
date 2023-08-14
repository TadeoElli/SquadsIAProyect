using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderRetreatState : IState
{
    Leader _leader;

    FSM<LeaderStates> _fsm;

    List<Node> _pathToFollow;
    Pathfinding _pathfinding;

    private Transform _obstacle, _wall;
    public LeaderRetreatState(FSM<LeaderStates> fsm, Leader leader, List<Node> pathToFollow, Pathfinding pathfinding)
    {
        _leader = leader;

        _fsm = fsm;
        _pathToFollow = pathToFollow;
        _pathfinding = pathfinding;

    }

    public void OnEnter()
    {
        _leader.SetStartingNode();
        _pathToFollow = _pathfinding.ThetaStar(_leader._startingNode, _leader._baseNode);
        _leader.isRetreating = true;
    }

    public void OnUpdate()
    {
        FollowPath();
        if(_pathToFollow.Count == 0)
        {
            _fsm.ChangeState(LeaderStates.Recovering);
        }
    }

    public void OnFixedUpdate()
    {
        Collider[] isInRange = Physics.OverlapSphere(_leader.transform.position, 0.5f, _leader.obstacles);
        foreach (var obstacle in isInRange)
        {
            _leader.isEvadingObstacles = true;
            _obstacle =  obstacle.GetComponent<Transform>();
        }
        isInRange = Physics.OverlapSphere(_leader.transform.position, 0.5f, _leader.walls);
        foreach (var wall in isInRange)
        {
            _leader.isEvadingWalls = true;
            _wall =  wall.GetComponent<Transform>();
        }
        if(_leader.isEvadingObstacles)
            if(_obstacle != null)
                _leader.EvadeObstacles(_obstacle.position);
        if(_leader.isEvadingWalls)
            if(_wall != null)
                _leader.EvadeWalls(_wall.position);
    }

    public void OnExit()
    {

    }
    void FollowPath()
    {
        if (_pathToFollow.Count == 0) return;
        Vector3 nextPos = _pathToFollow[0].transform.position;
        Vector3 dir = nextPos - _leader.transform.position;
        _leader.transform.forward = dir;
        _leader.transform.position += _leader.transform.forward * (_leader._maxSpeed * Time.deltaTime);
        //Debug.Log(_pathToFollow[0]);
        if (dir.magnitude < 1.5f)
        {
            _pathToFollow.RemoveAt(0);
        }
    }



}
