using UnityEngine;

/// <summary>
/// 蓝色小鸟
/// </summary>
public class BlueBird : Bird
{
    public override void ShowSkill()
    {
        base.ShowSkill();
        Vector3 speed = rg.velocity *= 1.2f; //获取小鸟速度(加速1.2)
        //小鸟分身坐标
        Vector3 v1 = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Vector3 v2 = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        //创建分身
        BlueBird b1 = Instantiate(this, v1, Quaternion.identity);
        BlueBird b2 = Instantiate(this, v2, Quaternion.identity);
        //赋予速度
        rg.velocity = speed;
        b1.rg.velocity = speed;
        b2.rg.velocity = speed;
        b1.enabled = false;
        b2.enabled = false;
    }
}
