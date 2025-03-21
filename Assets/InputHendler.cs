using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Events;

public class InputHendler : MonoBehaviour
{
    private PlayerControls inputActions;
    [SerializeField] private UnityEvent rightClickAction;

    private void Awake()
    {
        inputActions = new PlayerControls();
        inputActions.Player.building.performed += ñtx => rightClickAction?.Invoke();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();
}
