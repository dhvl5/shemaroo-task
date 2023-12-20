using System;
using Fusion;
using UnityEngine;
using ReadyPlayerMe.Core;

public class RPM_AvatarLoader : MonoBehaviour
{
    public static Action<GameObject> OnAvatarLoaded;

    public string avatarURL;
    public RuntimeAnimatorController animatorController;

    private GameObject _avatar;

    private void Start()
    {
        if (PlayerBehaviour.Local)
        {
            AvatarObjectLoader avatarLoader = new();

            avatarLoader.OnCompleted += OnAvatarLoadCompleted;

            avatarURL = avatarURL.Trim(' ');
            var result = avatarLoader.LoadAvatarAsync(avatarURL);
        }
    }

    private void OnAvatarLoadCompleted(object sender, CompletionEventArgs args)
    {
        _avatar = args.Avatar;
        AvatarAnimatorHelper.SetupAnimator(BodyType.FullBody, _avatar);

        if (_avatar == null)
            return;

        if (_avatar.TryGetComponent<Animator>(out var animator))
        {
            _avatar.transform.parent = transform;
            _avatar.transform.localPosition = new Vector3(0, -0.08f, 0);
            _avatar.transform.localRotation = Quaternion.Euler(0, 0, 0);

            // For fixing weird bug in Fusion where the object isn't spawning at the desired position when using with CharacterController...
            GetComponent<CharacterController>().enabled = false;
            transform.position = Vector3.zero;
            GetComponent<CharacterController>().enabled = true;

            animator.runtimeAnimatorController = animatorController;

            OnAvatarLoaded?.Invoke(_avatar);
        }
    }
}