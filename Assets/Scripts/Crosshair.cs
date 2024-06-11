using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public float m_Period;
    public float m_Amplitude;
    public float m_Shift;
    public float m_Center;
    public float m_RotationSpeed;
    void Update()
    {
        var Scale = m_Amplitude * Mathf.Sin(2 * Mathf.PI / m_Period * (Time.time - m_Shift)) + m_Center;
        transform.localScale = new Vector2(Scale, Scale);
        transform.Rotate(0f, 0f, m_RotationSpeed * Time.deltaTime);
    }
}
