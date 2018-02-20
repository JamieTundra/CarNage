using UnityEngine;

public class InputHandler : MonoBehaviour
{

    // Car movement
    public float m_steer;
    public float m_currentSpeed;
    public float m_accelerate;
    public float m_brake;
    public bool m_handBrake;
    public int m_controllerID;
    CarController carController;

    void Start()
    {
        carController = GetComponent<CarController>();
    }

    public void FixedUpdate()
    {
        m_steer = Input.GetAxis(m_controllerID + "LAnalogX");
        m_accelerate = Input.GetAxis(m_controllerID + "RTrigger");
        m_brake = -1 * Input.GetAxis(m_controllerID + "LTrigger");
        m_handBrake = Input.GetButton(m_controllerID + "XBtn");

        carController.Steer(m_steer);
        carController.Drive(m_handBrake, m_brake, m_accelerate);
        carController.Brake(m_handBrake);
    }
}
