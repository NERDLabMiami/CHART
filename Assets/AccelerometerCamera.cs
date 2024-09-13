using UnityEngine;
using UnityEngine.Video;
using UnityEngine.InputSystem; // Import the new Input System

public class AccelerometerCamera : MonoBehaviour
{
    public float sensitivity = 0.1f;  // Sensitivity for the accelerometer input
    private Vector3 initialRotation;  // The initial rotation of the device
    private bool attitudeSensorEnabled; // Flag to check if the Attitude Sensor is enabled
    private VideoPlayer videoPlayer;  // Reference to the VideoPlayer component

    private void Awake()
    {
        // Get the VideoPlayer component attached to the same GameObject
        videoPlayer = GetComponent<VideoPlayer>();
    }

    private void Start()
    {
        // Check if the device supports an attitude sensor
        if (AttitudeSensor.current != null)
        {
            attitudeSensorEnabled = true;
            InputSystem.EnableDevice(AttitudeSensor.current);
        }

        // Capture the initial rotation of the device
        initialRotation = GetDeviceRotation();
    }

    private void Update()
    {
        // Get the current rotation based on the device's sensor data
        Vector3 currentRotation = GetDeviceRotation();

        // Calculate the difference between the initial and current rotation
        Vector3 rotationDelta = currentRotation - initialRotation;

        // Rotate the camera based on the sensor input
        transform.Rotate(-rotationDelta.y * sensitivity, rotationDelta.x * sensitivity, 0, Space.World);
    }

    // Get the rotation from the Attitude Sensor or Accelerometer using the new Input System
    private Vector3 GetDeviceRotation()
    {
        if (attitudeSensorEnabled && AttitudeSensor.current != null)
        {
            // Use the Attitude Sensor for rotation
            Quaternion attitude = AttitudeSensor.current.attitude.ReadValue();
            return attitude.eulerAngles;
        }
        else if (Accelerometer.current != null)
        {
            // Use the accelerometer to calculate rotation if no attitude sensor
            Vector3 acceleration = Accelerometer.current.acceleration.ReadValue();
            return new Vector3(acceleration.y, -acceleration.x, 0);
        }

        return Vector3.zero; // Fallback in case neither sensor is available
    }
}
