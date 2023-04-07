using UnityEngine;

/// <summary>
/// 地图加载
/// </summary>
public class LoadLevel : MonoBehaviour
{
    public bool isNow = true;

    private void Awake()
    {
        NowLevel();
    }

    private void NowLevel()
    {
        Instantiate(Resources.Load(PlayerPrefs.GetString("nowLevel")));
    }

    private void NextLevel()
    {
        Instantiate(Resources.Load(PlayerPrefs.GetString("nextLevel")));
    }
}
