using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Image bar;
    private TMP_Text percentageText;

    private void Awake()
    {
        percentageText = GetComponentInChildren<TMP_Text>();
        Reset();
    }

    private void Reset()
    {
        bar.fillAmount = 0;
        percentageText.text = bar.fillAmount.ToString("P0");
    }

    public async Task Fill(int delay)
    {
        Reset();
        
        while (bar.fillAmount < 1)
        {
            bar.fillAmount += Time.deltaTime;
            percentageText.text = bar.fillAmount.ToString("P0");
            await Task.Delay(delay);
        }
    }
}