public static class GlobalData
{
	private static uint _currentLevel;
	private static LevelState[] _levelStates;

	public static void Save() => Saver.Save(_levelStates);
	public static void Load(uint levels) => _levelStates = Saver.Load(levels);

	public static void CompleteLevel(uint id)
	{
		_levelStates[id] = LevelState.Completed;
		if (id >= _levelStates.Length - 1)
			return;
		_currentLevel = id + 1;
		if (_currentLevel < _levelStates.Length && _levelStates[_currentLevel] != LevelState.Completed)
			_levelStates[_currentLevel] = LevelState.Unlocked;
		Save();
	}

	public static bool TryGoToLevel(uint id)
	{
		if (!Unlocked(id))
		{
			MainMenu.Print($"Level {id} is locked");
			return false;
		}
		_currentLevel = id;
		return true;
	}

	public static LevelState State(uint id) => _levelStates[id];
	public static bool Unlocked(uint id) => id < _levelStates.Length && _levelStates[id] != LevelState.Locked;

	public static uint CurrentLevel => _currentLevel;
}
