using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<AssetReference> cubePrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Button startButton;

    private Dictionary<AssetReference, AsyncOperationHandle> _opHandles = new ();
    private int spawnCount;

    private void OnEnable() => startButton.onClick.AddListener(DownloadAllCubes);
    private void OnDisable() => startButton.onClick.RemoveListener(DownloadAllCubes);



    public void DownloadAllCubes()
    {
        foreach (var cube in cubePrefabs)
        {
            _opHandles.Add(cube, Addressables.DownloadDependenciesAsync(cube));
            _opHandles[cube].Completed += LoadCube ; 
        }
    }

    private void LoadCube(AsyncOperationHandle handle)
    {
        var pos = spawnPoints[spawnCount].position;
        var rot = spawnPoints[spawnCount].rotation;
        Addressables.InstantiateAsync(handle,pos,rot);
        spawnCount++;
    }

    private void Update()
    {
        if (_opHandles.Count == 0) return;
        foreach (var op in _opHandles)
        {
            if(op.Value.Status == AsyncOperationStatus.Succeeded) continue;
            print($"{op.Key}: {op.Value.PercentComplete}");
        }
    }
}