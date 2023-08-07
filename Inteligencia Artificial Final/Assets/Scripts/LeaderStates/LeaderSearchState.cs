using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderSearchState : IState
{
    Leader _leader;

    FSM<LeaderStates> _fsm;

    List<Node> _pathToFollow;
    Pathfinding _pathfinding;

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
        Vector3 dir = nextPos - _leader.transform.position;
        _leader.transform.forward = dir;
        //Collider[] hitColliders = Physics.OverlapSphere(_leader.transform.position, 1f, LayerMask.GetMask("Objects"));

        _leader.transform.position += _leader.transform.forward * (_leader._maxSpeed * Time.deltaTime);
        Debug.Log(_pathToFollow[0]);
        if (dir.magnitude < 0.1f)
        {
            _pathToFollow.RemoveAt(0);
        }

        /*if (_leader.InFieldOfView(_pursuitTarget.transform.position))
        {
            if(!_leader._startSearch)  SetGoalNode(_pathToFollow.Peek(),_leader._allHunters);
            _fsm.ChangeState(HunterStates.Chase);
        }*/
    }

}
