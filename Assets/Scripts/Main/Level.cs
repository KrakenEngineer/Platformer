using UnityEngine;

public sealed class Level : MonoBehaviour
{
	[SerializeField] private uint _id;
	[SerializeField] private Vector2 _position;

	public void Initialize(Scene scene)
	{
		var handlers = transform.GetComponentsInChildren<EventHandler>();
		foreach (var handler in handlers)
			handler.Initialize(scene);
	}

	public void Complete() => GlobalData.CompleteLevel(_id);

	public uint ID => _id;
	public Vector2 Position => _position;
}

public enum LevelState : byte
{
	Locked,
	Unlocked,
	Completed
}
