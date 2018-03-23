using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    #region Singleton
    public LevelManager instance;
    public List<Transform> startPoints = new List<Transform>();
    public GameObject startPointHolder;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        InitialiseStartPoints();

        foreach (Player p in PlayerManager.instance.Players)
        {
            SpawnPlayer(p);
        }
    }

    private void Update()
    {
        if (Input.GetButtonUp("StartButton"))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    void InitialiseStartPoints()
    {
        startPointHolder = GameObject.Find("StartPointHolder");
        for (int i = 0; i < startPointHolder.transform.childCount; i++)
        {
            startPoints.Add(startPointHolder.transform.GetChild(i));
        }
    }

    void SpawnPlayer(Player p)
    {
        Transform chosenSpawn = startPoints[p.controllerID - 1];
        //Debug.Log(chosenSpawn.position);
        string carName = p.carID.Substring(0, (p.carID.Length - 7));
        GameObject chosenCar = Resources.Load("Cars/" + carName) as GameObject;
        GameObject player = Instantiate(chosenCar, chosenSpawn.transform.localPosition, Quaternion.identity);
        player.tag = p.playerName;
        player.transform.SetParent((GameObject.Find(p.playerName + "Holder").transform));
        player.AddComponent<CurrentRace>();
    }

    #endregion
}
