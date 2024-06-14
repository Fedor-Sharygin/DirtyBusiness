using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : MonoBehaviour
{
    private SpriteRenderer m_Sprite;
    private HealthComponent m_HP;
    private void Awake()
    {
        m_Sprite = GetComponent<SpriteRenderer>();
        m_HP = GetComponent<HealthComponent>();
    }

    public void Cleaner()
    {
        var CurColor = m_Sprite.color;
        CurColor.a = m_HP.HealthPercent;
        m_Sprite.color = CurColor;
    }

    public void GetDestroyed()
    {
        Destroy(gameObject);
    }
}
