﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VT_controller : MonoBehaviour
{
    public VT_tube_detect detectionArea;
    public GameObject display;
    public Transform machineCase;

    // Update is called once per frame
    void Update()
    {
        if (machineCase.localRotation.x <= 0.01f && detectionArea.lastTube != null)
        {
            float similarity = detectionArea.lastTube.similarity;
            float reproducibility = detectionArea.lastTube.reproducibility;
            float severity = detectionArea.lastTube.severity;
            float score = similarity + reproducibility + (1 - severity * 2);
            string newText = similarity + "\n" + reproducibility + "\n" + severity;
            Debug.Log(newText);
            Text component = display.GetComponent<Text>();
            component.text = newText;
            if (score >= 2.5f)
            {
                component.color = Color.green;
            } else if (score >= 1.5f)
            {
                component.color = Color.yellow;
            } else
            {
                component.color = Color.red;
            }
        }
    }
}