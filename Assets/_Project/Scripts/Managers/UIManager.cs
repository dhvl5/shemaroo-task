using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerStats playerStatsSO;

    [Header("Panels")]
    [Space]
    public GameObject findSessionPanel;
    public GameObject sessionListPanel;
    public GameObject createSessionPanel;

    [Space]
    public TMP_InputField playerNameInputField;
    public TMP_InputField sessionNameInputField;
    public GameObject errorTextGameObject;

    public static Action<string> OnFindSessionButtonClick;
    public static Action<string> OnCreateSessionButtonClick;

    private void Start()
    {
        errorTextGameObject.SetActive(false);
    }

    public void FindSessionButton()
    {
        if (playerNameInputField.text.Length <= 0)
        {
            errorTextGameObject.SetActive(true);
            return;
        }

        playerStatsSO.playerName = playerNameInputField.text;
        OnFindSessionButtonClick?.Invoke(playerNameInputField.text);

        FindObjectOfType<SessionListManager>(true).LookingForSessionsStatus();

        findSessionPanel.SetActive(false);
        sessionListPanel.SetActive(true);
        createSessionPanel.SetActive(false);
    }

    public void CreateNewSessionButton()
    {
        findSessionPanel.SetActive(false);
        sessionListPanel.SetActive(false);
        createSessionPanel.SetActive(true);
    }

    public void CreateSessionButton()
    {
        OnCreateSessionButtonClick?.Invoke(sessionNameInputField.text);

        findSessionPanel.SetActive(false);
        sessionListPanel.SetActive(false);
        createSessionPanel.SetActive(false);

        findSessionPanel.transform.parent.gameObject.SetActive(false);
    }

    public void OnJoiningSession()
    {
        findSessionPanel.SetActive(false);
        sessionListPanel.SetActive(false);
        createSessionPanel.SetActive(false);

        findSessionPanel.transform.parent.gameObject.SetActive(false);
    }
}