using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Camera Variables
    public float smoothing = 6f;
    public Transform lookAtTarget;
    public Transform positionTarget;
    public Transform sideView;

    // Flag to check if showing sideview
    bool m_ShowingSideView = false;

    private void FixedUpdate()
    {
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        // If we are showing sideview
        if (m_ShowingSideView)
        {
            // Set the cameras position to match the sideview from the CameraRig
            transform.position = sideView.position;
            transform.rotation = sideView.rotation;
        }
        else
        {
            // 
            transform.position = Vector3.Lerp(transform.position, positionTarget.position, Time.deltaTime * smoothing);
            transform.LookAt(lookAtTarget);
        }
    }
}
