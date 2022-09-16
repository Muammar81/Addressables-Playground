using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadRemoteScene : MonoBehaviour
{
    [SerializeField] private Image loadingSlider;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private Button playButton;
    [SerializeField] private AssetReference nextScene;
    private AssetReference mainMenuScene;

    private AsyncOperationHandle _downloadHandle;
    private int _nextSceneIndex;

    private void OnEnable()
    {
        ResetUI();
        _downloadHandle = Addressables.DownloadDependenciesAsync(nextScene);
        _downloadHandle.Completed += OnDownloadComplete;
    }

    private void OnDisable() => _downloadHandle.Completed -= OnDownloadComplete;
    
    private void Update()
    {
        float p = _downloadHandle.GetDownloadStatus().Percent;
        if(p >= 1) return;
        
        loadingSlider.fillAmount = p;
        loadingText.text = p.ToString("P0");
    }
    
    private void ResetUI()
    {
        loadingSlider.fillAmount = 0;
        loadingText.text = "0%";
        playButton.interactable = false;
    }

    
    private void OnDownloadComplete(AsyncOperationHandle handle)
    {
        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            playButton.onClick.AddListener(() => LoadLevel(nextScene));
            playButton.interactable = true;
        }
    }


    private void LoadLevel(AssetReference level)
    {
        Addressables.LoadSceneAsync(level, LoadSceneMode.Single, true); // store the handle in a global [DontDestroyOnload] to  Addressables.UnloadSceneAsync(handle);
    }
}