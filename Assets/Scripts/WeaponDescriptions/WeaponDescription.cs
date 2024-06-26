using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Flags]
public enum DamageType
{
    WATER = 1 << 0,
    CLEAN = 1 << 1,
    CRYSTALS = 1 << 2,
    SALT = 1 << 3,
    FIRE = 1 << 4
}


[CreateAssetMenu(fileName = "Weapon Description", menuName = "Scriptable Objects/Weapon Description")]
public class WeaponDescription : ScriptableObject
{
    public DamageType m_DamageType;
    public int m_Damage;

    [Space(5)]
    public float m_SafeDistance;
    public float m_Distance;
    public float m_SqDistance { get { return m_Distance * m_Distance; } }

    public float m_RateOfFire; //time between shots
    public float m_BlastRadius;

    [Space(5)]
    public float m_MinCameraShake;
    public float m_MaxCameraShake;
    public Sprite m_WeaponSprite;
}


