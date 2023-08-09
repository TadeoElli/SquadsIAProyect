using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerIdleState : IState
{
    Follower _follower;

    FSM<FollowerStates> _fsm;


    public FollowerIdleState(FSM<FollowerStates> fsm, Follower follower)
    {
        _follower = follower;

        _fsm = fsm;


    }

    public void OnEnter()
    {
        if(!_follower.InLineOfSight(_follower.leader.position))
            _fsm.ChangeState(FollowerStates.Search);
        else
            _fsm.ChangeState(FollowerStates.Arrive); 
    }

    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
    {
        
    }

    public void OnExit()
    {

    }


}
