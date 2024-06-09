using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    private float m_TimeLeftBeforeAttack = 0f;
    private void Update()
    {
        if (m_TimeLeftBeforeAttack > 0f)
        {
            m_TimeLeftBeforeAttack -= Time.deltaTime;
        }
    }

    public WeaponDescription m_CurrentWeapon;
    public void Attack(Vector2 p_Direction)
    {
        if (m_TimeLeftBeforeAttack > 0f)
        {
            return;
        }
        m_TimeLeftBeforeAttack = m_CurrentWeapon.m_RateOfFire;

        var ObjectHits = Physics2D.CircleCastAll((Vector2)transform.position + p_Direction * .6f, .5f, p_Direction, m_CurrentWeapon.m_Distance);
        foreach (var ObjectHit in ObjectHits)
        {
            var HPComp = ObjectHit.collider.GetComponent<HealthComponent>();
            if (HPComp == null)
            {
                continue;
            }
            HPComp.ReceieveDamage(m_CurrentWeapon.m_DamageType, m_CurrentWeapon.m_Damage);
        }
    }

    public void SwitchWeapon(WeaponDescription p_NewWeapon) => m_CurrentWeapon = p_NewWeapon;


}
