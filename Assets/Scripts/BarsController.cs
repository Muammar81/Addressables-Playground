using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarsController : MonoBehaviour
{
    private Bar[] bars;
    private Button startButton;

    private void Awake()
    {
        bars = FindObjectsOfType<Bar>();
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(FillBars);
    }

    private async void FillBars()
    {
        startButton.interactable = false;
        foreach (var bar in bars)
        {
            await bar.Fill(1);
        }
        startButton.interactable = true;
    }
}