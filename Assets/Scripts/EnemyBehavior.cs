using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public EnemyDescription m_Description;

    public Transform m_Target; //currently only player
    public int m_WeaponIdx = 0;
    public WeaponDescription m_CurrentWeapon { get { return m_Description.m_Attacks[m_WeaponIdx]; } }
    public WeaponComponent m_WeaponComponent;

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
        m_WeaponComponent.SwitchWeapon(m_CurrentWeapon);
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
        if ((m_Target.position - transform.position).sqrMagnitude <= m_CurrentWeapon.m_SqDistance)
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
        if ((m_Target.position - transform.position).sqrMagnitude > m_Description.m_Attacks[0].m_SqDistance)
        {
            m_State = BehaviorState.MOVE;
            m_WeaponIdx = 0;
            return;
        }

        if (m_WeaponIdx >= 0 && m_WeaponIdx < m_Description.m_Attacks.Length) {
            if ((m_Target.position - transform.position).sqrMagnitude <= m_CurrentWeapon.m_SqDistance)
            {
                m_WeaponComponent.SwitchWeapon(m_CurrentWeapon);
                m_WeaponIdx++;
            }
            else
            {
                m_WeaponComponent.SwitchWeapon(m_Description.m_Attacks[m_WeaponIdx-1]);
                m_WeaponIdx--;
            }
        }
        m_WeaponComponent.Attack( ( (Vector2)(m_Target.position - transform.position) ).normalized );
    }
}
