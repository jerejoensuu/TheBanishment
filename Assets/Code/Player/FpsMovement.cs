using UnityEngine;

namespace Code.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class FpsMovement : MonoBehaviour
    {
        private CharacterController _charController;
        public Camera headCam;
        [SerializeField] private PlayerStamina stamina;

        public float walkSpeed = 6.0f;
        public float runSpeed = 9f;
        public float sneakSpeed = 1f;
        private float Speed;
        private float crouchTransitionSpeed = 15f;

        public float gravity = -9.8f;

        public float sensitivityHor = 9.0f;
        public float sensitivityVert = 9.0f;

        public float minimumVert = -45.0f;
        public float maximumVert = 45.0f;

        private float rotationVert = 0;

        private Vector3 _movementInput;
        [HideInInspector] public bool running;
        [HideInInspector] public bool sneaking;

        private Vector3 cameraStartPos;

        private void Awake()
        {
            _charController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            MoveCharacter();
            RotateCharacter();
            RotateCamera();
            AdjustCamera();

            if (running)
            {
                Speed = runSpeed;
            } else if (sneaking)
            {
                Speed = sneakSpeed;
            } else 
            {
                Speed = walkSpeed;
            }

            cameraStartPos = new Vector3(transform.position.x, transform.position.y + 0.488f, transform.position.z);
        }

        public void ReceiveInput(Vector2 movementInput)
        {
            _movementInput = new Vector3(movementInput.x, 0, movementInput.y);
        }

        private void MoveCharacter()
        {
            float deltaX = _movementInput.x * Speed;
            float deltaZ = _movementInput.z * Speed;

            Vector3 movement = new Vector3(deltaX, 0, deltaZ);
            movement = Vector3.ClampMagnitude(movement, Speed);

            movement.y = gravity;
            movement *= Time.deltaTime;
            movement = transform.TransformDirection(movement);

            _charController.Move(movement);
        }

        private void RotateCharacter()
        {
            if (!MenuManager.gamePaused)
            {
                transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
            }
        }

        private void RotateCamera()
        {
            if (!MenuManager.gamePaused)
            {
                rotationVert -= Input.GetAxis("Mouse Y") * sensitivityVert;
                rotationVert = Mathf.Clamp(rotationVert, minimumVert, maximumVert);

                headCam.transform.localEulerAngles = new Vector3(
                    rotationVert, headCam.transform.localEulerAngles.y, 0
                );
            }
        }

        public Vector3 GetVelocity()
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            return movement * (running ? 2 : 1);
        }

        private void AdjustCamera()
        { 
            if (sneaking)
            {
                headCam.transform.position = new Vector3(cameraStartPos.x, 
                    Mathf.Lerp(headCam.transform.position.y, cameraStartPos.y - 0.5f, crouchTransitionSpeed*Time.deltaTime), cameraStartPos.z);
            } else 
            {
                headCam.transform.position = new Vector3(cameraStartPos.x, 
                    Mathf.Lerp(headCam.transform.position.y, cameraStartPos.y, crouchTransitionSpeed*Time.deltaTime), cameraStartPos.z);
            }
        }
    }
}