using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats", order = 0)]
public class PlayerStats : ScriptableObject
{
    public GameObject currentAvatar;
    public string playerName;
    public bool isSpeaking;
}