using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRace : MonoBehaviour
{
    public float checkpointCount = 1;
    public float totalCheckpoints;
    public float startTime;
    public float finishTime;
    bool onCheckPoint = false;
    bool finishedRace = false;

    private void Start()
    {
        totalCheckpoints = RaceManager.instance.checkpoints.Count;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            if (other.gameObject.name == "Checkpoint" + checkpointCount && !onCheckPoint)
            {
                onCheckPoint = true;
            }
        }

        if (other.CompareTag("FinishLine"))
        {
            if (checkpointCount == totalCheckpoints + 1 && !finishedRace)
            {
                finishTime = Time.time - startTime;
                finishedRace = true;
                RaceTime r = new RaceTime
                {
                    playerName = other.tag,
                    raceTime = finishTime
                };
                Debug.Log(r.playerName + " finished the race in: " + finishTime);
                RaceManager.instance.AddRaceTime(r);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Checkpoint" + checkpointCount && onCheckPoint)
        {
            onCheckPoint = false;
            checkpointCount++;
        }
    }


}
