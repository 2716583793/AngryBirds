using UnityEngine;

/// <summary>
/// 绿色小鸟
/// </summary>
public class GreenBird : Bird
{
    /// <summary>
    /// 反弹技能
    /// </summary>
    protected override void ShowSkill()
    {
        base.ShowSkill();
        Vector3 speed = rg.velocity; //获取小鸟速度
        speed.x *= -1.5f; //横向速度反向并加速
        rg.velocity = speed; //其他保持不变
    }
}
