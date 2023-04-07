using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 暂停界面
/// </summary>
public class PausePanel : MonoBehaviour
{
    private Animator _animator;
    
	public GameObject pauseButton;
    public GameObject retryButton;

	public Text gameText; //当前关卡名（1-1）

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 点击暂停按钮
    /// </summary>
    public void Pause()
    {
        // print("pause");
        _animator.SetBool("isPause", true); //isPause设置为true
        SetButtonHide();

		int n = int.Parse((PlayerPrefs.GetString("nowLevel")).Substring(5, 1));
		int a = n / 8;
		int b = n % 8;
		gameText.text = ((1 + a) + " - " + b);
		//print(gameText.text);
        
        if (GameManager.instance.birds.Count <= 0)
            return;
        if (!GameManager.instance.birds[0].isOriginal)
            return;
        GameManager.instance.birds[0].canMove = false; //现存鸟的数量大于零//第一只鸟是初始状态//当前鸟设置为不可移动
    }

    /// <summary>
    /// 点击继续按钮
    /// </summary>
    public void Resume()
    {
        // print("resume");
        Time.timeScale = 1; //恢复动画
        _animator.SetBool("isPause", false); //isPause设置为false
        
        if (GameManager.instance.birds.Count <= 0)
            return;
        if (!GameManager.instance.birds[0].isOriginal)
            return;
        GameManager.instance.birds[0].canMove = true;
        //现存鸟的数量大于零//第一只鸟是初始状态//当前鸟恢复为可移动
    }

	public void SetButtonHide()
    {
        pauseButton.SetActive(false); //隐藏按钮
        retryButton.SetActive(false); //隐藏按钮
    }

    /// <summary>
    /// pause动画播放完
    /// </summary>
    public void PauseAnimEnd()
    {
        // print("pauseEnd");
        Time.timeScale = 0; //暂停动画
    }

    /// <summary>
    /// resume动画播放完
    /// </summary>
    public void ResumeAnimEnd()
    {
        // print("resumeEnd");
        pauseButton.SetActive(true); //显示按钮
        retryButton.SetActive(true); //显示按钮
    }

    /// <summary>
    /// 点击重新开始按钮
    /// </summary>
    public void Retry()
    {
        //print("retry");
        Time.timeScale = 1;
        SceneManager.LoadScene(2); //重新开始
    }

    /// <summary>
    /// 点击主页按钮
    /// </summary>
    public void Home()
    {
        //print("home");
        Time.timeScale = 1;
        SceneManager.LoadScene(1); //回到主页
    }
}
