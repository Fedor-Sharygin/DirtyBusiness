using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class JanitorController : MonoBehaviour
{
    public static float Interpolate(float p_Value, float p_MinInput, float p_MaxInput, float p_MinOutput, float p_MaxOutput)
    {
        p_Value = Mathf.Clamp(p_Value, p_MinInput, p_MaxInput);
        return (p_Value - p_MinInput) / (p_MaxInput - p_MinInput) * (p_MaxOutput - p_MinOutput) + p_MinOutput;
    }


    [SerializeField]
    private WeaponComponent m_WeaponComponent;
    private PlayerControls m_PlayerControls;
    private void Awake()
    {
        m_PlayerControls = new PlayerControls();
        if (m_PlayerControls == null)
        {
            Debug.LogError($"{name}: Could not load Player Controls. Quitting the game!");
            Application.Quit();
            return;
        }
        m_PlayerControls.Enable();
        m_PlayerControls.BasicPlayerControls.Enable();

        m_JanitorMovement   = m_PlayerControls.BasicPlayerControls.Movement;
        m_CameraMovement    = m_PlayerControls.BasicPlayerControls.CameraControl;
        m_Attack            = m_PlayerControls.BasicPlayerControls.Attack;
        m_Interact          = m_PlayerControls.BasicPlayerControls.Interact;
    }

    #region Inputs Activation
    private InputAction m_JanitorMovement;
    private InputAction m_CameraMovement;
    private InputAction m_Attack;
    private InputAction m_Interact;
    private void EnableInputs()
    {
        m_JanitorMovement.Enable();
        m_JanitorMovement.performed += JanitorMovement_Perform;
        m_JanitorMovement.canceled  += JanitorMovement_Cancel;

        m_CameraMovement.Enable();
        m_CameraMovement.performed += CameraMovement_Perform;


        m_Attack.Enable();
        m_Attack.performed += Attack;

        m_Interact.Enable();
        m_Interact.performed += Interact;


        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        m_CenterPosition = new Vector2(Screen.width / 2, Screen.height / 2);
    }
    private void DisableInputs()
    {
        m_JanitorMovement.started   -= JanitorMovement_Perform;
        m_JanitorMovement.canceled  -= JanitorMovement_Cancel;
        m_JanitorMovement.Disable();

        m_CameraMovement.performed -= CameraMovement_Perform;
        m_CameraMovement.Disable();


        m_Attack.started -= Attack;
        m_Attack.Disable();

        m_Interact.started -= Interact;
        m_Interact.Disable();


        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    #endregion

    private Vector2 m_CenterPosition;
    private void OnEnable()
    {
        EnableInputs();
    }
    private void OnDisable()
    {
        DisableInputs();
    }
    private void OnDestroy()
    {
        DisableInputs();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    #region Movement
    private Vector3 m_MovementVector = Vector3.zero;
    private void JanitorMovement_Perform(InputAction.CallbackContext p_Obj)
    {
        m_MovementVector = p_Obj.ReadValue<Vector2>();
        if (m_MovementVector.sqrMagnitude > 1)
        {
            m_MovementVector.Normalize();
        }
    }
    private void JanitorMovement_Cancel(InputAction.CallbackContext p_Obj)
    {
        m_MovementVector = Vector3.zero;
    }

    public float m_MouseMaxDist = 100f;
    public float m_CameraMaxDist = 5f;
    private Vector3 m_CameraOffset = new Vector3(0,0,-10);
    private void CameraMovement_Perform(InputAction.CallbackContext p_Obj)
    {
        m_CameraOffset = (p_Obj.ReadValue<Vector2>() - m_CenterPosition) / m_MouseMaxDist;
        if (m_CameraOffset.sqrMagnitude >= 1)
        {
            //Debug.Log("MOUSE TOO FAR!!!!");
            m_CameraOffset = m_CameraOffset.normalized;
            Mouse.current.WarpCursorPosition(m_CenterPosition + new Vector2(m_CameraOffset.x, m_CameraOffset.y) * m_MouseMaxDist);
        }
        m_CameraOffset *= m_CameraMaxDist;
        m_CameraOffset.z = -10;
    }
    private void CameraMovement_Cancel(InputAction.CallbackContext p_Obj)
    {
        throw new System.NotImplementedException();
    }
    #endregion

    public void DamageCameraShake(int p_Amount)
    {
        float Len = Random.Range(.005f, .02f) * p_Amount;
        float Rad = Random.Range(-Mathf.PI, Mathf.PI);
        ShakeCamera(Len, Rad);
    }
    private void ShakeCamera(float p_Length, float p_Angle)
    {
        m_CameraTransform.localPosition = new Vector2(p_Length * Mathf.Cos(p_Angle), p_Length * Mathf.Sin(p_Angle));
    }

    #region Interactions
    [SerializeField]
    private Transform m_CameraTransform;
    [SerializeField]
    private float m_CameraShakeRadiusMin = 1f;
    [SerializeField]
    private float m_CameraShakeRadiusMax = 8f;
    private void Attack(InputAction.CallbackContext p_Obj)
    {
        if (!m_WeaponComponent.Attack( ((Vector2)m_CameraOffset).normalized ))
        {
            return;
        }

        float ShakeRadius = Random.Range(m_WeaponComponent.m_CurrentWeapon.m_MinCameraShake, m_WeaponComponent.m_CurrentWeapon.m_MaxCameraShake);
        float ShakeAngle = Random.Range(-Mathf.PI, Mathf.PI);
        ShakeCamera(ShakeRadius, ShakeAngle);
    }

    [System.Serializable]
    public struct InteractableDist
    {
        public InteractableArea m_Interactable;
        public bool m_InRange;
        public float m_SqDist;
    }
    [SerializeField]
    private InteractableDist[] m_Interactables;
    private void Interact(InputAction.CallbackContext p_Obj)
    {
        InteractableArea ClosestObj = null;
        float MinSqDist = float.MaxValue;
        foreach (var InterDist in m_Interactables)
        {
            if (!InterDist.m_InRange)
            {
                continue;
            }

            if (MinSqDist <= InterDist.m_SqDist)
            {
                continue;
            }
            MinSqDist = InterDist.m_SqDist;
            ClosestObj = InterDist.m_Interactable;
        }

        ClosestObj?.m_InteractionActions?.Invoke(m_WeaponComponent);
    }
    #endregion

    public float m_Speed = 5f;
    [SerializeField]
    private Transform m_CameraHolderTransform;
    private void Update()
    {
        transform.position += m_MovementVector * m_Speed * Time.deltaTime;
        m_CameraHolderTransform.localPosition = m_CameraOffset;

        UpdateInteractableDist();
    }


    private void OnTriggerEnter2D(Collider2D p_Collision)
    {
        if (!p_Collision.CompareTag("Interactable"))
        {
            return;
        }

        var Interactable = p_Collision.GetComponent<InteractableArea>();
        if (Interactable == null)
        {
            return;
        }

        for (int i = 0; i < m_Interactables.Length; ++i)
        {
            if (m_Interactables[i].m_Interactable != Interactable)
            {
                continue;
            }
            m_Interactables[i].m_InRange = true;
        }
    }

    private void UpdateInteractableDist()
    {
        for (int i = 0; i < m_Interactables.Length; ++i)
        {
            if (!m_Interactables[i].m_InRange)
            {
                continue;
            }

            m_Interactables[i].m_SqDist =
                Vector3.SqrMagnitude(transform.position - m_Interactables[i].m_Interactable.transform.position);
        }
    }
}
