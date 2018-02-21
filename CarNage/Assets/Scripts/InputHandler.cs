using UnityEngine;

public class InputHandler : MonoBehaviour
{

    // Car movement
    public float m_steer;
    public float m_currentSpeed;
    public float m_drivingForce;
    public bool m_handBrake;
    public int m_controllerID;
    CarController carController;
    public bool m_carInit = false;

    void Start()
    {
        carController = GetComponent<CarController>();
        m_controllerID = int.Parse(this.tag.Substring(6, 1));
    }

    public void FixedUpdate()
    {
        if (m_carInit)
        {
            m_steer = Input.GetAxis(m_controllerID + "LAnalogX");
            m_drivingForce = Input.GetAxis(m_controllerID + "Triggers");
            m_handBrake = Input.GetButton(m_controllerID + "XButton");

            carController.Steer(m_steer);
            carController.Drive(m_handBrake, m_drivingForce);
            carController.Brake(m_handBrake);
        }

    }
}
