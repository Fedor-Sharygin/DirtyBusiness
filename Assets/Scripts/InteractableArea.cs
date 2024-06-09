using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableArea : MonoBehaviour
{
    public WeaponDescription m_WeaponDescription;
    public UnityEvent<WeaponComponent> m_InteractionActions;
    public void GrantWeapon(WeaponComponent p_WeaponComponent) => p_WeaponComponent.SwitchWeapon(m_WeaponDescription);
}
