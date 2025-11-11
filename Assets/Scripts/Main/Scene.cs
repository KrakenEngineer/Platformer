using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapRenderer))]
[RequireComponent(typeof(TilemapCollider2D))]
public sealed class Scene : MonoBehaviour
{
	private static Player _player;
	private static Level[] _levels;
	public Tilemap Tilemap { get; private set; }

	private void Awake()
	{
		Tilemap = GetComponent<Tilemap>();
		_player = FindAnyObjectByType<Player>();
		_player.Initialize();
		OnLevelWasLoaded();
	}

    private void OnLevelWasLoaded()
    {
		Level[] levels = Object.FindObjectsByType<Level>(FindObjectsSortMode.None);
		_levels = new Level[levels.Length];
		foreach (var level in levels)
		{
			level.Initialize(this);
			if (level.ID >= _levels.Length || _levels[level.ID])
				throw new System.Exception($"Invalid id {level.ID} of {_levels.Length}");
			_levels[level.ID] = level;
		}
		_player.transform.position = VectorUtilities.Vector3(_levels[GlobalData.CurrentLevel].Position);
	}

    public static void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	public static void ReturnToMenu() => SceneManager.LoadScene("MainMenu");
}