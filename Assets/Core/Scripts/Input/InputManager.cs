using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Minofall
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance
        { get; private set; }

        // Events
        public event Action OnMoveRight;
        public event Action OnMoveLeft;
        public event Action OnRotateRight;
        public event Action OnRotateLeft;
        public event Action OnSoftDrop;
        public event Action OnHardDrop;
        public event Action OnHold;

        public GInputActions InputActions { get; private set; }

        private GInputActions.KeyboardActions _keyboardActions;
        private GInputActions.TouchActions _touchActions;

        // Soft drop flag and timer
        private bool _softDropActive = false;
        private float _softDropDelay = 0.02f;
        private float _softDropTimer = 0f;

        // Touch state
        private bool _isTouching = false;
        private Vector2 _touchStartPosition;
        private Vector2 _lastTouchPosition;
        private float _touchStartTime;

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
            InputActions = new GInputActions();

            // Get action maps
            _keyboardActions = InputActions.Keyboard;
            _touchActions = InputActions.Touch;
        }

        private void Start()
        {
            // Keyboard
            _keyboardActions.MoveLeft.performed += OnKeyboardMoveLeftPerformed;
            _keyboardActions.MoveRight.performed += OnKeyboardMoveRightPerformed;
            _keyboardActions.RotateLeft.performed += OnKeyboardRotateLeftPerformed;
            _keyboardActions.RotateRight.performed += OnKeyboardRotateRightPerformed;
            _keyboardActions.SoftDrop.started += OnKeyboardSoftDropStarted;
            _keyboardActions.SoftDrop.canceled += OnKeyboardSoftDropCanceled;
            _keyboardActions.HardDrop.performed += OnKeyboardHardDropPerformed;
            _keyboardActions.Hold.performed += OnKeyboardHoldPerformed;

            // Touch
            _touchActions.PrimaryContact.started += OnTouchStarted;
            _touchActions.PrimaryContact.canceled += OnTouchEnded;
            _touchActions.PrimaryPosition.performed += OnTouchPositionChanged;
        }

        private void OnEnable()
        {
            InputActions.Enable();
        }

        private void Update()
        {
            // Soft drop timer handle
            if (_softDropActive)
            {
                _softDropTimer += Time.deltaTime;
                if (_softDropTimer >= _softDropDelay)
                {
                    RaiseSoftDrop();
                    _softDropTimer = 0f;
                }
            }
            else
            {
                _softDropTimer = 0f;
            }
        }

        private void OnDisable()
        {
            InputActions.Disable();
        }

        private void OnDestroy()
        {
            // Keyboard
            _keyboardActions.MoveLeft.performed -= OnKeyboardMoveLeftPerformed;
            _keyboardActions.MoveRight.performed -= OnKeyboardMoveRightPerformed;
            _keyboardActions.RotateLeft.performed -= OnKeyboardRotateLeftPerformed;
            _keyboardActions.RotateRight.performed -= OnKeyboardRotateRightPerformed;
            _keyboardActions.SoftDrop.started -= OnKeyboardSoftDropStarted;
            _keyboardActions.SoftDrop.canceled -= OnKeyboardSoftDropCanceled;
            _keyboardActions.HardDrop.performed -= OnKeyboardHardDropPerformed;
            _keyboardActions.Hold.performed -= OnKeyboardHoldPerformed;

            // Touch
            _touchActions.PrimaryContact.started -= OnTouchStarted;
            _touchActions.PrimaryContact.canceled -= OnTouchEnded;
            _touchActions.PrimaryPosition.performed -= OnTouchPositionChanged;
        }

        public void EnableKeyboardInput()
        {
            _keyboardActions.Enable();
        }

        public void EnableTouchInput()
        {
            _touchActions.Enable();
        }

        public void DisableKeyboardInput()
        {
            _keyboardActions.Disable();
        }

        public void DisableTouchInput()
        {
            _touchActions.Disable();
        }

        public void ResetInputState()
        {
            _softDropActive = false;
            _softDropTimer = 0f;
            _isTouching = false;
        }

        // Keyboard action handlers
        private void OnKeyboardMoveLeftPerformed(InputAction.CallbackContext context) => RaiseMoveLeft();
        private void OnKeyboardMoveRightPerformed(InputAction.CallbackContext context) => RaiseMoveRight();
        private void OnKeyboardRotateLeftPerformed(InputAction.CallbackContext context) => RaiseRotateLeft();
        private void OnKeyboardRotateRightPerformed(InputAction.CallbackContext context) => RaiseRotateRight();
        private void OnKeyboardSoftDropStarted(InputAction.CallbackContext context) => _softDropActive = true;
        private void OnKeyboardSoftDropCanceled(InputAction.CallbackContext context) => _softDropActive = false;
        private void OnKeyboardHardDropPerformed(InputAction.CallbackContext context) => RaiseHardDrop();
        private void OnKeyboardHoldPerformed(InputAction.CallbackContext context) => RaiseHold();

        // Called when touch (or pointer press) begins
        private void OnTouchStarted(InputAction.CallbackContext ctx)
        {
            _isTouching = true;
            _touchStartPosition = _touchActions.PrimaryPosition.ReadValue<Vector2>();
            _touchStartTime = Time.time;
        }

        // Called when touch (or pointer press) ends
        private void OnTouchEnded(InputAction.CallbackContext ctx)
        {
            if (!_isTouching) return;
            _isTouching = false;

            Vector2 touchEndPos = _touchActions.PrimaryPosition.ReadValue<Vector2>();
            float touchEndTime = Time.time;

            Vector2 touchDeltaPos = touchEndPos - _touchStartPosition;
            float touchDeltaTime = touchEndTime - _touchStartTime;


        }

        // Called continuously while finger moves
        private void OnTouchPositionChanged(InputAction.CallbackContext ctx)
        {
            
        }

        // Raise event
        private void RaiseMoveLeft() => OnMoveLeft?.Invoke();
        private void RaiseMoveRight() => OnMoveRight?.Invoke();
        private void RaiseRotateLeft() => OnRotateLeft?.Invoke();
        private void RaiseRotateRight() => OnRotateRight?.Invoke();
        private void RaiseSoftDrop() => OnSoftDrop?.Invoke();
        private void RaiseHardDrop() => OnHardDrop?.Invoke();
        private void RaiseHold() => OnHold?.Invoke();
    }
}