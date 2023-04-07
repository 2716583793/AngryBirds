using UnityEngine;

/// <summary>
/// 死亡动画
/// </summary>
public class Boom : MonoBehaviour
{
    /// <summary>
    /// 销毁对象
    /// </summary>
    public void Destroying()
    {
        Destroy(gameObject); //动画播放完后销毁对象
    }
}
