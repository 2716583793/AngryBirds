/// <summary>
/// 黄色小鸟
/// </summary>
public class YellowBird : Bird
{
    /// <summary>
    /// 加速技能
    /// </summary>
    protected override void ShowSkill()
    {
        base.ShowSkill();
        rg.velocity *= 2; //速度乘二
    }
}
