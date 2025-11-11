using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EventTrigger : MonoBehaviour
{
	[SerializeField] private EventHandler _eventHandler;

	private void Awake()
	{
		GetComponent<BoxCollider2D>().isTrigger = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out Player p))
			_eventHandler.TryExecuteEvents();
	}

	public void Initialize(EventHandler eventHandler)
	{
		_eventHandler = eventHandler;
	}
}
