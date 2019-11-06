using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MushroomEnemyEffect  : MonoBehaviour {

    /// <summary>
    /// 蘑菇敌人的警戒线
    /// </summary>
    public float enemyCordon;
    public float enemySpeed;    //敌人的巡视速度
    public float enemyAddSpeed;     //触发警戒后，追人速度
    private Ray mushroom_Ray;
    private RaycastHit2D hitInfo;
    private GameObject player;
    private Vector3 mushroomPos; //初始位置
    private bool isLeft = false;
    private bool isOnce = false;
    public int hp = 100;


    //学习之前学习到的资源管理容器的方法，建立一个所有的物体方法进去
    private GameObject barragePrefab;   //弹幕的预制体

    /// <summary>
    /// 表示Enemy蘑菇树的三种状态
    /// </summary>
    bool isAttact = false;
    bool isRun = true;
    bool isGoHome = false;

    private static MushroomEnemyEffect _instance;
    public static MushroomEnemyEffect Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        player = GameObject.Find("Player");

        //通过单独的管理器容器类的静态方法，来获取管理器中的对象
        barragePrefab = ManagerVars.GetManagerVars().barragePrefab;

        mushroomPos = transform.position;
    }

    // Use this for initialization
    void Start () {
        _instance = this;
	}
	
	// Update is called once per frame
	void Update ()
    {
        EnemyAI();

    }

    /// <summary>
    /// 怪物受到伤害扣血函数
    /// </summary>
    /// <param name="damage"></param>
    private void MushroomDamage(int damage)
    {
        if (hp>0)
        {
            hp -= damage;

            if (hp<=0)
            {
                Destroy(gameObject);
            }
        }


    }


    /// <summary>
    /// 涉及到AI时，一定要有状态管理器，来管理当前玩家的状态
    /// </summary>
    private void EnemyAI()
    {
        //TODO:敌人AI函数
        //TODO:警戒、加速奔跑、固定方向追人、发射弹幕、弹幕抛物线运动、弹幕落地还会炸成小弹幕
        
        Vector2 directionRay = new Vector2(-10, 0);   //射线方向
        Vector2 directionRay1 = new Vector2(5, 0);   //另外一条射线方向
        Vector2 originRay = this.transform.position;  //射线发射点
        Ray ray = new Ray(originRay,directionRay);

        LayerMask laymask = 1 << 8; //表示只检测第8层
        LayerMask laymask1 = ~(1 << 8); //表示只不检测第8层
        LayerMask laymask2 = (1 << 8)|(1<<9); //表示只检测第8层和第9层
        LayerMask laymask3 = ~(1 << 0); //表示检测所有的层

        //线性射线检测，返回值bool
        bool isRayCayst = Physics2D.Raycast(originRay, directionRay,10,~(1<<LayerMask.NameToLayer("UI")));
        
        //线性射线检测，射线碰到的第一个物体信息
        RaycastHit2D aaa = Physics2D.Raycast(originRay, directionRay, 10);
        //线性射线检测，射线碰撞的所有物体信息数组
        RaycastHit2D[] bbb = Physics2D.RaycastAll(originRay, directionRay, 10);


        //圆形射线检测，返回值bool
        bool isCircleRayCayst = Physics2D.OverlapCircle(originRay,3f, ~(1 << LayerMask.NameToLayer("UI")));

        //圆形射线检测，射线碰撞的所有物体信息数组集合
        Collider2D[] CircleHtiGOs = Physics2D.OverlapCircleAll(originRay,3f, ~(1 << LayerMask.NameToLayer("UI")));


        /// <summary>
        /// 通过对射线检测返回的碰撞信息数组循环遍历，找出符合的collider
        /// </summary>
        for (int i = 0;i<CircleHtiGOs.Length;i++)
        {
            if (CircleHtiGOs[i].gameObject.tag=="Player")
            {
                print("喂！你碰到我了，煞笔！");

                if (!isOnce)
                {
                    for (int j = 0; j < 80; j++)
                    {
                        //进入范围后，蘑菇怪开始放弹幕攻击; 360/80这表示取整！千万记住
                        Instantiate(barragePrefab, transform.position, Quaternion.Euler(0, 0, 0 + (float)360 / 80 * j),gameObject.transform);

                        //这表示的除法的浮点数,表示的是将360转为浮点数；而(float)（360/80），对一个int值去浮点，精度其实后面小数位还是丢了
                        print((float)360 / 80);

                    }

                    isOnce = true;
                }
               

                return;
            }
        }



        // Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask);



        if (isCircleRayCayst)
        {

            //print("xxx");
        }

        if (isRayCayst)
        {
            isAttact = true;
            print(aaa.transform.tag);
        }

        if (Mathf.Abs(transform.position.x - mushroomPos.x) >= 5)
        {
            isGoHome = true;
            isAttact = false;
            isRun = false;
        }

        if (isAttact==true||isGoHome==true)
        {
            isRun = false;
        }
        else
        {
            isRun = true;
        }


        MushroomRun();
        MushRoomAttact();
        MushRoomGoHome();



        Debug.DrawRay(originRay,directionRay,Color.green);
        Debug.DrawRay(originRay, directionRay1, Color.red);

    }

    private void MushroomFire()
    {
        //TODO:蘑菇树发射子弹弹幕
    }


    private void MushroomRun()
    {
        if (!isRun)
        {
            return;
        }

        //TODO：蘑菇自动左右巡逻
        int radios = 3; //巡逻半径
        int defendSpeed = 1;
        //if (transform.position.x- mushroomPos.x >= radios|| transform.position.x - mushroomPos.x<=-3)
        if(Mathf.Abs(transform.position.x - mushroomPos.x)>= radios)
        {
            isLeft = !isLeft;
        }
            
        if(isLeft)
        {
            transform.Translate(Vector3.left * Time.deltaTime * defendSpeed);
        }
        else
        {
            transform.Translate(Vector3.right * Time.deltaTime * defendSpeed);
        }
  
    }

    //画圆形的函数
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, 3);
    }

    private void MushRoomAttact()
    {
        if (!isAttact)
        {
            return;
        }

        //TODO:触发射线检测后，开始狂奔向主角;蘑菇不能脱离领地,到达最大领地范围时，快速回到原来位置处，领地范围+-5
        int runSpeed = 3;
    
        if (player.transform.position.x - transform.position.x < 0)
        {
            transform.Translate(Vector3.left * Time.deltaTime * runSpeed);
        }
        else
        {
            transform.Translate(Vector3.right * Time.deltaTime * runSpeed);
        }

    }


    private void MushRoomGoHome()
    {
        if (!isGoHome)
        {
            return;
        }

        if (Mathf.Abs(transform.position.x - mushroomPos.x) <= 0.1)
        {
            isGoHome = false;
            print("aaaaa ");
            return;
        }

        if (transform.position.x - mushroomPos.x >= 0)
        {
            transform.Translate(Vector3.left * Time.deltaTime * 20);
        }
        else
        {
            transform.Translate(Vector3.right * Time.deltaTime * 20);
        }

  

    }

}
