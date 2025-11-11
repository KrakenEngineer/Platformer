using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private Color _unlockedLevelColor;
	[SerializeField] private Color _lockedLevelColor;
	[SerializeField] private Color _completedLevelColor;
	public static MainMenu Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
		OnLevelWasLoaded();
	}

	private void OnLevelWasLoaded()
	{
		LevelLoadingButton[] buttons = FindObjectsByType<LevelLoadingButton>(FindObjectsSortMode.None);
		GlobalData.Load((uint)buttons.Length);
		foreach (var i in buttons)
			i.Initialize();
	}

	public static void Load(uint index)
	{
		if (GlobalData.TryGoToLevel(index))
			SceneManager.LoadScene("Playable");
	}

	public static void Print(string message) => Debug.LogError(message);

	public static Color UnlockedLevelColor => Instance._unlockedLevelColor;
	public static Color LockedLevelColor => Instance._lockedLevelColor;
	public static Color CompletedLevelColor => Instance._completedLevelColor;
}
