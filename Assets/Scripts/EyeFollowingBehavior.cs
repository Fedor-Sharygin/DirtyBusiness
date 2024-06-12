using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeFollowingBehavior : MonoBehaviour
{
    [SerializeField]
    private EnemyBehavior m_Shoggoth;
    private Transform m_PlayerTarget;
    private void Start()
    {
        m_PlayerTarget = m_Shoggoth.m_Target;
        m_ShakeCurveX.Start();
        m_ShakeCurveY.Start();
    }

    [SerializeField]
    private Transform m_MaskTransform;
    private Vector2 m_TargetOffset;
    [SerializeField]
    private float m_TargetDistance = .5f;
    private float m_SqTargetDist { get { return m_TargetDistance * m_TargetDistance; } }
    private void CheckTargetOffset()
    {
        m_TargetOffset = m_MaskTransform.position + m_TargetDistance * (m_PlayerTarget.position - transform.position).normalized;
    }

    [System.Serializable]
    public struct ShakeCurveDescription
    {
        public AnimationCurve m_ShakeCurve;
        public float m_ShakeCurveShift;
        public float m_ShakeCurveLength;
        public float m_CurShakeCurveTime;
        public float m_AmplitudeAdjustion;

        public void Start()
        {
            m_CurShakeCurveTime = m_ShakeCurveShift;
        }
        public void Update()
        {
            m_CurShakeCurveTime += Time.deltaTime;
            if (m_CurShakeCurveTime > m_ShakeCurveLength)
            {
                m_CurShakeCurveTime = 0f;
            }
        }

        public float Evaluate()
        {
            return m_AmplitudeAdjustion * m_ShakeCurve.Evaluate(m_CurShakeCurveTime / m_ShakeCurveLength);
        }
    }
    [SerializeField]
    private ShakeCurveDescription m_ShakeCurveX;
    [SerializeField]
    private ShakeCurveDescription m_ShakeCurveY;
    [SerializeField]
    private float m_LerpPercent = .05f;
    private void Update()
    {
        m_ShakeCurveX.Update();
        m_ShakeCurveY.Update();

        CheckTargetOffset();
        transform.position =
            Vector3.Lerp(transform.position, m_TargetOffset, m_LerpPercent)
            + new Vector3(m_ShakeCurveX.Evaluate(), m_ShakeCurveY.Evaluate());
    }
}
