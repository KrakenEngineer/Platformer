using UnityEngine;

public class Portal : MonoBehaviour
{
	private PortalPair _pair = null;
    private SpriteRenderer _renderer => GetComponent<SpriteRenderer>();

	public void Initialize(PortalPair pair, Color color)
	{
		if (_pair != null)
			throw new System.Exception("This portal is already initialized");
		if (pair == null)
			throw new System.NullReferenceException();
		_pair = pair;
		_renderer.color = color;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		_pair.Teleport(this, collision.transform);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		_pair.Remove(this, collision.transform);
	}
}
