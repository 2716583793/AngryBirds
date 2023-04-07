using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏逻辑处理
/// </summary>
public class GameManager : MonoBehaviour
{
    public List<Bird> birds; //存放小鸟的集合
    public List<Pig> pigs; //存放小猪的集合
    public static GameManager instance; //对象实例

    public GameObject win; //
    public GameObject lose; //

    public GameObject[] stars; //存放游戏结算星星的数组

    private int _starsNum; //存放某个关卡的星星的数量

    private const int TotalNum = 24; //总共的关卡数量

    public GameObject[] tails; //小鸟拖尾
    [HideInInspector] public List<GameObject> tailLine; //当前拖尾
    private int _next;

    private int _score; //关卡分数记录
    public Text scoreText; //关卡分数显示

    public GameObject pauseButton;
    public GameObject retryButton;

    public GameObject rb;
    public GameObject hb;
    public GameObject nb;

    public Transform rPos;

    private void Awake()
    {
        instance = this; //当前对象
        Input.multiTouchEnabled = false; //禁止多点触摸
    }

    private void Start()
    {
        Initialized(); //调用初始化方法初始化小鸟
    }

    /// <summary>
    /// 划拖尾
    /// </summary>
    private void SpawnTrail()
    {
        if (birds.Count <= 0)
            return;
        if (!birds[0].isFly)
            return;
        //print(birds[0].GetComponent<Rigidbody2D>().velocity.sqrMagnitude);
        _next = (_next + 1) % tails.Length; //循环打印tail数组
        var newTail = Instantiate(tails[_next], birds[0].transform.position, Quaternion.identity);
        tailLine.Add(newTail);
    }

    private void DropLine()
    {
        for (int i = 0; i < tailLine.Count; i++)
        {
            Destroy(tailLine.ElementAt(i), 2f);
        }
    }

    /// <summary>
    /// 初始化小鸟
    /// </summary>
    private void Initialized()
    {
        for (var i = 0; i < birds.Count; i++) //遍历小鸟
        {
            if (i == 0) //若为第一只小鸟
            {
                var pos = new Vector2(rPos.transform.position.x, rPos.transform.position.y - 0.3f);
                birds[i].transform.position = pos; //小鸟放到这
                //激活
                birds[i].enabled = true; //小鸟
                birds[i].sj.enabled = true; //弹簧
                birds[i].Line(); //初始化时划线
                birds[i].canMove = true; //可以移动
            }
            else //不是第一支小鸟
            {
                //禁用组件
                birds[i].enabled = false;
                birds[i].sj.enabled = false;
                birds[i].canMove = false; //不可移动
            }

            InvokeRepeating("SpawnTrail", 0.05f, 0.05f); //每0.05秒调用一次画拖尾方法
        }
    }

    /// <summary>
    /// 判定游戏逻辑
    /// </summary>
    public void NextBird()
    {
        if (pigs.Count == 0) //猪没了，过关
        {
            Invoke("Win", 2);
            Invoke("SetButtonHide", 0);
        }
        else //猪还有
        {
            if (birds.Count > 0) //鸟还有
            {
                Initialized(); //下一只鸟
                DropLine();
            }
            else //鸟没了，失败
            {
                Invoke("Lose", 2);
                Invoke("SetButtonHide", 0);
            }
        }
    }

    /// <summary>
    /// 影藏按钮
    /// </summary>
    public void SetButtonHide()
    {
        pauseButton.SetActive(false); //隐藏按钮
        retryButton.SetActive(false); //隐藏按钮
        rb.SetActive(false); //隐藏按钮
        hb.SetActive(false); //隐藏按钮
        nb.SetActive(false); //隐藏按钮
    }

    /// <summary>
    /// 显示按钮
    /// </summary>
    public void SetButtonApp()
    {
        rb.SetActive(true); //显示按钮
        hb.SetActive(true); //显示按钮
        nb.SetActive(true); //显示按钮
    }

    /// <summary>
    /// 实时更新分数
    /// </summary>
    private void Update()
    {
        scoreText.text = _score.ToString();
        //print(score);
    }

    /// <summary>
    /// 进行加分
    /// </summary>
    public void AddScore(int i)
    {
        _score += i;
    }

    /// <summary>
    /// 过关
    /// </summary>
    private void Win()
    {
        win.SetActive(true);
    }

    /// <summary>
    /// 失败
    /// </summary>
    private void Lose()
    {
        lose.SetActive(true);
    }

    /// <summary>
    /// 显示星星
    /// </summary>
    public void ShowStars()
    {
        StartCoroutine("Show");
        Invoke("SetButtonApp", 1f);
    }

    /// <summary>
    /// 显示结算页面
    /// </summary>
    private IEnumerator Show()
    {
        for (; _starsNum <= birds.Count; _starsNum++)
        {
            if (_starsNum >= stars.Length) //防止星星数量超过三个
            {
                break; //等于三时退出防止数组下标越界
            }

            yield return new WaitForSeconds(0.2f); //延时0.2秒
            stars[_starsNum].SetActive(true); //显示星星图标
        }
        //print("_starsNum: " + _starsNum); //最后的_starsNum为该关卡获得的星星个数
    }

    /// <summary>
    /// 重新开始
    /// </summary>
    public void Replay()
    {
        SaveData(); //保存数据
        SceneManager.LoadScene(2); //重新开始
    }

    /// <summary>
    /// 回到主页
    /// </summary>
    public void Home()
    {
        SaveData(); //保存数据
        SceneManager.LoadScene(1); //回到主页
    }

    /// <summary>
    /// 下一关
    /// </summary>
    public void Next()
    {
        SaveData(); //保存数据
        //将下一关的关卡名（level(n+1)）存入nowLevel
        SceneManager.LoadScene(1);
        var n = int.Parse(PlayerPrefs.GetString("nowLevel").Substring(5, 1));
        PlayerPrefs.SetString("nowLevel", "level" + (1 + n));
        //print(PlayerPrefs.GetString("nowLevel"));
        SceneManager.LoadScene(2);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    private void SaveData()
    {
        //当前关卡星星数量与当前关卡存储的历史最高星星数量
        var num = PlayerPrefs.GetInt(PlayerPrefs.GetString("nowLevel"));
        if (_starsNum > num)
        {
            //取最大的进行存储
            PlayerPrefs.SetInt(PlayerPrefs.GetString("nowLevel"), _starsNum);
        }

        //所有的星星数量
        var sum = 0;
        for (var i = 1; i <= TotalNum; i++) //统计每个关卡的总星数
        {
            sum += PlayerPrefs.GetInt("level" + i);
        }

        PlayerPrefs.SetInt("totalNum", sum); //将星星总数存入totalNum
        //print("totalNum: " + PlayerPrefs.GetInt("totalNum"));
    }
}