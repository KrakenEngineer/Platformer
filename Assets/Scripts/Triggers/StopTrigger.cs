using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StopTrigger : MonoBehaviour
{
	[SerializeField] private bool _magnetsOnly = false;
	[SerializeField] private bool _playerOnly = false;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.TryGetComponent(out Rigidbody2D r) || Unstoppable(collision))
			return;
		r.velocity = Vector2.zero;
		r.angularVelocity = 0;
	}

	private bool Unstoppable(Collider2D collision)
	{
		return (_magnetsOnly && !collision.TryGetComponent(out Magnet m)) ||
			(_playerOnly && !collision.TryGetComponent(out Player p));
	}
}
