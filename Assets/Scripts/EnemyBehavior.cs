using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public EnemyDescription m_Description;

    public Transform m_Target; //currently only player
    public WeaponComponent m_WeaponComponent; //for now ONE weapon (REALLY WANT TO MAKE MORE)

    private enum BehaviorState
    {
        MOVE,
        ATTACK,

        DEFAULT
    }
    private BehaviorState m_State;
    private void Start()
    {
        m_State = BehaviorState.MOVE;
    }

    private void Update()
    {
        switch (m_State)
        {
            case BehaviorState.MOVE:
                {
                    Move();
                }
                break;
            case BehaviorState.ATTACK:
                {
                    Attack();
                }
                break;
        }
    }

    private void Move()
    {
        if ((m_Target.position - transform.position).sqrMagnitude <= m_WeaponComponent.m_CurrentWeapon.m_SqDistance)
        {
            m_State = BehaviorState.ATTACK;
            return;
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            m_Target.position,
            m_Description.m_Speed * Time.deltaTime
            );
    }

    private void Attack()
    {
        if ((m_Target.position - transform.position).sqrMagnitude > m_WeaponComponent.m_CurrentWeapon.m_SqDistance)
        {
            m_State = BehaviorState.MOVE;
            return;
        }

        m_WeaponComponent.Attack((m_Target.position - transform.position).normalized);
    }
}
