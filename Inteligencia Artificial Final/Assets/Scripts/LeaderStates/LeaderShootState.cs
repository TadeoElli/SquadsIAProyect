using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderShootState : IState
{
    Leader _leader;

    FSM<LeaderStates> _fsm;


    private Transform _obstacle, _wall;
    public LeaderShootState(FSM<LeaderStates> fsm, Leader leader)
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
