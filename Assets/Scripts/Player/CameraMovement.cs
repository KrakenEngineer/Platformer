using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class CameraMovement : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private Player _target;

    [SerializeField] private float _depth;
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _minZoom;
    [SerializeField] private float _maxZoom;

    private void Start() => _camera = GetComponent<Camera>();

    private void Update()
	{
		float x = _target.transform.position.x;
		float y = _target.transform.position.y;
		var position = new Vector3(x, y, _depth);
		transform.position = position;

		float resultChange = -Input.GetAxis("MouseScrollWheel") * _zoomSpeed * _camera.orthographicSize;
		resultChange += _camera.orthographicSize;
		resultChange = Mathf.Clamp(resultChange, _minZoom, _maxZoom);
		_camera.orthographicSize = resultChange;
	}
}