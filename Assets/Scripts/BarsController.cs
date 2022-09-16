using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class BarsController : MonoBehaviour
{
    private Bar[] bars;
    private Button startButton;
    private string[] prefabNames ={"a","b","c"};
    private List<GameObject> downloadedPrefabs;

    private void Awake()
    {
        bars = FindObjectsOfType<Bar>();
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(InstantiateAssets);
    }

    private void InstantiateAssets()
    {
        
    }

    private async UniTask<GameObject[]>  DownloadAssets()
    {
        downloadedPrefabs.Clear();
        foreach (var prefabName in prefabNames)
        {
            await Addressables.DownloadDependenciesAsync(prefabName);
        }

        return default;
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