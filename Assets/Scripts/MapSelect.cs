using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 地图选择
/// </summary>
public class MapSelect : MonoBehaviour
{
    public int starNum; //规定的可被选择的临界星星个数
    private bool _mapCanSelect; //某个地图是否可被选择

    public GameObject locks; //锁图标
    public GameObject stars; //星星图标

    public GameObject panel; //panel界面
    public GameObject map; //map界面

    public Text starsText; //map(n)上的分数（12/12）

    public int startNum = 1; //开始
    public int endNum = 8; //结束

    private void Start()
    {
        //PlayerPrefs.DeleteAll(); //删除数据

        if (PlayerPrefs.GetInt("totalNum", 0) >= starNum) //若关卡可选择（场景中所有的星星个数大于等于规定的星星个数）
        {
            _mapCanSelect = true; //地图可选择
        }

        if (!_mapCanSelect)
            return;
        //若地图可选择
        locks.SetActive(false); //隐藏锁图标
        stars.SetActive(true); //显示星星图标
        //text显示
        var counts = 0;
        for (var i = startNum; i <= endNum; i++) //遍历
        {
            counts += PlayerPrefs.GetInt("level" + i, 0);
        }

        starsText.text = counts + " / 24";
        // print(counts);
    }

    /// <summary>
    /// 鼠标点击
    /// </summary>
    public void Selected() //点击某个地图块
    {
        if (!_mapCanSelect)
            return;
        panel.SetActive(true); //显示关卡界面
        map.SetActive(false); //取消地图界面
    }

    public void PanelSelect() //点击返回按钮
    {
        panel.SetActive(false); //取消关卡界面
        map.SetActive(true); //显示地图界面
    }
}
