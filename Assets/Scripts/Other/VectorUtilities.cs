using UnityEngine;
using UnityEngine.Tilemaps;

public static class VectorUtilities
{
    public static bool IsPositionInCircle(Vector2 center, Vector2 position, float radius)
    {
        Vector2 deltaPos = center - position;
        return deltaPos.sqrMagnitude > radius;
    }

    public static Vector2 Direction4ToVector(Direction4 direction)
    {
        switch (direction)
        {
            case Direction4.Up:
                return new Vector2(0, 1);
            case Direction4.Right:
                return new Vector2(1, 0);
            case Direction4.Down:
                return new Vector2(0, -1);
            case Direction4.Left:
                return new Vector2(-1, 0);
            default:
                throw new System.Exception("Invalid direction");
        }
    }

    public static Vector2Int Floor(Vector2 vector)
    {
        int x = Mathf.FloorToInt(vector.x);
        int y = Mathf.FloorToInt(vector.y);
        return new Vector2Int(x, y);
    }

	public static Vector3 Vector3(Vector2 vector) => new Vector3(vector.x, vector.y);

	public static Vector3Int Floor(Vector3 vector)
    {
        int x = Mathf.FloorToInt(vector.x);
        int y = Mathf.FloorToInt(vector.y);
        int z = Mathf.FloorToInt(vector.z);
        return new Vector3Int(x, y, z);
    }

    public static Vector2Int Ceil(Vector2 vector)
    {
        int x = Mathf.CeilToInt(vector.x);
        int y = Mathf.CeilToInt(vector.y);
        return new Vector2Int(x, y);
    }

    public static int TilesCountInRange(Tilemap tilemap, TileBase tile, Vector2 position, float radius)
    {
        Vector2Int start = Floor(position - Vector2.one * radius);
        Vector2Int end = Ceil(position + Vector2.one * radius);
        var output = 0;

        for (int x = start.x; x < end.x; x++)
        {
            for (int y = start.y; y < end.y; y++)
            {
                if (!IsPositionInCircle(position, new Vector2(x, y), radius))
                    continue;
                TileBase t = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (t == tile)
                    output++;
            }
        }
        
        return output;
    }
}

public static class ObjectActivator
{
	public static void Activate(this Object obj, bool value)
	{
		if (obj == null)
			throw new System.ArgumentNullException();
		if (obj is GameObject gameObject)
		{
			gameObject.Activate(value);
			return;
		}
		if (obj is Component component)
		{
			component.Activate(value);
			return;
		}
		throw new System.ArgumentException();
	}

	public static void Activate(this GameObject obj, bool value) => obj.SetActive(value);

	public static void Actiavte(this Behaviour component, bool value) => component.enabled = value;
}

public sealed class LineEquation
{
	public float a { get; private set; }
	public float b { get; private set; }
	public float c { get; private set; }

	public LineEquation(float angle, float x, float y)
	{
		angle *= -Mathf.Deg2Rad;
		a = Mathf.Cos(angle);
		b = Mathf.Sin(angle);
		c = -(x * a + y * b);
	}

	public LineEquation(Transform transform) : this(transform.eulerAngles.z, transform.position.x, transform.position.y) { }

	public float Distance(float x, float y) => Mathf.Abs(x * a + y * b + c);

	public float Distance(Transform t) => Distance(t.position.x, t.position.y);

	public override string ToString() => $"{a}x + {b}y + {c} = 0";
}

public abstract class Event<T> where T : Object
{
    [SerializeField] private bool _cancel;
	[SerializeField] private float _rotation;
    [SerializeField] private Vector3 _position;
    [SerializeField] private T _prefab;

    public Event(bool cancel, float rotation, Vector3 position, T prefab)
    {
        _cancel = cancel;
		_rotation = rotation;
        _position = position;
        _prefab = prefab;
    }

    public bool Cancel => _cancel;
	public float Rotation => _rotation;
    public Vector3 Position => _position;
    public T Prefab => _prefab;
}

[System.Serializable]
public sealed class TileEvent : Event<TileBase>
{
	public TileEvent(bool cancel, float rotation, Vector3 position, TileBase prefab) : base(cancel, rotation, position, prefab) { }
}

[System.Serializable]
public sealed class ObjectEvent : Event<Object>
{
	[SerializeField] private bool _inverse;

	public ObjectEvent(bool cancel, bool inverse, float rotation, Vector3 position, Object prefab) : base(cancel, rotation, position, prefab)
	{ _inverse = inverse; }

	public bool Inverse => _inverse;
}

[System.Serializable]
public sealed class ActivationEvent
{
	[SerializeField] private GameObject _object;
	[SerializeField] private bool _inverse;

	public ActivationEvent(GameObject @object, bool inverse)
	{
		_object = @object;
		_inverse = inverse;
	}

	public GameObject Object => _object;
	public bool Inverse => _inverse;
}

public enum Direction4
{
    Up,
    Right,
    Down,
    Left
}