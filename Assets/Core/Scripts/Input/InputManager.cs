using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Minofall
{
    // Singleton in Bootstrapper prefab
    // No need to DontDestroyOnLoad
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        public event Action OnMoveRight;
        public event Action OnMoveLeft;
        public event Action OnRotateRight;
        public event Action OnRotateLeft;
        public event Action OnSoftDrop;
        public event Action OnHardDrop;
        public event Action OnHold;

        public EventSystem eventSystem { get; private set; }
        public GInputActions inputActions { get; private set; }

        private GInputActions.KeyboardActions _keyboardActions;
        private GInputActions.MobileActions _mobileActions;

        private void Awake()
        {
            InstanceInit();
            eventSystem = GetComponentInChildren<EventSystem>();
            inputActions = new GInputActions();
            //
            _keyboardActions = inputActions.Keyboard;
            _mobileActions = inputActions.Mobile;
        }

        private void Start()
        {
            _keyboardActions.MoveLeft.performed += OnMoveLeftPerformed;
            _keyboardActions.MoveRight.performed += OnMoveRightPerformed;
            _keyboardActions.RotateLeft.performed += OnRotateLeftPerformed;
            _keyboardActions.RotateRight.performed += OnRotateRightPerformed;
            _keyboardActions.SoftDrop.performed += OnSoftDropPerformed;
            _keyboardActions.HardDrop.performed += OnHardDropPerformed;
            _keyboardActions.Hold.performed += OnHoldPerformed;
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        private void OnDestroy()
        {
            _keyboardActions.MoveLeft.performed -= OnMoveLeftPerformed;
            _keyboardActions.MoveRight.performed -= OnMoveRightPerformed;
            _keyboardActions.RotateLeft.performed -= OnRotateLeftPerformed;
            _keyboardActions.RotateRight.performed -= OnRotateRightPerformed;
            _keyboardActions.SoftDrop.performed -= OnSoftDropPerformed;
            _keyboardActions.HardDrop.performed -= OnHardDropPerformed;
            _keyboardActions.Hold.performed -= OnHoldPerformed;
        }

        public void EnableKeyboardActions()
        {
            _keyboardActions.Enable();
        }

        public void DisableKeyboardActions()
        {
            _keyboardActions.Disable();
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

        private void OnMoveLeftPerformed(InputAction.CallbackContext context)
        {
            OnMoveLeft?.Invoke();
        }

        private void OnMoveRightPerformed(InputAction.CallbackContext context)
        {
            OnMoveRight?.Invoke();
        }
        
        private void OnRotateLeftPerformed(InputAction.CallbackContext context)
        {
            OnRotateLeft?.Invoke();
        }

        private void OnRotateRightPerformed(InputAction.CallbackContext context)
        {
            OnRotateRight?.Invoke();
        }

        private void OnSoftDropPerformed(InputAction.CallbackContext context)
        {
            OnSoftDrop?.Invoke();
        }

        private void OnHardDropPerformed(InputAction.CallbackContext context)
        {
            OnHardDrop?.Invoke();
        }

        private void OnHoldPerformed(InputAction.CallbackContext context)
        {
            OnHold?.Invoke();
        }
    }
}