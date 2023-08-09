using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerArriveState : IState
{
    Follower _follower;

    FSM<FollowerStates> _fsm;
    private Transform _obstacle, _wall;
    public FollowerArriveState(FSM<FollowerStates> fsm, Follower follower)
    {
        _follower = follower;

        _fsm = fsm;


    }

    public void OnEnter()
    {
        Debug.Log("Arrive");
    }

    public void OnUpdate()
    {
        _follower.transform.position += _follower._velocity * Time.deltaTime;

        _follower.transform.forward = _follower._velocity;

        if(Vector3.Distance(_follower.transform.position, _follower.leader.position) > _follower._distanceToLeader)
            _follower.AddForce(_follower._myArriveSteering.Execute(_follower._velocity, _follower.leader));
        else
        {
            if(_follower.gameObject.tag == "Team1")
                _follower.AddForce(_follower._mySeparationSteering.Execute(_follower._velocity,_follower.gameObject) * FollowersManagerTeam1.Instance.SeparationWeight +
                        _follower._myAlignmentSteering.Execute(_follower._velocity,_follower.gameObject) * FollowersManagerTeam1.Instance.AlignmentWeight +
                        _follower._myCohesionSteering.Execute(_follower._velocity,_follower.gameObject) * FollowersManagerTeam1.Instance.CohesionWeight);
            if(_follower.gameObject.tag == "Team2")
                _follower.AddForce(_follower._mySeparationSteering.Execute(_follower._velocity,_follower.gameObject) * FollowersManagerTeam2.Instance.SeparationWeight +
                        _follower._myAlignmentSteering.Execute(_follower._velocity,_follower.gameObject) * FollowersManagerTeam2.Instance.AlignmentWeight +
                        _follower._myCohesionSteering.Execute(_follower._velocity,_follower.gameObject) * FollowersManagerTeam2.Instance.CohesionWeight);
        }
        
        if(!_follower.InLineOfSight(_follower.leader.position))
            _fsm.ChangeState(FollowerStates.Search);
        /*if (!_follower.InFieldOfView(_follower.leader.transform.position))
        {
            _fsm.ChangeState(FollowerStates.Search);
        }*/
    }

    public void OnFixedUpdate()
    {
        Collider[] isInRange = Physics.OverlapSphere(_follower.transform.position, 0.5f, _follower.obstacles);
        foreach (var obstacle in isInRange)
        {
            _follower.isEvadingObstacles = true;
            _obstacle =  obstacle.GetComponent<Transform>();
        }
        Collider[] isInRange2 = Physics.OverlapSphere(_follower.transform.position, 0.5f, _follower.walls);
        foreach (var wall in isInRange2)
        {
            _follower.isEvadingWalls = true;
            _wall =  wall.GetComponent<Transform>();
        }
        if(_follower.isEvadingObstacles)
            if(_obstacle != null)
                _follower.EvadeObstacles(_obstacle.position);
        if(_follower.isEvadingWalls)
            if(_wall != null)
                _follower.EvadeWalls(_wall.position);
    }

    public void OnExit()
    {
        _follower._velocity = Vector3.zero;
    }


}
