using UnityEngine;

public class Win : MonoBehaviour
{
    /// <summary>
    /// 显示星星
    /// </summary>
    public void Show()
    {
        GameManager.instance.ShowStars(); //调用显示星星方法
    }
}
