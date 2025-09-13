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

        public GInputActions inputActions { get; private set; }

        private GInputActions.KeyboardActions _keyboardActions;
        private GInputActions.TouchActions _touchActions;

        private void Awake()
        {
            InstanceInit();
            inputActions = new GInputActions();
            //
            _keyboardActions = inputActions.Keyboard;
            _touchActions = inputActions.Touch;
        }

        private void Start()
        {
            // Keyboard
            _keyboardActions.MoveLeft.performed += RaiseMoveLeft;
            _keyboardActions.MoveRight.performed += RaiseMoveRight;
            _keyboardActions.RotateLeft.performed += RaiseRotateLeft;
            _keyboardActions.RotateRight.performed += RaiseRotateRight;
            _keyboardActions.SoftDrop.performed += RaiseSoftDrop;
            _keyboardActions.HardDrop.performed += RaiseHardDrop;
            _keyboardActions.Hold.performed += RaiseHold;

            // Mobile
            _touchActions.PrimaryContact.started += OnTouchStarted;
            _touchActions.PrimaryContact.canceled += OnTouchEnded;
            _touchActions.PrimaryPosition.performed += OnPositionPerformed;
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
            // Keyboard
            _keyboardActions.MoveLeft.performed -= RaiseMoveLeft;
            _keyboardActions.MoveRight.performed -= RaiseMoveRight;
            _keyboardActions.RotateLeft.performed -= RaiseRotateLeft;
            _keyboardActions.RotateRight.performed -= RaiseRotateRight;
            _keyboardActions.SoftDrop.performed -= RaiseSoftDrop;
            _keyboardActions.HardDrop.performed -= RaiseHardDrop;
            _keyboardActions.Hold.performed -= RaiseHold;

            // Mobile
            _touchActions.PrimaryContact.started -= OnTouchStarted;
            _touchActions.PrimaryContact.canceled -= OnTouchEnded;
            _touchActions.PrimaryPosition.performed -= OnPositionPerformed;
        }

        public void EnableMainGameActions()
        {
            _keyboardActions.Enable();
            _touchActions.Enable();
        }

        public void DisableMainGameActions()
        {
            _keyboardActions.Disable();
            _touchActions.Disable();
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

        private void RaiseMoveLeft(InputAction.CallbackContext context) => OnMoveLeft?.Invoke();
        private void RaiseMoveRight(InputAction.CallbackContext context) => OnMoveRight?.Invoke();
        private void RaiseRotateLeft(InputAction.CallbackContext context) => OnRotateLeft?.Invoke();
        private void RaiseRotateRight(InputAction.CallbackContext context) => OnRotateRight?.Invoke();
        private void RaiseSoftDrop(InputAction.CallbackContext context) => OnSoftDrop?.Invoke();
        private void RaiseHardDrop(InputAction.CallbackContext context) => OnHardDrop?.Invoke();
        private void RaiseHold(InputAction.CallbackContext context) => OnHold?.Invoke();

        private void OnTouchStarted(InputAction.CallbackContext ctx)
        {
            
        }

        private void OnTouchEnded(InputAction.CallbackContext ctx)
        {
            
        }

        private void OnPositionPerformed(InputAction.CallbackContext ctx)
        {
            
        }
    }
}