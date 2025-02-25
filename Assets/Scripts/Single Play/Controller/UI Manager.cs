using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject ScorePanel;

    public bool MenuPanelActive => MenuPanel.activeSelf;
    public bool ScorePanelActive => ScorePanel.activeSelf;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        InitPanelVisible();
    }

    void Update()
    {
        MenuButtonKeyboardTab();
    }

    private void InitPanelVisible()
    {
        MenuPanel.SetActive(false);
        ScorePanel.SetActive(false);
    }

    private void MenuButtonKeyboardTab()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickMenuButton();
        }
    }

    public void OnClickMenuButton()
    {
        MenuPanel.SetActive(!MenuPanel.activeSelf);
    }

    public void OnClickQuitButton()
    {
        SceneManager.LoadScene("StartMenuScene");
    }
}
