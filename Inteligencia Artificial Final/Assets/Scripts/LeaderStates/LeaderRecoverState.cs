using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderRecoverState : IState
{
    Leader _leader;

    FSM<LeaderStates> _fsm;

    float timer, recoveryTime;
    public LeaderRecoverState(FSM<LeaderStates> fsm, Leader leader)
    {
        _leader = leader;

        _fsm = fsm;


    }

    public void OnEnter()
    {
        recoveryTime = 3f;
    }

    public void OnUpdate()
    {
        if(_leader.life < _leader.maxLife)
        {
            if(timer > recoveryTime)
            {
                timer = 0;
                _leader.life = _leader.life + 10;
                if(_leader.life >= _leader.maxLife)
                    _leader.life = _leader.maxLife;
            }
            else
                timer = timer + 1 * Time.deltaTime;
        }
        else
            _fsm.ChangeState(LeaderStates.Idle);
        

    }

    public void OnFixedUpdate()
    {
        
    }

    public void OnExit()
    {
        _leader.isRetreating = false;
    }


}
