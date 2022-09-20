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
    [SerializeField] private List<AssetReference> cubeReferences;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Button startButton;

    private Dictionary<AssetReference, AsyncOperationHandle> _opHandles = new ();
    private int spawnCount;

    private void OnEnable() => startButton.onClick.AddListener(DownloadAllCubes);
    private void OnDisable() => startButton.onClick.RemoveListener(DownloadAllCubes);

    private void Update()
    {
        if (_opHandles.Count == 0) return;
        foreach (var op in _opHandles)
        {
            if (op.Value.Status == AsyncOperationStatus.Succeeded) continue;
            print($"{op.Key}: {op.Value.GetDownloadStatus().Percent}");
        }
    }


    public void DownloadAllCubes()
    {
        foreach (var cube in cubeReferences)
        {
            //var handle = Addressables.DownloadDependenciesAsync(cube);
            //_opHandles.Add(cube, handle);
            var handle = Addressables.LoadAssetAsync<GameObject>(cube);
            handle.Completed += (op) =>
            {
                print($"Loaded: {handle.DebugName}");
                var pos = spawnPoints[spawnCount].position;
                var rot = spawnPoints[spawnCount].rotation;
                
                var h = cube.InstantiateAsync(pos,rot, transform);
                spawnCount++;
                
                h.Completed += Instantiated;
            };
        }
    }

    private void Downloaded(AsyncOperationHandle<GameObject> op)
    {
        //Addressables.InstantiateAsync(obj ,pos,rot);
    }

    private void Downloaded(AsyncOperationHandle handle)
    {
        print($"Downloaded: {handle.DebugName}");
        var pos = spawnPoints[spawnCount].position;
        var rot = spawnPoints[spawnCount].rotation;
        //var obj = Addressables.InstantiateAsync(handle,pos,rot);
        //obj.Completed += Instantiated;
        spawnCount++;
    }

    private void Instantiated(AsyncOperationHandle<GameObject> obj)
    {
        print(obj.DebugName +" is instantiated");
    }


}