using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    private static int m_Day = 0;
    [SerializeField]
    private Timer m_DayTimer;
    [SerializeField]
    private float[] m_DayLength;
    private void Awake()
    {
        if (m_Day == 5)
        {
            SceneManager.LoadScene("EndCutscene");
            return;
        }

        m_DayTimer.TimerModifier = m_DayLength[m_Day];
        ++m_Day;
    }


    [SerializeField]
    private Transform m_Player;
    [SerializeField]
    private GameObject[] m_EnemyTypes; //last one is always dirt
    public void SpawnEnemyRandom()
    {
        var PosAngle = Random.Range(-Mathf.PI, Mathf.PI);
        var Pos = new Vector3(Mathf.Cos(PosAngle), Mathf.Sin(PosAngle)) * 12f;
        SpawnEnemyTypeAtPosition(Random.Range(0, m_EnemyTypes.Length - 1), m_Player.position + Pos);
    }

    public void SpawnEnemyTypeAtPosition(int p_Idx, Vector3 p_Pos)
    {
        if (p_Idx >= m_EnemyTypes.Length)
        {
            return;
        }

        var Enemy = Instantiate(m_EnemyTypes[p_Idx], p_Pos, Quaternion.identity);
        if (Enemy == null)
        {
            return;
        }
        var EnB = Enemy.GetComponent<EnemyBehavior>();
        if (EnB == null)
        {
            return;
        }
        EnB.m_Target = m_Player;
    }
}
