using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDamage : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] m_SpriteRenderers;
    private Color[] m_Original;
    private void Start()
    {
        m_Original = new Color[m_SpriteRenderers.Length];
        for (int i = 0; i < m_SpriteRenderers.Length; ++i)
        {
            m_Original[i] = m_SpriteRenderers[i].color;
        }
    }

    public void DamageReceived()
    {
        foreach (var SR in m_SpriteRenderers)
        {
            SR.color *= Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < m_SpriteRenderers.Length; ++i)
        {
            m_SpriteRenderers[i].color = Color.Lerp(m_SpriteRenderers[i].color, m_Original[i], .02f);
        }
    }
}
