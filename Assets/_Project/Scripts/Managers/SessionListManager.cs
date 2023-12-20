using TMPro;
using Fusion;
using UnityEngine;

public class SessionListManager : MonoBehaviour
{
    public TMP_Text statusText;

    public GameObject sessionListCellPrefab;
    public Transform sessionListContent;

    public void AddSessionToList(SessionInfo sessionInfo)
    {
        SessionListCell sessionListCell = Instantiate(sessionListCellPrefab, sessionListContent).GetComponent<SessionListCell>();

        sessionListCell.SetData(sessionInfo);

        sessionListCell.OnJoinSessionButtonClicked += OnJoinSessionButtonClicked;
    }

    private void OnJoinSessionButtonClicked(SessionInfo info)
    {
        FindObjectOfType<NetworkManager>().JoinSession(info);

        FindObjectOfType<UIManager>().OnJoiningSession();
    }

    public void ClearSessionList()
    {
        foreach (Transform cell in sessionListContent)
            Destroy(cell.gameObject);

        statusText.gameObject.SetActive(false);
    }

    public void NoSessionFoundStatus()
    {
        ClearSessionList();

        statusText.text = "No sessions found!";
        statusText.gameObject.SetActive(true);
    }

    public void LookingForSessionsStatus()
    {
        ClearSessionList();

        statusText.text = "Looking for sessions...";
        statusText.gameObject.SetActive(true);
    }
}