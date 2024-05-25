using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class MinigameHolderBase : MonoBehaviour
{
    public ObjectSocket m_LocalSocket;
    private void Awake()
    {
        SceneManager.sceneLoaded += MinigameLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= MinigameLoaded;
    }

    protected virtual void MinigameLoaded(Scene p_MinigameScene, LoadSceneMode _p_LoadSceneMode)
    {
        if (p_MinigameScene.name == "OneAreaNavigationTest")
        {
            return;
        }

        //p_MinigameScene.GetRootGameObjects();
        Debug.LogWarning($"WARNING: {p_MinigameScene.name} LOADED AND TRIGGERED FUNCTION!");
        foreach (var MGHolder in GameObject.FindGameObjectsWithTag("MinigameHolder"))
        {
            MGHolder.GetComponent<MinigameRequiredElement>().m_MinigameClosed += MinigameClosed;
        }
    }
    public abstract void MinigameClosed(MinigameRequiredElement.MinigameStatus p_Status);
}

