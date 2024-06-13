using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedObject : MonoBehaviour
{
    public float m_LifeTime = 2f;
    private float m_TimeLeft;
    [SerializeField]
    private SpriteRenderer m_SpriteRenderer;
    void Start()
    {
        m_TimeLeft = m_LifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_TimeLeft <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        m_TimeLeft -= Time.deltaTime;
        var CurColor = m_SpriteRenderer.color;
        CurColor.a = Mathf.Sin(m_TimeLeft / m_LifeTime * Mathf.PI / 2);
        m_SpriteRenderer.color = CurColor;
    }
}
