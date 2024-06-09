using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public struct DamageReaction
{
    public DamageType m_DamageType;
    public UnityEvent m_Reaction;
}


[CreateAssetMenu(fileName = "Enemy Description", menuName = "Scriptable Objects/Enemy Description", order = 1)]
public class EnemyDescription : ScriptableObject
{
    public float m_Speed;
    public DamageType[] m_Attacks;
}