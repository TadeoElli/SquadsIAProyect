using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderShootState : IState
{
    Leader _leader;

    FSM<LeaderStates> _fsm;
    float minDistance = 10;

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
        foreach (var enemies in _leader.enemiesFollowers)
        {
            if(Vector3.Distance(_leader.transform.position, enemies.transform.position) < minDistance)
            {
                minDistance = Vector3.Distance(_leader.transform.position, enemies.transform.position);
                _leader.transform.forward = enemies.transform.position - _leader.transform.position;
            }
        }
    }

    public void OnFixedUpdate()
    {
        if(_leader.isBulletCooldown)
            _leader.ShootCooldown();
        else
            _leader.Shoot();

        if(_leader.life <= 20)
            _fsm.ChangeState(LeaderStates.Retreat);
    }

    public void OnExit()
    {
        minDistance = 10;
    }



}
