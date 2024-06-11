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
    public bool Attack(Vector2 p_Direction)
    {
        if (m_CurrentWeapon == null || m_TimeLeftBeforeAttack > 0f)
        {
            return false;
        }
        m_TimeLeftBeforeAttack = m_CurrentWeapon.m_RateOfFire;

        var ObjectHits = Physics2D.CircleCastAll(
            (Vector2)transform.position + p_Direction * m_CurrentWeapon.m_SafeDistance,
            m_CurrentWeapon.m_BlastRadius,
            p_Direction,
            m_CurrentWeapon.m_Distance
            );
        DrawCircleCast(
            (Vector2)transform.position + p_Direction * m_CurrentWeapon.m_SafeDistance,
            p_Direction,
            m_CurrentWeapon.m_BlastRadius,
            m_CurrentWeapon.m_Distance,
            ObjectHits
            );
        foreach (var ObjectHit in ObjectHits)
        {
            var HPComp = ObjectHit.collider.GetComponentInParent<HealthComponent>();
            if (HPComp == null)
            {
                continue;
            }
            HPComp.ReceieveDamage(m_CurrentWeapon.m_DamageType, m_CurrentWeapon.m_Damage);
        }

        return true;
    }

    public void SwitchWeapon(WeaponDescription p_NewWeapon) => m_CurrentWeapon = p_NewWeapon;

    void DrawCircleCast(Vector2 origin, Vector2 direction, float radius, float distance, RaycastHit2D[] hits)
    {
        // Calculate the end position of the circle cast
        Vector2 endPosition = origin + direction.normalized * distance;

        // Draw the initial and final circles
        DrawCircle(origin, radius, Color.green);
        DrawCircle(endPosition, radius, Color.red);

        // Draw the line representing the direction of the cast
        Debug.DrawLine(origin, endPosition, Color.blue, 1);

        // Draw hit points
        foreach (RaycastHit2D hit in hits)
        {
            Debug.DrawLine(hit.point, hit.point + Vector2.up * 0.1f, Color.yellow, 1);
        }
    }

    void DrawCircle(Vector2 position, float radius, Color color)
    {
        int segments = 20;
        float angle = 0f;
        float increment = 2f * Mathf.PI / segments;

        Vector2 lastPoint = position + new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
        for (int i = 1; i <= segments; i++)
        {
            angle += increment;
            Vector2 nextPoint = position + new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
            Debug.DrawLine(lastPoint, nextPoint, color, 1);
            lastPoint = nextPoint;
        }
    }

}
