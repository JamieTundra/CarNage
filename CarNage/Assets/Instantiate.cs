using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate : MonoBehaviour {

    public GameObject spawn;
    public GameObject car;

	// Use this for initialization
	void Start () {
        Instantiate(car, spawn.transform.position, Quaternion.identity);
	}
	
}
