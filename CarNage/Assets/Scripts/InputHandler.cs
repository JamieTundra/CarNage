using UnityEngine;

public class InputHandler : MonoBehaviour
{

    // Car movement
    public float m_steer;
    public float m_drivingForce;
    public bool m_handBrake;
    public bool m_isReversing;
    public bool m_selfRight;
    public int m_controllerID;
    CarController carController;
    public bool m_carInit = false;

    void Start()
    {
        carController = GetComponent<CarController>();
        if (m_controllerID == 0)
        {
            m_controllerID = int.Parse(this.tag.Substring(6, 1));
        }
    }

    public void FixedUpdate()
    {
        if (m_carInit)
        {
            m_steer = Input.GetAxis(m_controllerID + "LAnalogX");
            m_drivingForce = Input.GetAxis(m_controllerID + "Triggers");
            //m_drivingForce = Mathf.RoundToInt(Input.GetAxis(m_controllerID + "Triggers"));
            m_handBrake = Input.GetButton(m_controllerID + "XButton");
            m_selfRight = Input.GetButton(m_controllerID + "YButton");
            if (m_drivingForce < -0.1)
            {
                m_isReversing = true;
            }

            carController.Steer(m_steer);
            carController.Drive(m_drivingForce);
            carController.HandBrake(m_handBrake);

            if (m_selfRight)
            {
                carController.SelfRight();
            }
        }
    }
}
