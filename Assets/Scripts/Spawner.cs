using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<AssetReference> cubePrefabs;
    [SerializeField] private Transform[] spawnPoints;

    private Dictionary<AssetReference, AsyncOperationHandle> _opHandles;
    private int spawnCount;

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
        foreach (var op in _opHandles)
        {
            if(op.Value.Status == AsyncOperationStatus.Succeeded) continue;
            print($"{op.Key}: {op.Value.PercentComplete}");
        }
    }
}