using UnityEngine;

[CreateAssetMenu]
public class Car : ScriptableObject
{

    public string m_carName;
    public float m_mass;
    public float m_maxWheelRPM;
    public float m_turnForce;
    public float m_maxTorque;
    public float m_maxBrakeTorque;
    public bool m_canSelfRight;

}
