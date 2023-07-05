using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu_ChooseLv : MonoBehaviour
{
    public GameObject LoadingScreen;
    public TMP_Text loadingText;
    public bool isDelay = false;
    public float WaitToLoading = 0;
    public bool isLoad;
    AsyncOperation loading;

    public void Start() 
    {
        loadingText = LoadingScreen.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>();
    }
    public void LoadLevel(int SceneIdx)
    {
        StartCoroutine(LoadSceneAsync(SceneIdx));
    }

    IEnumerator LoadSceneAsync(int SceneIdx)
    {
        loading = SceneManager.LoadSceneAsync(SceneIdx);
        isLoad = true;
        LoadingScreen.SetActive(true);
        loading.allowSceneActivation = false;
        while(!loading.isDone)
        {
            float progress = Mathf.Clamp01(loading.progress / 0.9f);
            LoadingScreen.GetComponentInChildren<Slider>().value = progress;
            loadingText.text = (progress * 100) + "%";;
            if (progress >= 0.9f)
            {
                loadingText.text = "Touch the screen to continue";
            }
            yield return null;
        }
    }

    public void OnClickTheScreen()
    {
        if(loading != null)
        {
            loading.allowSceneActivation = true;
        }
    }

    private void Update() 
    {
        if(loading != null && WaitToLoading <= 0 && isLoad && isDelay)
        {
            loading.allowSceneActivation = true;
        }
    }

    private void FixedUpdate() 
    {
        if(WaitToLoading > 0 && isLoad && isDelay)
        {
            WaitToLoading -= 0.02f;
        }
    }
}
