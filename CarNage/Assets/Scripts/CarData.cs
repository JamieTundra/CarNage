using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CarData : ScriptableObject {

    public float m_mass;
    public float m_maxSpeed;
    public float m_maxTorque;
    public float m_turnForce;
    public float m_maxBrakeTorque;

}
