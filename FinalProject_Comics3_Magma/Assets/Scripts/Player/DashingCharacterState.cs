using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingCharacterState : State
{
    private PlayerController m_Owner;

    private float _dashingPower;
    private float _dashingTime;
    private float _dashingCooldown; //Non utilizzato, coroutine in PlayerController

    private float _timeElapsed;
    Vector2 direction;
    public DashingCharacterState(PlayerController owner)
    {
        m_Owner = owner;
    }

    public override void OnStart()
    {
        Debug.Log("Sono in dashing");

        _timeElapsed = 0;

        _dashingPower = m_Owner.dashingPower;
        _dashingTime = m_Owner.dashingTime;
        _dashingCooldown = m_Owner.dashingCooldown;

        direction = m_Owner.Direction.normalized;

        m_Owner.CanDash = false;
        m_Owner.IsDashing = true;
    }

    public override void OnEnd() //Da interrompere il dash quando va conrto un muro
    {
        //m_Owner.CanDash = true;
        m_Owner.IsDashing = false;
        m_Owner.StartCoroutine(m_Owner.DashCooldownRoutine());
    }

    public override void OnFixedUpdate() 
    {
        m_Owner.Rigidbody.velocity = direction * _dashingPower;
    }

    public override void OnUpdate() //Spostato da Fixed temporaneamente
    {
        
        DashingTimer();
    }

    private void DashingTimer()
    {
        _timeElapsed += Time.deltaTime;

        if (_timeElapsed > _dashingTime)
        {
            m_Owner.StateMachine.SetState(EPlayerState.Idle);
        }
    }
}
