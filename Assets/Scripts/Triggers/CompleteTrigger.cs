using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CompleteTrigger : MonoBehaviour
{
	[SerializeField] private Level _level;

	private void Awake()
	{
		_level = GetComponentInParent<Level>();
	}

	private void OnTriggerEnter2D(Collider2D collider)
    {
		if (!collider.gameObject.TryGetComponent(out Player player))
			return;
		_level.Complete();
    }
}
