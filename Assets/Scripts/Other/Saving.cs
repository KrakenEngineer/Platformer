using System.IO;
using UnityEngine;

public class Saver : MonoBehaviour
{
	public static readonly string SavePath = "progress.txt";

	public static void Save(LevelState[] completed)
	{
		string content = "";
		for (int i = 0; i < completed.Length; i++)
			content += (byte)completed[i];
		File.WriteAllText(SavePath, content);
	}

	public static LevelState[] Load(uint len)
	{
		string content = "";
		if (File.Exists(SavePath))
		{
			string[] lines = File.ReadAllLines(SavePath);
			content = lines.Length == 0 ? "" : lines[0];
		}

		var result = new LevelState[len];
		int newLen = Mathf.Min((int)len, content.Length);
		bool fixSave = newLen != len;

		if (fixSave)
			result[newLen] = LevelState.Unlocked;
		result[0] = content == "" ? LevelState.Unlocked : (LevelState)(content[0] - '0');

		for (int i = 1; i < newLen; i++)
			result[i] = (LevelState)(content[i] - '0');
		for (int i = newLen + 1; i < len; i++)
			result[i] = LevelState.Locked;

		if (fixSave)
			Save(result);
		return result;
	}
}
