using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public sealed class EventHandler : MonoBehaviour
{
	[SerializeField] private bool _enabledByDefault = false;
    [SerializeField] private bool _enabled = false;
    [SerializeField] private List<ObjectEvent> _objects;
    [SerializeField] private List<TileEvent> _tiles;
	[SerializeField] private List<ActivationEvent> _objectsToActivate;

	private Tilemap _tilemap;
    private Dictionary<Vector3Int, TileEvent> _startTiles;
    private List<GameObject> _temporalObjects = new List<GameObject>();

    public void Initialize(Scene scene)
    {
        _tilemap = scene.Tilemap;

        _startTiles = new Dictionary<Vector3Int, TileEvent>();
        foreach (var tile in _tiles)
        {
            Vector3Int position = VectorUtilities.Floor(tile.Position);
            var newTile = new TileEvent(tile.Cancel, tile.Rotation, tile.Position, _tilemap.GetTile<TileBase>(position));
            _startTiles.Add(position, newTile);
        }

		if (_enabledByDefault)
		{
			_enabled = false;
			TryExecuteEvents();
		}
		else
		{
			_enabled = true;
			TryCancelEvents();
		}
	}

    public bool TryExecuteEvents()
    {
        if (_enabled) return false;

        foreach (var tile in _tiles)
            _tilemap.SetTile(VectorUtilities.Floor(tile.Position), tile.Prefab);

		foreach (var obj in _temporalObjects)
			Object.Destroy(obj);
		_temporalObjects.Clear();

        foreach (var obj in _objects)
        {
			if (obj.Inverse)
				continue;
			var newObject = Object.Instantiate(obj.Prefab, obj.Position, Quaternion.Euler(0, 0, obj.Rotation)) as GameObject;
            if (obj.Cancel)
                _temporalObjects.Add(newObject);
        }

		foreach (var obj in _objectsToActivate)
			obj.Object.Activate(!obj.Inverse);

        _enabled = true;
        return true;
    }

    public bool TryCancelEvents()
    {
        if (!_enabled) return false;

        foreach (var tile in _startTiles)
            if (tile.Value.Cancel)
                _tilemap.SetTile(tile.Key, tile.Value.Prefab);

		foreach (var obj in _temporalObjects)
            Object.Destroy(obj);
        _temporalObjects.Clear();

		foreach (var obj in _objects)
		{
			if (!obj.Inverse)
				continue;
			var newObject = Object.Instantiate(obj.Prefab, obj.Position, Quaternion.Euler(0, 0, obj.Rotation)) as GameObject;
			if (obj.Cancel)
				_temporalObjects.Add(newObject);
		}

		foreach(var obj in _objectsToActivate)
			obj.Object.Activate(obj.Inverse);

        _enabled = false;
        return true;
    }
}