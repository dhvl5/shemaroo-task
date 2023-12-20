using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Orbit")]
    [Space]
    public float mouseSensitivityX = 1;
    public float mouseSensitivityY = 2;
    public bool smoothDamp = false;

    private float _mouseX;
    private float _mouseY;

    private float _minRotationX = -60f;
    private float _maxRotationX = 50f;

    private Vector3 _rotation;
    private Vector3 _currentVelocity;

    private bool _isOrbiting = false;

    [Header("Camera Follow")]
    [Space]
    public Transform target;
    public float cameraDistance = -2.4f;

    private bool _isFollowing = false;
    private Transform _cameraTransform;

    private GameObject _avatar;

    private void OnEnable()
    {
        RPM_AvatarLoader.OnAvatarLoaded += OnAvatarLoaded;
    }

    private void Start()
    {
        _rotation = transform.transform.eulerAngles;

        _cameraTransform = Camera.main.transform;
    }

    private void OnDisable()
    {
        RPM_AvatarLoader.OnAvatarLoaded -= OnAvatarLoaded;
    }

    private void OnAvatarLoaded(GameObject avatar)
    {
        if (target == null)
        {
            _avatar = avatar;

            target = avatar.transform.parent.GetChild(0);
            StartFollowingPlayer();
        }
    }

    private void LateUpdate()
    {
        if (_isOrbiting)
        {
            _mouseX += Input.GetAxis("Mouse X") * mouseSensitivityX;
            _mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivityY;

            if (smoothDamp)
                _rotation = Vector3.SmoothDamp(_rotation, new Vector3(_mouseY, _mouseX), ref _currentVelocity, .1f);
            else
                _rotation = new Vector3(_mouseY, _mouseX, _rotation.z);

            _rotation.x = ClampAngle(_rotation.x, _minRotationX, _maxRotationX);
            transform.transform.rotation = Quaternion.Euler(_rotation);

            if (_isFollowing)
            {
                _cameraTransform.localPosition = Vector3.forward * cameraDistance;
                _cameraTransform.localRotation = Quaternion.Euler(Vector3.zero);
                transform.position = target.position;
            }
        }
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
        {
            angle += 360F;
        }

        if (angle > 360F)
        {
            angle -= 360F;
        }

        return Mathf.Clamp(angle, min, max);
    }

    public void StartFollowingPlayer()
    {
        _isOrbiting = true;
        _isFollowing = true;
    }

    public void StopFollowingPlayer()
    {
        _isOrbiting = false;
        _isFollowing = false;
    }
}