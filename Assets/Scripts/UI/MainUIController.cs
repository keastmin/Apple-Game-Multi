using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIController : MonoBehaviour
{
    private readonly string _soloPlayGameSceneName = "SoloPlayGameScene";
    private readonly string _multiPlayGameSceneName = "MultiPlayGameScene";

    public void OnClickSoloPlayButton()
    {
        SceneManager.LoadScene(_soloPlayGameSceneName);
    }

    public void OnClickMultiPlayButton()
    {

    }

    public void OnClickQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
