using UnityEngine;
using ProjectNahual.Input;
using ProjectNahual.Weapons;
using ProjectNahual.Utils;

namespace ProjectNahual.FPCharacter
{
    public class FirstPersonCharacter : MonoBehaviour, IPlayerCharacter
    {
        [Header("References")]
        [SerializeField] private MovementController movementController;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private WeaponController weaponController;
        public IWeapon _weapon;
        private IPlayerInput playerInput;
        private bool initialized;
        public bool Initialized => initialized;
        public Vector3 Position => transform.position;

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
            Registry<IPlayerCharacter>.TryAdd(this);
        }

        private void OnDestroy() => Registry<IPlayerCharacter>.Remove(this); 

        public void Init(MonoBehaviour weaponBehaviour)
        {
            if (!initialized) { return; }

            _weapon = weaponBehaviour as IWeapon;

            if(_weapon == null)
            {
                Debug.LogWarning("IWeapon not found. Add a valid IWeapon instance to the class profiles.");
                return;
            }

            playerInput = GetComponent<StandaloneInputController>();
            movementController.Init(playerInput);
            cameraController.Init(playerInput);
            weaponController.Init(playerInput, _weapon);
        }

        public void SetPosition(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
        }


        private void Update()
        {
            movementController.Tick();
            cameraController.Tick();
        }

        public void Reset()
        {
            movementController.Stop();
            cameraController.Stop();
            weaponController.Stop();
        }
    }
}