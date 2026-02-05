using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectNahual.Input;
using ProjectNahual.Utils;

namespace ProjectNahual.FPCharacter
{

    public class CameraController : MonoBehaviour
    {
        [Header("Look Settings")]
        [SerializeField] private float lookSensitivity = 2f;
        [SerializeField] private float lookXLimit = 80f;

        private IPlayerInput playerInput;
        private Camera playerCamera;
        private float verticalRotation = 0f;
        private bool canTick = false;

        public void Init(IPlayerInput input)
        {
            playerInput = input;
            playerCamera = Camera.main;
            canTick = true;

            CursorHandler.LockCursor();
        }

        public void Tick()
        {
            if (!canTick) return;
            HandleRotation();
        }

        private void HandleRotation()
        {
            // Get mouse input
            Vector2 lookInput = playerInput.Look;

            // Rotate character horizontally (parent object)
            transform.Rotate(0, lookInput.x * lookSensitivity, 0);

            // Rotate camera vertically
            verticalRotation -= lookInput.y * lookSensitivity;
            verticalRotation = Mathf.Clamp(verticalRotation, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }

        public void Stop() => canTick = false;
    }
}