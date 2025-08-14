using UnityEngine;

public class UILookAt : MonoBehaviour // Class definition (UILookAt is a class that inherits from MonoBehaviour)
{
    Camera cam; // Variable (reference to the main camera)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main; // Method (gets a reference to the main camera and assigns it to the variable `cam`)
    }

    // Update is called once per frame
    void Update()
    {
        // Method (Makes the UI element always face the camera)
        transform.LookAt(
            transform.position + cam.transform.rotation * Vector3.forward, // Calculates the direction to look at
            cam.transform.rotation * Vector3.up // Keeps the UI element aligned with the camera's up direction
        );
    }
}
