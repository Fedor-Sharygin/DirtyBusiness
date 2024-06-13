using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingRepeatPass : MonoBehaviour
{
    private Renderer m_RepeatMaterialRenderer;
    private Vector3 PrevScale = Vector3.one;

    private void Awake()
    {
        SetScaleInShader();
    }

    private void OnDrawGizmos()
    {
        SetScaleInShader();
    }

    private void Update()
    {
        SetScaleInShader();
    }

    private void SetScaleInShader()
    {
        if (m_RepeatMaterialRenderer is SpriteRenderer)
        {
            m_RepeatMaterialRenderer.sharedMaterial.SetFloat("_Alpha", ((SpriteRenderer)m_RepeatMaterialRenderer).color.a);
        }

        Vector3 CurScale = transform.localScale;
        if (transform.parent != null)
        {
            CurScale.x *= transform.parent.localScale.x;
            CurScale.y *= transform.parent.localScale.y;
            CurScale.z *= transform.parent.localScale.z;
        }
        if (CurScale == PrevScale)
        {
            return;
        }
        if (m_RepeatMaterialRenderer == null)
        {
            m_RepeatMaterialRenderer = GetComponent<Renderer>();
        }

        m_RepeatMaterialRenderer.sharedMaterial.SetVector("_Scale", new Vector4(CurScale.x, CurScale.y));
        PrevScale = CurScale;
    }
}
