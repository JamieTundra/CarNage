using UnityEngine;

[CreateAssetMenu]
public class Car : ScriptableObject
{

    public string m_carName;
    public float m_mass;
    public float m_maxWheelRPM;
    public float m_maxSpeed;
    public float m_turnAngle;
    public float m_turnSpeed;
    public float m_torque;
    public float m_BrakeTorque;
    public bool m_canSelfRight;

}
