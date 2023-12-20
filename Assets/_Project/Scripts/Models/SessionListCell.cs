using TMPro;
using System;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class SessionListCell : MonoBehaviour
{
    public TMP_Text sessionNameText;
    public TMP_Text playerCountText;
    public Button joinSessionButton;

    private SessionInfo _sessionInfo;

    public Action<SessionInfo> OnJoinSessionButtonClicked;

    public void SetData(SessionInfo sessionInfo)
    {
        _sessionInfo = sessionInfo;

        sessionNameText.text = sessionInfo.Name;
        playerCountText.text = $"{sessionInfo.PlayerCount}/{sessionInfo.MaxPlayers}";

        bool isJoinSessionButtonActive = true;

        if (sessionInfo.PlayerCount >= sessionInfo.MaxPlayers)
            isJoinSessionButtonActive = false;

        joinSessionButton.gameObject.SetActive(isJoinSessionButtonActive);
    }

    public void JoinSessionButtonClick()
    {
        OnJoinSessionButtonClicked?.Invoke(_sessionInfo);
    }
}