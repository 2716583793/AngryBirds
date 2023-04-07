using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 异步加载
/// </summary>
public class LoadLevelAsync : MonoBehaviour
{
    private void Start()
    {
        Screen.SetResolution(1280, 800, false); //设置打开的窗口大小
        Invoke("Load", 3); //3秒后调用Load方法进入地图界面
    }

    private void Load() //进入地图界面
    {
        SceneManager.LoadSceneAsync(1); //异步加载地图界面
    }
}
