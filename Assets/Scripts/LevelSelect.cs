using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 关卡选择
/// </summary>
public class LevelSelect : MonoBehaviour
{
    private bool _levelCanSelect; //某个关卡是否可被选择
    public Sprite levelBg; //替换锁图标的背景
    private Image _image; //背景图标
    
    public GameObject[] stars; //关卡完成后的三个星星（左中右）

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        //父亲的第零个孩子（第一个子对象-->某个地图中的第一个关卡）==目前游戏物体的名
        //print("gameObject" + gameObject.name); //关卡1~8 9~16 17~24
        //print("GetChild" + transform.parent.GetChild(0).name); //1 9 17
        if (transform.parent.GetChild(0).name == gameObject.name) //当前关卡为地图的第一关
        {
            _levelCanSelect = true; //关卡可被选择
        }
        else //当前关卡非地图的第一关
        {
            var beforeNum = int.Parse(gameObject.name) - 1; //当前的前一个关卡
            if (PlayerPrefs.GetInt("level" + beforeNum) > 0) //若前一个关卡的星星大于零 
            {
                _levelCanSelect = true; //关卡可被选择
            }
        }

        if (_levelCanSelect) //若关卡可被选择
        {
            _image.overrideSprite = levelBg; //关卡的锁图标替换为新图标
            transform.Find("num").gameObject.SetActive(true); //显示num物体（关卡图表上的数字）
        }

        var count = PlayerPrefs.GetInt("level" + gameObject.name); //获取当前关卡名，获得对应的星星个数
        // print(count);
        if (count <= 0)
            return;
        //星星数量大于零
        for (var i = 0; i < count; i++)
        {
            stars[i].SetActive(true);
        }
    }

    public void Selected()
    {
        if (!_levelCanSelect)
            return;
        //某个关卡可被选择
        PlayerPrefs.SetString("nowLevel", "level" + gameObject.name); //将当前关卡名（level(n)）存入nowLevel

            // print(gameObject.name);
            SceneManager.LoadScene(2); //转到关卡的游戏界面
    }

    public void NextSelected()
    {
        if (!_levelCanSelect)
            return;
        //某个关卡可被选择
    }
}
