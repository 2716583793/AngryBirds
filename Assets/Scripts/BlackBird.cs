using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑色小鸟
/// </summary>
public class BlackBird : Bird
{
    public List<Pig> blocks = new List<Pig>(); //存放靠近小鸟的障碍物的集合

    /// <summary>
    /// 范围检测（进）
    /// </summary>
    /// <param name="coll"></param>
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Enemy")) //靠近小鸟
        {
            blocks.Add(coll.gameObject.GetComponent<Pig>()); //加入集合
        }
    }

    /// <summary>
    /// 范围检测（出）
    /// </summary>
    /// <param name="coll"></param>
    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Enemy")) //离开小鸟
        {
            blocks.Remove(coll.gameObject.GetComponent<Pig>()); //退出集合
        }
    }

    /// <summary>
    /// 爆炸技能
    /// </summary>
    protected override void ShowBlackSkill()
    {
        base.ShowBlackSkill();
        if (blocks != null)
        {
            if (blocks.Count > 0)
            {
                for (var i = 0; i < blocks.Count; i++)
                {
                    blocks[i].Dead();
                }
            }
        }
        OnClear();
        enabled = false; //禁用小鸟
        Invoke("Next", 3f);
    }

    /// <summary>
    /// 清楚障碍物
    /// </summary>
    private void OnClear()
    {
        rg.velocity = Vector3.zero;
        Instantiate(boom, transform.position, Quaternion.identity); //爆炸特效
        rd.enabled = false; //
        GetComponent<CircleCollider2D>().enabled = false;
    }

    /// <summary>
    /// 重写Next方法，剔除了小鸟销毁时的动画
    /// </summary>
    protected override void Next()
    {
        GameManager.instance.birds.Remove(this); //移除当前小鸟
        Destroy(gameObject); //销毁当前小鸟
        GameManager.instance.NextBird(); //判定游戏逻辑
    }
}
