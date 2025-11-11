using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelLoadingButton : MonoBehaviour
{
    public void Initialize()
    {
		uint id = GetIndex();
		GetComponent<Button>().onClick.AddListener(delegate { MainMenu.Load(id); });
		transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = id.ToString();
		GetComponent<Image>().color = GlobalData.State(id) switch
		{
			LevelState.Locked => MainMenu.LockedLevelColor,
			LevelState.Unlocked => MainMenu.UnlockedLevelColor,
			LevelState.Completed => MainMenu.CompletedLevelColor
		};
    }

	private uint GetIndex()
	{
		string name = gameObject.name;
		int start = 0;
		int n = name.Length - 1;
		for (int i = n; i >= 0; i--)
			if (name[i] == '(')
			{
				start = i + 1;
				break;
			}
		uint index = 0;
		for (int i = start; i < n; i++)
		{
			index *= 10;
			index += (uint)(name[i] - '0');
		}
		return index;
	}
}
