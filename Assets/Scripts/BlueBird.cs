using System;
using UnityEngine;

/// <summary>
/// 蓝色小鸟
/// </summary>
public class BlueBird : Bird
{
    /// <summary>
    /// 分身技能
    /// </summary>
    protected override void ShowSkill()
    {
        base.ShowSkill();
        //速度
        Vector3 speed = rg.velocity *= 1.2f; //获取小鸟速度(加速1.2)
        //print("speed：" + speed);
        var x = speed.x;
        var y = speed.y;
        var s = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2)); //速度值
        //print("s：" + s);
        var deg = 0f; //角度
        if (x > 0 && y >= 0) //第一象限
        {
            var rad = Mathf.Atan(y / x);
            deg = rad * 180 / Mathf.PI;
        }
        else if (x <= 0 && y > 0) //第二象限
        {
            var rad = Mathf.Atan(-x / y);
            deg = rad * 180 / Mathf.PI + 90;
        }
        else if (x < 0 && y <= 0) //第三象限
        {
            var rad = Mathf.Atan(-y / -x);
            deg = rad * 180 / Mathf.PI + 180;
        }
        else if (x >= 0 && y < 0) //第四象限
        {
            var rad = Mathf.Atan(x / -y);
            deg = rad * 180 / Mathf.PI + 270;
        }
        //print("deg：" + deg); //与x轴角度（0~360）
        //角度
        const float r = 10f;
        var deg1 = (deg + r + 360) % 360; //分身1角度
        var deg2 = (deg - r + 360) % 360; //分身2角度
        //print("deg1：" + deg1);
        //print("deg2：" + deg2);
        //弧度
        var rad0 = Mathf.PI * deg / 180;
        var rad1 = Mathf.PI * deg1 / 180;
        var rad2 = Mathf.PI * deg2 / 180;
        
        //小鸟分身坐标
        const float d = 0.3f;
        var x0 = transform.position.x;
        var y0 = transform.position.y;
        var pos1 = new Vector3(x0 - d * Mathf.Sin(rad0), y0 + d * Mathf.Cos(rad0), transform.position.z);
        var pos2 = new Vector3(x0 + d * Mathf.Sin(rad0), y0 - d * Mathf.Cos(rad0), transform.position.z);
        //创建分身
        b1 = Instantiate(this, pos1, Quaternion.identity);
        b2 = Instantiate(this, pos2, Quaternion.identity);
        //赋予速度
        rg.velocity = speed;
        b1.rg.velocity = new Vector2(Mathf.Cos(rad1) * s, Mathf.Sin(rad1) * s);
        b2.rg.velocity = new Vector2(Mathf.Cos(rad2) * s, Mathf.Sin(rad2) * s);
        //print("b1：" + b1.rg.velocity);
        //print("b2：" + b2.rg.velocity);
        isShowBlue = true; //使用蓝鸟了技能
    }
}