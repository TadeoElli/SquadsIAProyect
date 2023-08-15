using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerIdleState : IState
{
    Follower _follower;

    FSM<FollowerStates> _fsm;
    float timer, recoveryTime;

    public FollowerIdleState(FSM<FollowerStates> fsm, Follower follower)
    {
        _follower = follower;

        _fsm = fsm;


    }

    public void OnEnter()
    {
        recoveryTime = 3f;
    }

    public void OnUpdate()
    {
        if(!_follower.isRetreating)
        {
            if(!_follower.InLineOfSight(_follower.leader.position))
                _fsm.ChangeState(FollowerStates.Search);
            else
                _fsm.ChangeState(FollowerStates.Arrive); 
        }
        else
        {
            if(_follower.life < _follower.maxLife)
            {
                if(timer > recoveryTime)
                {
                    timer = 0;
                    _follower.life = _follower.life + 10;
                    if(_follower.life >= _follower.maxLife)
                        _follower.life = _follower.maxLife;
                }
                else
                    timer = timer + 1 * Time.deltaTime;
            }
            else
                _follower.isRetreating = false;
        }
        
    }

    public void OnFixedUpdate()
    {
        
    }

    public void OnExit()
    {

    }


}
