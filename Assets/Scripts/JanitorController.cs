using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JanitorController : MonoBehaviour
{
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
        m_Attack.started += Attack;

        m_Interact.Enable();
        m_Interact.started += Interact;
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
    }

    private Vector2 m_CenterPosition;
    private void OnEnable()
    {
        EnableInputs();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        m_CenterPosition = new Vector2(Screen.width / 2, Screen.height / 2);
    }
    private void OnDisable()
    {
        DisableInputs();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void OnDestroy()
    {
        DisableInputs();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    #region Movement
    private Vector3 m_MovementVector = Vector3.zero;
    private void JanitorMovement_Perform(InputAction.CallbackContext obj)
    {
        m_MovementVector = obj.ReadValue<Vector2>();
        if (m_MovementVector.sqrMagnitude > 1)
        {
            m_MovementVector.Normalize();
        }
    }
    private void JanitorMovement_Cancel(InputAction.CallbackContext obj)
    {
        m_MovementVector = Vector3.zero;
    }

    public float m_MouseMaxDist = 100f;
    public float m_CameraMaxDist = 5f;
    private Vector3 m_CameraOffset = new Vector3(0,0,-10);
    private void CameraMovement_Perform(InputAction.CallbackContext obj)
    {
        m_CameraOffset = (obj.ReadValue<Vector2>() - m_CenterPosition) / m_MouseMaxDist;
        if (m_CameraOffset.sqrMagnitude >= 1)
        {
            //Debug.Log("MOUSE TOO FAR!!!!");
            m_CameraOffset = m_CameraOffset.normalized;
        }
        m_CameraOffset *= m_CameraMaxDist;
        m_CameraOffset.z = -10;
    }
    private void CameraMovement_Cancel(InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region Interactions
    private void Attack(InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }
    #endregion

    public float m_Speed = 5f;
    [SerializeField]
    private Transform m_CameraTransform;
    private void Update()
    {
        transform.position += m_MovementVector * m_Speed * Time.deltaTime;
        m_CameraTransform.localPosition = m_CameraOffset;
    }
}
