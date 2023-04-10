using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 小猪（障碍物）
/// </summary>
public class Pig : MonoBehaviour
{
    public float maxSpeed = 6f; //死亡速度阈值
    public float minSpeed = 2f; //受伤速度阈值
    private SpriteRenderer _renderer;
    public Sprite hurt; //受伤
    public GameObject boom; //死亡
    public List<GameObject> scoreImg; //死亡获得的分数

    public int score; //自身分数值

    public bool isPig; //标记区分猪和障碍物

    public AudioClip hurtClip; //受伤音效
    public AudioClip deadClip; //死亡音效

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 碰撞后
    /// </summary>
    private void OnCollisionEnter2D(Collision2D coll)
    {
        // print("pig" + coll.relativeVelocity.magnitude);
        if (coll.relativeVelocity.magnitude >= maxSpeed ||
            _renderer.sprite == hurt && coll.relativeVelocity.magnitude >= minSpeed) //直接死亡
        {
            Dead(); //死亡调用Dead方法销毁对象并统计分数
        }
        else if (coll.relativeVelocity.magnitude < maxSpeed &&
                 coll.relativeVelocity.magnitude >= minSpeed) //受伤
        {
            AudioPlay(hurtClip); //受伤音效
            _renderer.sprite = hurt; //更改图标为受伤后的
        }
    }

    /// <summary>
    /// 死亡时
    /// </summary>
    public void Dead()
    {
        if (isPig) //如果是猪死亡
        {
            GameManager.instance.pigs.Remove(this); //移走猪-->用于判定猪是否全部死亡
        }
        Destroy(gameObject); //销毁对象
        
        GameManager.instance.AddScore(score); //根据自身分数传给manager进行加分

        Instantiate(boom, transform.position, Quaternion.identity); //播放死亡动画
        AudioPlay(deadClip); //播放死亡音效
        
        var rd = new System.Random();
        var index =rd.Next(scoreImg.Count); //获取分数对象下标
        //print("index" + index);

        var go = Instantiate(scoreImg[index], transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity); //显示分数动画
        Destroy(go, 1.5f); //1.5秒后销毁分数对象
    }

    /// <summary>
    /// 添加音效
    /// </summary>
    /// <param name="clip">指定音效</param>
    private void AudioPlay(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position); //播放指定音效
    }
}
