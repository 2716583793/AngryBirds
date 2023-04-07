using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 小鸟
/// </summary>
public class Bird : MonoBehaviour
{
    [HideInInspector] public bool isClick; //鼠标是否按下
    private const float MaxDis = 1.5f; //弹弓拉伸最大距离
    private const float MinDis = 0.8f; //弹弓拉伸最小距离

    [HideInInspector] public SpringJoint2D sj; //弹簧
    protected Rigidbody2D rg; //2D刚体

    //弹弓划线组件
    public Transform rightPos;
    public Transform leftPos;

    public LineRenderer right; //划线渲染
    public LineRenderer left; //划线渲染
    
    public GameObject boom; //销毁

    [HideInInspector] public bool canMove; //是否能移动
    private const float Smooth = 10; //光滑程度

    public AudioClip selectClip; //小鸟被选中音效
    public AudioClip flyClip; //小鸟飞行音效
    public AudioClip hurtClip; //小鸟受伤音效

    [HideInInspector]public bool isFly; //小鸟是否为飞行状态
    [HideInInspector] public bool isOriginal = true; //是否为初始状态
    
    public Sprite hurt; //小鸟受伤图片
    protected SpriteRenderer rd; //小鸟图片渲染
    
    /// <summary>
    /// 唤醒事件
    /// </summary>
    /// <returns></returns>
    private void Awake()
    {
        //创建对象
        sj = GetComponent<SpringJoint2D>(); //弹簧
        rg = GetComponent<Rigidbody2D>(); //刚体
        rd = GetComponent<SpriteRenderer>(); //渲染
    }

    /// <summary>
    /// 鼠标按下
    /// </summary>
    private void OnMouseDown()
    {
        if (!canMove) //小鸟不能移动--忽略鼠标事件
            return;
        //小鸟能够移动
        AudioPlay(selectClip); //播放小鸟被选中时的音效
        isClick = true; //记录鼠标被按下
        rg.isKinematic = true; //启动动力学（物体受到力的影响）
    }

    /// <summary>
    /// 鼠标抬起
    /// </summary>
    private void OnMouseUp()
    {
        if (!canMove) //小鸟不能移动--忽略鼠标事件
            return;
        //小鸟能够移动
        isClick = false; //记录鼠标被抬起
        rg.isKinematic = false; //关闭动力学
        Invoke("Fly", 0.1f); //0.1秒后调用Fly方法
        
        right.enabled = false; //禁用划线组件
        left.enabled = false;
        canMove = false; //不能够移动
    }

    /// <summary>
    /// 更新操作
    /// </summary>
    private void Update()
    {
        //print(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //print(isClick);
        if (EventSystem.current.IsPointerOverGameObject()) //判断是否点击UI
            return;
        if (isClick) //鼠标按下时
        {
            Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //小鸟跟着鼠标移动
            transform.position = mp; //（鼠标的）屏幕坐标--转换为世界坐标
            //transform.position += new Vector3(0, 0, -Camera.main.transform.position.z); //关于z轴2

            //print(Vector3.Distance(transform.position, rightPos.position));
            
            if (Vector2.Distance(transform.position, rightPos.position) > MaxDis) //弹弓最大移动位置限定
            {
                var pos = (transform.position - rightPos.position).normalized; //单位化向量

                //print(pos);
                
                pos *= MaxDis; //最大长度向量
                transform.position = pos + rightPos.position; //限制位置
            } else if (Vector3.Distance(transform.position, rightPos.position) < MinDis)
            {
                //print("拉伸太短，弹回原位");
            }
            Line(); //调用弹簧划线方法
        }

        //相机跟随
        var posX = transform.position.x; //获取小鸟的x坐标
        var v = new Vector3(Mathf.Clamp(posX, 0, 9), Camera.main.transform.position.y,
            Camera.main.transform.position.z); //限定范围内的坐标
        Camera.main.transform.position =
            Vector3.Lerp(Camera.main.transform.position, v, Smooth * Time.deltaTime); //限定跟随速度的移动
        
        //使用技能
        if (!Input.GetMouseButtonDown(0))
            return;
        //鼠标左键按下
        if (transform.GetComponent<Bird>() is BlackBird) //当前为黑鸟
        {
            if (!isOriginal) //非初始状态点击鼠标
            {
                ShowBlackSkill(); //使用黑鸟技能
            }
        }
        else //当前不是黑鸟
        {
            if (isFly) //飞行时点击鼠标
            {
                ShowSkill(); //使用小鸟技能
            }
        }
    }

    /// <summary>
    /// 起飞后
    /// </summary>
    private void Fly()
    {
        isFly = true; //飞行状态
        isOriginal = false; //飞行后为非初始状态

        AudioPlay(flyClip); //播放小鸟飞行音效

        sj.enabled = false; //禁用弹簧组件

        //是黑鸟5秒后调用ShowBlackSkill方法//不是黑鸟5秒后调用Next方法
        Invoke((transform.GetComponent<Bird>() is BlackBird) ? "ShowBlackSkill" : "Next", 5);
    }

    /// <summary>
    /// 划弹弓的线
    /// </summary>
    public void Line()
    {
        //激活划线组件
        right.enabled = true;
        left.enabled = true;
        //右
        var v = transform.position;
        right.SetPosition(0, rightPos.position);
        right.SetPosition(1, v);
        //左
        left.SetPosition(0, leftPos.position);
        left.SetPosition(1, v);
    }

    /// <summary>
    /// 处理下一只小鸟飞出
    /// </summary>
    protected virtual void Next()
    {
        GameManager.instance.birds.Remove(this); //从集合中移除当前小鸟
        Destroy(gameObject); //销毁当前小鸟
        Instantiate(boom, transform.position, Quaternion.identity); //显示小鸟销毁动画
        GameManager.instance.NextBird(); //判定游戏逻辑
    }

    /// <summary>
    /// 小鸟碰撞事件
    /// </summary>
    private void OnCollisionEnter2D(Collision2D coll)
    {
        isFly = false; //取消飞行状态
        if (isOriginal) //小鸟为初始状态--忽略碰撞影响
            return;
        //小鸟为飞出状态
        Hurt(); //小鸟受伤
        AudioPlay(hurtClip); //播放小鸟受伤音效
    }

    /// <summary>
    /// 使用小鸟技能
    /// </summary>
    public virtual void ShowSkill()
    {
        isFly = false; //取消飞行状态
    }

    /// <summary>
    /// 使用黑鸟技能
    /// </summary>
    public virtual void ShowBlackSkill()
    {
        isFly = false; //取消飞行状态
    }
    
    /// <summary>
    /// 小鸟受伤
    /// </summary>
    public void Hurt()
    {
        rd.sprite = hurt; //修改图片为受伤状态
    }
    
    /// <summary>
    /// 添加音效
    /// </summary>
    /// <param name="clip">指定音效</param>
    public void AudioPlay(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position); //添加指定音效
    }
}