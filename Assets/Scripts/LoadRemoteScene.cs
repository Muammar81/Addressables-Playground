using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadRemoteScene : MonoBehaviour
{
    [SerializeField] private Image loadingSlider;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private Button playButton;

    private AsyncOperationHandle _downloadHandle;
    private int _nextSceneIndex;
    private string _nextSceneName;


    private void OnEnable()
    {
        loadingSlider.fillAmount = 0;
        loadingText.text = "0%";
        playButton.interactable = false;
        
        //_nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        _nextSceneName = "Level1";// SceneManager.GetSceneByBuildIndex(_nextSceneIndex).name;
        
        _downloadHandle = Addressables.DownloadDependenciesAsync(_nextSceneName);
        _downloadHandle.Completed += OnDownloadLoaded;

        playButton.onClick.AddListener(() => LoadLevel(_nextSceneName));
    }

    private void OnDisable() => _downloadHandle.Completed -= OnDownloadLoaded;
    private void OnDownloadLoaded(AsyncOperationHandle obj)
    {
        if(obj.Status == AsyncOperationStatus.Succeeded)
        {
            playButton.interactable = true;
        }
    }

    private async void LoadLevel(string level1)
    {
        var sceneHandle = Addressables.LoadSceneAsync(_nextSceneName, LoadSceneMode.Single, true);
    }

    private void Update()
    {
        float p = _downloadHandle.GetDownloadStatus().Percent;
        if(p >= 1) return;
        
        loadingSlider.fillAmount = p;
        loadingText.text = p.ToString("P0");
    }
}