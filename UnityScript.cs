using UnityEngine;
using System.IO.Ports;

public class NewEmptyCSharpScript : MonoBehaviour
{
    public CharacterController controller;   // Reference to Unity’s built-in movement/physics component
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;                // Helper variable used for smoothing

    SerialPort serialPort = new SerialPort("/dev/tty.usbmodem14401", 115200);
    string flexState = "STILL";  // default state
    void Start()
    {
        // Automatically grab the CharacterController on this GameObject
        controller = GetComponent<CharacterController>();

        serialPort.Open();
        serialPort.ReadTimeout = 50;
    }

    void Update()
    {
        // 1. Read serial input
        if (serialPort.IsOpen)
        {
            try
            {
                string line = serialPort.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    // Expecting only the state string from Python: "STILL", "WALK", or "RUN"
                    flexState = line.Trim();
                }
            }
            catch (System.TimeoutException) { }
        }

        // 2. Determine movement speed based on flex state
        float currentSpeed = 0f;
        if (flexState == "WALK")
            currentSpeed = walkSpeed;
        else if (flexState == "RUN")
            currentSpeed = runSpeed;
        else
            currentSpeed = 0f; // STILL

        // 3. Move character forward based on facing direction
        if (currentSpeed > 0f)
        {
            // Keep moving in the character's forward direction
            Vector3 moveDir = transform.forward;
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }

        // 4. Optional: Keep your keyboard-based rotation
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort.IsOpen)
            serialPort.Close();
    }
}
