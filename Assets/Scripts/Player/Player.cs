using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class Player : Magnet
{
    private Tilemap _tilemap;
    private Rigidbody2D _rigidbody;
	private List<EMstream> _EMStreams = new List<EMstream>();

    [SerializeField, Range(0, float.MaxValue)] private float _horizontalForce;
    [SerializeField, Range(0, float.MaxValue)] private float _verticalForce;
	[SerializeField, Range(0, float.MaxValue)] private float _interactionForce;
	[SerializeField] private TileBase _magnetTile;
	[SerializeField] private Vector2[] _debugCheckpoints;

	private static int _debugCheckpointsCount;

    public void Initialize()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _tilemap = FindAnyObjectByType<Scene>().Tilemap;
		_debugCheckpointsCount = Mathf.Min(10, _debugCheckpoints.Length);

	}

    private void FixedUpdate()
    {
		TryUseDebugCheckpoints();
        if (Input.GetKey(KeyCode.R))
            Scene.Restart();
		if (Input.GetKey(KeyCode.E))
			Scene.ReturnToMenu();

        if (!IsActive(out IEnumerable<Magnet> objectsInField))
            return;

        Vector2 force = GetForce();
        Push(force);
		PushMagnets(objectsInField);
    }

    private void TryUseDebugCheckpoints()
	{
		for (var i = KeyCode.Alpha0; (int)i < (int)KeyCode.Alpha0 + _debugCheckpointsCount; i++)
			if (Input.GetKey(i))
			{
				_rigidbody.velocity = Vector2.zero;
				_rigidbody.angularVelocity = 0;
				transform.position = _debugCheckpoints[i - KeyCode.Alpha0];
			}
	}

	private void PushMagnets(IEnumerable<Magnet> magnets)
	{
		if (magnets == null)
			return;

		foreach (var magnet in magnets)
		{
			if (magnet == this) continue;
			Vector3 relativePosition = magnet.transform.position - transform.position;
			Vector2 force = _interactionForce * relativePosition.normalized / relativePosition.sqrMagnitude;
			magnet.Push(force);
			Push(-force);
		}
	}

    private Vector2 GetForce()
    {
        var force = new Vector2();

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
            force += Vector2.up;
        if (Input.GetKey(KeyCode.S))
            force += Vector2.down;
        if (Input.GetKey(KeyCode.A))
            force += Vector2.left;
        if (Input.GetKey(KeyCode.D))
            force += Vector2.right;

		force.x *= _horizontalForce;
		force.y *= _verticalForce;
        return force;
    }

    private bool IsActive(out IEnumerable<Magnet> objectsInField)
    {
        if (Input.GetKey(KeyCode.Q))
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _interactionRadius, Vector2.zero);
            IEnumerable<Magnet> magnets = hits.Select(hit => hit.collider.gameObject.GetComponent<Magnet>()).
                Where(magnet => magnet != null && !(magnet is Player)).Distinct();

            objectsInField = magnets;
			if (magnets.Count() > 0)
				return true;
        }

        int tiles = VectorUtilities.TilesCountInRange(_tilemap, _magnetTile, transform.position, _interactionRadius);
        objectsInField = null;
        return tiles > 0 || _EMStreams.Count > 0;
    }

	public override void Push(Vector2 force) => _rigidbody.AddForce(force);
	public override bool IsActive() => IsActive(out IEnumerable<Magnet> objectsInField);

	public void EnterEMStream(EMstream stream)
	{
		if (stream == null || _EMStreams.Contains(stream))
			return;
		_EMStreams.Add(stream);
	}

	public void ExitEMStream(EMstream stream)
	{
		_EMStreams.Remove(stream);
	}

    public float VerticalForce => _verticalForce;
    public float HorizontalForce => _horizontalForce;
    public float InteractionRadius => _interactionRadius;
}