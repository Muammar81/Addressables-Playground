using System.Collections.Generic;
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

    private void OnEnable() => startButton.onClick.AddListener(DownloadAllCubes);
    private void OnDisable() => startButton.onClick.RemoveListener(DownloadAllCubes);

    private void Update()
    {
        if (_opHandles.Count == 0) return;
        foreach (var op in _opHandles)
        {
            //if (op.Value.Status == AsyncOperationStatus.Succeeded) continue;
            var p = op.Value.GetDownloadStatus().Percent;
            if (p == 0 || p >= 1) return;
            print($"{op.Key.editorAsset.name}: {p.ToString("P0")}");
        }
    }


    public void DownloadAllCubes()
    {
        var spawnCount = 0;
        foreach (var cube in cubeReferences)
        {
            var handle = Addressables.DownloadDependenciesAsync(cube);
            handle.Completed += (op) =>
            {
                print($"Loaded: {cube.editorAsset.name}");
                var pos = spawnPoints[spawnCount].position;
                var rot = spawnPoints[spawnCount].rotation;
                
                var h = cube.InstantiateAsync(pos,rot, transform);
                spawnCount++;
                
                h.Completed += Instantiated;
            };
            
            if(!_opHandles.ContainsKey(cube))
                _opHandles.Add(cube, handle);
        }
    }

    private void Instantiated(AsyncOperationHandle<GameObject> obj)
    {
        print(obj.Result.name +" is instantiated");
    }


}