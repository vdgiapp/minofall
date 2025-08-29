using System;
using UnityEngine;

namespace Minofall
{
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

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Update()
        {
            // TODO: Switch to new Unity InputSystem
            if (Input.GetKeyDown(KeyCode.D)) OnMoveRight?.Invoke();
            if (Input.GetKeyDown(KeyCode.A)) OnMoveLeft?.Invoke();
            if (Input.GetKeyDown(KeyCode.E)) OnRotateRight?.Invoke();
            if (Input.GetKeyDown(KeyCode.Q)) OnRotateLeft?.Invoke();
            if (Input.GetKey(KeyCode.S)) OnSoftDrop?.Invoke();
            if (Input.GetKeyDown(KeyCode.Space)) OnHardDrop?.Invoke();
            if (Input.GetKeyDown(KeyCode.W)) OnHold?.Invoke();
        }
    }
}