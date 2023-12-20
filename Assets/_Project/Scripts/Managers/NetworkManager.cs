using System;
using Fusion;
using UnityEngine;
using Fusion.Sockets;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [Space]
    public NetworkObject playerPrefab;

    public NetworkRunner networkRunner;

    public SessionListManager sessionListManager;

    private PlayerInputHandler _playerInputHandler;

    private void Awake()
    {
        networkRunner.ProvideInput = true;
    }

    private void OnEnable()
    {
        UIManager.OnFindSessionButtonClick += OnFindSessionButtonClick;
        UIManager.OnCreateSessionButtonClick += OnCreateSessionButtonClick;
    }

    private void OnDisable()
    {
        UIManager.OnFindSessionButtonClick -= OnFindSessionButtonClick;
        UIManager.OnCreateSessionButtonClick += OnCreateSessionButtonClick;
    }

    private void OnFindSessionButtonClick(string playerName)
    {
        FusionLobby();
    }

    private void OnCreateSessionButtonClick(string sessionName)
    {
        CreateSession(sessionName);
    }

    public async void FusionLobby()
    {
        if (networkRunner == null)
            networkRunner = gameObject.AddComponent<NetworkRunner>();

        var result = await networkRunner.JoinSessionLobby(SessionLobby.Custom, "LobbyName");

        if (result.Ok)
            Debug.Log($"FussionLobby joined!");
        else
            Debug.Log($"Unable to join: LobbyName");
    }

    public async void FusionSession(GameMode gameMode, string sessionName)
    {
        if (networkRunner == null)
            networkRunner = gameObject.AddComponent<NetworkRunner>();

        var result = await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = gameMode,
            SessionName = sessionName,
            CustomLobbyName = "LobbyName",
            Scene = 0,
            PlayerCount = 4,
        });

        if (result.Ok)
            Debug.Log($"FusionSession created!");
        else
            Debug.Log($"Unable to create: {sessionName}");
    }

    public void CreateSession(string sessionName)
    {
        Debug.Log($"Creating Session named: {sessionName}");

        FusionSession(GameMode.Host, sessionName);
    }

    public void JoinSession(SessionInfo sessionInfo)
    {
        Debug.Log($"Joining Session named: {sessionInfo.Name}");

        FusionSession(GameMode.Client, sessionInfo.Name);
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log($"{sessionList.Count}");

        if (sessionListManager == null)
            return;

        if (sessionList.Count == 0)
        {
            sessionListManager.NoSessionFoundStatus();
        }
        else
        {
            sessionListManager.ClearSessionList();

            foreach (SessionInfo sessionInfo in sessionList)
            {
                sessionListManager.AddSessionToList(sessionInfo);
            }
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"OnPlayerJoined");

        if (runner.IsServer)
        {
            NetworkObject playerObject = runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, player);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (_playerInputHandler == null && PlayerBehaviour.Local != null)
            _playerInputHandler = PlayerBehaviour.Local.GetComponent<PlayerInputHandler>();

        if (_playerInputHandler != null)
            input.Set(_playerInputHandler.GetPlayerInput());
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log($"OnConnectedToServer");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }
}