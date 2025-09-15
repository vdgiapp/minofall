using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Minofall
{
    /// <summary>
    /// Là một singleton, quản lý đầu vào của người chơi. Sử dụng Input System mới của Unity.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance
        { get; private set; }

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
            // Singleton init
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            // Input actions
            inputActions = new GInputActions();

            // Get action maps
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
            _touchActions.PrimaryContact.performed += OnTouchPerformed;
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
            _touchActions.PrimaryContact.performed -= OnTouchPerformed;
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

        private void RaiseMoveLeft(InputAction.CallbackContext context) => OnMoveLeft?.Invoke();
        private void RaiseMoveRight(InputAction.CallbackContext context) => OnMoveRight?.Invoke();
        private void RaiseRotateLeft(InputAction.CallbackContext context) => OnRotateLeft?.Invoke();
        private void RaiseRotateRight(InputAction.CallbackContext context) => OnRotateRight?.Invoke();
        private void RaiseSoftDrop(InputAction.CallbackContext context) => OnSoftDrop?.Invoke();
        private void RaiseHardDrop(InputAction.CallbackContext context) => OnHardDrop?.Invoke();
        private void RaiseHold(InputAction.CallbackContext context) => OnHold?.Invoke();

        // Called when touch (or pointer press) begins
        private void OnTouchStarted(InputAction.CallbackContext ctx)
        {
            Debug.Log("Touch started");
        }

        // Called when touch (or pointer press) ends
        private void OnTouchEnded(InputAction.CallbackContext ctx)
        {
            Debug.Log("Touch ended");
        }

        // Called when touch (or pointer press) ends
        private void OnTouchPerformed(InputAction.CallbackContext ctx)
        {
            Debug.Log("Touch performed");
        }

        // Called continuously while pointer/mouse/finger moves
        private void OnPositionPerformed(InputAction.CallbackContext ctx)
        {
            Debug.Log("Touch position: " + ctx.ReadValue<Vector2>());
        }
    }
}