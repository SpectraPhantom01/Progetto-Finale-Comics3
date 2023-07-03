using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region SINGLETON
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance != null)
                    return instance;

                GameObject go = new GameObject("UIManager");
                return go.AddComponent<UIManager>();
            }
            else
                return instance;
        }
        set
        {
            instance = value;
        }
    }

    #endregion

    [SerializeField] GameObject pauseMenu;
    public UIPauseMenu PauseMenu;
    public UIPlayArea UIPlayArea;
    [SerializeField] GameObject writtenPanel;
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] UIOptions uiOptions;
    [SerializeField] UIContinueButton continueButton;
    [SerializeField] LogScriptableObject logSO;
    bool pause = false;

    private List<string> logs;
    private void Awake()
    {
        PauseMenu = pauseMenu.GetComponent<UIPauseMenu>();
    }

    private void Start()
    {
        logs = new();

        if (logSO.Logs == null)
        {
            logSO.Logs = new();
        }
        else
        {
            foreach (var log in logSO.Logs)
            {
                logs.Add(log);
                uiOptions.Add(log);
            }
        }
    }

    public void Pause()
    {
        pause = !pause;

        Time.timeScale = pause ? 0 : 1;

        pauseMenu.SetActive(pause);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenWrittenPanel(string message, List<string> nextMessages = null)
    {
        writtenPanel.SetActive(true);
        messageText.text = message;

        Time.timeScale = 0;

        uiOptions.Add(message);

        logSO.Logs.Add(message);

        if(nextMessages != null && nextMessages.Count > 1)
        {
            List<Coroutine> messagesCoroutine = new();
            for (int i = 1; i < nextMessages.Count; i++)
            {
                messagesCoroutine.Add(StartCoroutine(NextMessageCoroutine(i * 0.3f, nextMessages[i])));
            }
        }
    }

    private IEnumerator NextMessageCoroutine(float time, string message)
    {
        yield return new WaitForSeconds(time);
        OpenWrittenPanel(message);
    }
}
