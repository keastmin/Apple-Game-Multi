using SinglePlay.Manager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject ScorePanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeOutScoreText;

    public bool MenuPanelActive => MenuPanel.activeSelf;
    public bool ScorePanelActive => ScorePanel.activeSelf;
    public int Score
    {
        get => int.Parse(scoreText.text);
        set => scoreText.text = value.ToString();
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        InitUIManager();
    }

    private void Start()
    {
        GameManager.Instance.OnRestartGame += InitUIManager;
    }

    void Update()
    {
        MenuButtonKeyboardTab();
    }

    private void InitUIManager()
    {
        InitPanelVisible();
        InitTextString();
    }

    private void InitPanelVisible()
    {
        MenuPanel.SetActive(false);
        ScorePanel.SetActive(false);
    }

    private void InitTextString()
    {
        Score = 0;
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

    public void OnClickRestartButton()
    {
        GameManager.Instance.RestartGame();
    }

    public void TimeEnd()
    {
        timeOutScoreText.text = scoreText.text;
        ScorePanel.SetActive(true);
    }
}
