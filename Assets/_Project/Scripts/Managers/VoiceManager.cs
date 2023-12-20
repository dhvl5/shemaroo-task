using UnityEngine;
using Photon.Voice.Unity;

public class VoiceManager : MonoBehaviour
{
    public PlayerStats playerStats;

    [Space]
    public Recorder recorder;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            recorder.TransmitEnabled = true;

        if (Input.GetKeyUp(KeyCode.V))
            recorder.TransmitEnabled = false;

        if (recorder.TransmitEnabled && recorder.VoiceDetector.Detected)
        {
            playerStats.isSpeaking = true;
        }
        else
        {
            playerStats.isSpeaking = false;
        }
    }
}