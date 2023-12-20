using TMPro;
using Fusion;
using UnityEngine.UI;

public class PlayerBehaviour : NetworkBehaviour, IPlayerLeft
{
    public static PlayerBehaviour Local { get; set; }

    [Networked(OnChanged = nameof(UpdatePlayerName))]
    public NetworkString<_16> PlayerName { get; set; }

    [Networked(OnChanged = nameof(UpdateVoiceIndicator))]
    public NetworkBool isSpeaking { get; set; }

    public PlayerStats playerStats;
    public TMP_Text playerNameText;
    public Image speakingIndicator;

    public PlayerMovementHandler _playerMovementHandler;

    private static void UpdatePlayerName(Changed<PlayerBehaviour> changed)
    {
        changed.Behaviour.playerNameText.text = changed.Behaviour.PlayerName.ToString();
    }

    private static void UpdateVoiceIndicator(Changed<PlayerBehaviour> changed)
    {
        var _isSpeaking = changed.Behaviour.isSpeaking;
        var _speakingIndicator = changed.Behaviour.speakingIndicator;

        if (_isSpeaking)
            _speakingIndicator.enabled = true;
        else
            _speakingIndicator.enabled = false;
    }

    private void Start()
    {
        if (HasStateAuthority)
        {
            PlayerName = playerStats.playerName;
        }
    }

    private void Update()
    {
        if (HasStateAuthority)
        {
            isSpeaking = playerStats.isSpeaking;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority)
            return;

        if (GetInput(out PlayerInputStruct playerInputStruct))
        {
            _playerMovementHandler.Move(playerInputStruct);
        }
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
            Runner.Despawn(Object);
    }
}