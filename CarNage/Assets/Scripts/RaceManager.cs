using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class RaceManager : MonoBehaviour
{
    #region Singleton
    public static RaceManager instance;
    public List<Transform> checkpoints = new List<Transform>();
    float countdownValue = 6f;
    float currCountdownValue;
    public TextMeshProUGUI countdownText;
    public List<RaceTime> raceTimes = new List<RaceTime>();


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        InitialiseCheckpoints();
        DisableCars();
        StartCoroutine(StartCountdown(countdownValue));
    }

    private void DisableCars()
    {
        //Debug.Log("Disabled cars");
        for (int i = 1  ; i < PlayerManager.instance.numberOfPlayers + 1; i++)
        {
            GameObject.FindGameObjectWithTag("Player" + i).GetComponent<InputHandler>().enabled = false;
            //Debug.Log("Car should be kinematic");
        }
    }

    IEnumerator StartCountdown(float countDownFrom)
    {
        currCountdownValue = countdownValue;

        while (currCountdownValue >= 1)
        {
            while (currCountdownValue >= 2)
            {
                countdownText.text = ((currCountdownValue -1).ToString());
                countdownText.gameObject.SetActive(true);
                yield return new WaitForSeconds(1.0f);
                currCountdownValue--;
            }

            while (currCountdownValue == 1)
            {
                countdownText.text = ("GO!");
                countdownText.gameObject.SetActive(true);
                yield return new WaitForSeconds(1.0f);
                currCountdownValue--;
            }

        }
        countdownText.gameObject.SetActive(false);

        StartRace();

    }

    private void StartRace()
    {
        //Debug.Log("Start Race");
        for (int i = 1; i < PlayerManager.instance.numberOfPlayers + 1; i++)
        {
            GameObject.FindGameObjectWithTag("Player" + i).GetComponent<InputHandler>().enabled = true;
        }
    }

    void InitialiseCheckpoints()
    {
        foreach (GameObject checkPoint in GameObject.FindGameObjectsWithTag("Checkpoint"))
        {
            checkpoints.Add(checkPoint.transform);
        }
    }

    public void AddRaceTime(RaceTime r)
    {
        raceTimes.Add(r); // add new player
        raceTimes.OrderBy(rT => rT.raceTime);
    }
    #endregion
}

public class RaceTime
{
    public string playerName;
    public float raceTime;
}
