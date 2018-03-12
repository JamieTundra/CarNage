using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    Button freeRoamButton;
    [SerializeField]
    Button trialButton;
    [SerializeField]
    Button minButton;
    [SerializeField]
    Button maxButton;
    [SerializeField]
    GameObject panel;
    TimeTrial timeTrial;
    FreeRoam freeRoam;
    bool minimised;
    bool maximised;

    public static DebugManager instance;

    private void Awake()
    {
        InitReferences();
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
        InitValues();
    }

    private void InitReferences()
    {
        if (instance == null)
            instance = this;

        timeTrial = GetComponent<TimeTrial>();
        freeRoam = GetComponent<FreeRoam>();
    }

    private void InitValues()
    {
        timeTrial.enabled = false;
        freeRoam.enabled = false;
        minButton.gameObject.SetActive(true);
        minButton.gameObject.SetActive(false);
        minimised = false;
        maximised = true;
    }

    public void StartTimeTrial()
    {
        timeTrial.enabled = true;
        freeRoam.enabled = false;
    }

    public void StartFreeRoam()
    {
        timeTrial.enabled = false;
        freeRoam.enabled = true;
    }

    public void Minimise()
    {
        if (maximised)
        {
            RectTransform panelRectTransform = panel.GetComponent<RectTransform>();
            panelRectTransform.sizeDelta.Set(40, 40);

            foreach (Transform child in panel.transform)
            {
                child.gameObject.SetActive(false);
            }

            maximised = false;
            minimised = true;
            minButton.gameObject.SetActive(false);
            minButton.gameObject.SetActive(true);
        }

    }

    public void Maximise()
    {
        if (minimised)
        {
            RectTransform panelRectTransform = panel.GetComponent<RectTransform>();
            panelRectTransform.sizeDelta.Set(150, 150);

            foreach (Transform child in panel.transform)
            {
                child.gameObject.SetActive(true);
            }

            maximised = true;
            minimised = false;
            minButton.gameObject.SetActive(true);
            minButton.gameObject.SetActive(false);
        }

    }
}
