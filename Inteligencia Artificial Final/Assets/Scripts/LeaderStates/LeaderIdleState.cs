using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderIdleState : IState
{
    Leader _leader;

    FSM<LeaderStates> _fsm;


    public LeaderIdleState(FSM<LeaderStates> fsm, Leader leader)
    {
        _leader = leader;

        _fsm = fsm;


    }

    public void OnEnter()
    {

        
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
