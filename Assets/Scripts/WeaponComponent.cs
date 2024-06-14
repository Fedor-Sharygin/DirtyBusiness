using System;
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

    [SerializeField]
    private Animator m_AttackAnimator;
    [SerializeField]
    private string m_Trigger;
    public WeaponDescription m_CurrentWeapon;
    public bool Attack(Vector2 p_Direction)
    {
        if (m_CurrentWeapon == null || m_TimeLeftBeforeAttack > 0f)
        {
            return false;
        }
        m_TimeLeftBeforeAttack = m_CurrentWeapon.m_RateOfFire;
        Vector2 Origin = (Vector2)transform.position + p_Direction * m_CurrentWeapon.m_SafeDistance;

        var ObjectHits = Physics2D.CircleCastAll(
            Origin,
            m_CurrentWeapon.m_BlastRadius,
            p_Direction,
            m_CurrentWeapon.m_Distance
            );
        DrawCircleCast(
            Origin,
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

        m_AttackAnimator?.SetTrigger(m_Trigger);
        DrawAttackPath(
            Origin,
            p_Direction,
            m_CurrentWeapon.m_BlastRadius,
            m_CurrentWeapon.m_Distance
            );

        return true;
    }

    [SerializeField]
    private GameObject m_PathPrefab;
    [SerializeField]
    private GameObject m_EndPointPrefab;
    private void DrawAttackPath(Vector2 p_Origin, Vector2 p_Direction, float p_Radius, float p_Distance)
    {
        if (m_PathPrefab == null || m_EndPointPrefab == null)
        {
            return;
        }

        Vector2 ToVector = new Vector2(1, 0);
        float ang = Vector2.Angle(p_Direction, ToVector);
        Vector3 cross = Vector3.Cross(p_Direction, ToVector);
        if (cross.z > 0)
        {
            ang = -ang;
        }
        var SplashRotation = Quaternion.Euler(0, 0, ang - 90);

        var OriginSplash = Instantiate(
            m_EndPointPrefab,
            p_Origin,
            SplashRotation
            );
        OriginSplash.transform.localScale = Vector3.one * p_Radius * 1.2f;

        var EndSplash = Instantiate(
            m_EndPointPrefab,
            p_Origin + p_Direction * p_Distance,
            SplashRotation
            );
        EndSplash.transform.localScale = Vector3.one * p_Radius * 1.2f;

        var PathSplash = Instantiate(
            m_PathPrefab,
            p_Origin,
            SplashRotation
            );
        PathSplash.transform.localScale = new Vector3(p_Radius, p_Distance * 2, 1f);
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
