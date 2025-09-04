using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Minofall
{
    // Singleton in Bootstrapper prefab
    // No need to DontDestroyOnLoad
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        // In-game input event
        public event Action OnMoveRight;
        public event Action OnMoveLeft;
        public event Action OnRotateRight;
        public event Action OnRotateLeft;
        public event Action OnSoftDrop;
        public event Action OnHardDrop;
        public event Action OnHold;

        private GameInputActions _inputActions;

        private GameInputActions.IngameActions _ingameActions;

        private void Awake()
        {
            InstanceInit();
            _inputActions = new();
            _ingameActions = _inputActions.Ingame;
        }

        private void Start()
        {
            _ingameActions.MoveLeft.performed += OnIngameActionMoveLeft;
            _ingameActions.MoveRight.performed += OnIngameActionMoveRight;
            _ingameActions.RotateLeft.performed += OnIngameActionRotateLeft;
            _ingameActions.RotateRight.performed += OnIngameActionRotateRight;
            _ingameActions.SoftDrop.performed += OnIngameActionSoftDrop;
            _ingameActions.HardDrop.performed += OnIngameActionHardDrop;
            _ingameActions.Hold.performed += OnIngameActionHold;
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void OnDestroy()
        {
            _ingameActions.MoveLeft.performed -= OnIngameActionMoveLeft;
            _ingameActions.MoveRight.performed -= OnIngameActionMoveRight;
            _ingameActions.RotateLeft.performed -= OnIngameActionRotateLeft;
            _ingameActions.RotateRight.performed -= OnIngameActionRotateRight;
            _ingameActions.SoftDrop.performed -= OnIngameActionSoftDrop;
            _ingameActions.HardDrop.performed -= OnIngameActionHardDrop;
            _ingameActions.Hold.performed -= OnIngameActionHold;
        }

        public void EnableIngameActions()
        {
            _ingameActions.Enable();
        }

        public void DisableIngameActions()
        {
            _ingameActions.Disable();
        }

        private void InstanceInit()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void OnIngameActionMoveLeft(InputAction.CallbackContext context)
        {
            OnMoveLeft?.Invoke();
        }

        private void OnIngameActionMoveRight(InputAction.CallbackContext context)
        {
            OnMoveRight?.Invoke();
        }
        
        private void OnIngameActionRotateLeft(InputAction.CallbackContext context)
        {
            OnRotateLeft?.Invoke();
        }

        private void OnIngameActionRotateRight(InputAction.CallbackContext context)
        {
            OnRotateRight?.Invoke();
        }

        private void OnIngameActionSoftDrop(InputAction.CallbackContext context)
        {
            OnSoftDrop?.Invoke();
        }

        private void OnIngameActionHardDrop(InputAction.CallbackContext context)
        {
            OnHardDrop?.Invoke();
        }

        private void OnIngameActionHold(InputAction.CallbackContext context)
        {
            OnHold?.Invoke();
        }
    }
}