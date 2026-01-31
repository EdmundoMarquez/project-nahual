using UnityEngine;
using ProjectNahual.Input;

namespace ProjectNahual.FPCharacter
{
    public class FirstPersonCharacter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MovementController movementController;
        [SerializeField] private CameraController cameraController;
        private IPlayerInput playerInput;
        private bool initialized;

        private void Awake()
        {
            // If components don't exist, try to find them on this GameObject
            if (movementController == null)
            {
                movementController = GetComponent<MovementController>();
            }

            if (cameraController == null)
            {
                cameraController = GetComponentInChildren<CameraController>();
            }

            // If still null, log warning
            if (movementController == null)
            {
                Debug.LogWarning("MovementController not found. Add MovementController component to this GameObject.");
                return;
            }

            if (cameraController == null)
            {
                Debug.LogWarning("CameraController not found. Add CameraController component to a child GameObject.");
                return;
            }

            initialized = true;
        }

        private void Start()
        {
            if (!initialized) { return; }

            playerInput = GetComponent<StandaloneInputController>();
            movementController.Init(playerInput);
            cameraController.Init(playerInput);
        }


        private void Update()
        {
            movementController.Tick();
            cameraController.Tick();
        }

        private void LateUpdate()
        {
        }
    }
}