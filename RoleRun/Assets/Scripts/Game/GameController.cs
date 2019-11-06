using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameController : MonoBehaviour {

    private Rigidbody2D m_Rig;
    private float timelost = 0;
    private float jumpForce;
    private float moveSpeed;
    private bool isJump = false;
    private bool isTwoJump = false;
    private bool isSkying = false;
    private float fireCD = 1f;  //发射子弹间隔
    private float timeCount = 0f;
    private GameObject bullet;
    private bool shiyan;    //为了试验GitHub


    //储存的主角属性
    private bool isWine;
    private int life;
    private int hp;
    private bool isFirstGame;

    private GameDate date;

    /// <summary>
    /// 是否开始倒计时
    /// </summary>
    private bool isTiming = false;
    private float countDown;    //倒计时计时

    private float fallMultiplier = 2.5f;
    private float lowMultiplier = 2f;
    private float jumpvalue;

    private void Awake()
    {
        //实例一个游戏数据类对象
        date = new GameDate();

        m_Rig = this.GetComponent<Rigidbody2D>();
        bullet = ManagerVars.GetManagerVars().bulletPrefab;

        jumpForce = 300f;
        moveSpeed = 5f;

        InitGameDate();
    }

    // Use this for initialization
    void Start () {
       
	}

    //如果选择了rigidbody2d.velocity函数，这个函数是自带的物理效果，防止卡顿抖动的话，用fixedUpdate来更新
    private void FixedUpdate()
    {

        //Jump();
        LeftWalk();
        RightWalk();
        DoubleClickJump();
        AireJump();
        OnceClickJump();
    }

    /// <summary>
    /// 初始化游戏数据
    /// </summary>
    private void InitGameDate()
    {
        ReadDate();

        //如果读取到了数据；ReadDate()当读取这个文件为空时，则会让date对象为空
        if (date!=null)
        {
            isFirstGame = date.GetIsFirstGame();
        }
        else
        {
            isFirstGame = true;
        }

        //如果是第一次开始游戏，对游戏数据进行初始化
        if (isFirstGame)
        {
            life = 3;
            hp = 10;
            isWine = false;

            SaveDate();
        }
        else
        {
            isWine = date.GetIsWine();
            hp = date.GetHp();
            life = date.GetLife();

        }

    }



    /// <summary>
    /// 写入/储存数据
    /// </summary>
    private void SaveDate()
    {

        try
        {
            //需要引入对应的命名空间
            //BinaryFormatter以二进制格式序列化和反序列化对象。
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Create(Application.persistentDataPath+"/GameDate.date")) //创建一个文件，名字，格式
            {
                date.SetIsWine(isWine);
                date.SetHp(hp);
                date.SetLife(life);
                date.SetIsFirstGame(isFirstGame);

                //将这个类序列化写入本地
                //Serialize(Stream, Object)	将对象序列化到给定的流
                bf.Serialize(fs,date);
            }
        }

        catch(System.Exception e)
        {
            //print(e);
            Debug.Log(e.Message);
        }

    }

    /// <summary>
    /// 读取数据
    /// </summary>
    private void ReadDate()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Open(Application.persistentDataPath+"/GameDate.date",FileMode.Open))
            {
                //Deserialize(Stream)	将指定的流反序列化成对象
                date = (GameDate)bf.Deserialize(fs);


            }

        }
        catch (System.Exception e)
        {

            Debug.Log(e.Message);

        }

    }


    private void OnFire()
    {
        timeCount += Time.deltaTime;
        if (timeCount>=fireCD&&Input.GetKeyDown(KeyCode.J))
        {
            //发射子弹
            Instantiate(bullet,transform.position,transform.rotation,gameObject.transform);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground"||collision.transform.tag=="Wood")
        {
            isJump = false;
            isSkying = false;
            isTwoJump = false;
        }
    }

    //空气中，可以再次跳跃一次
    private void AireJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isSkying == true&&isTwoJump==false)
        {
            if (transform.localScale.x == -1f)
            {
                m_Rig.velocity = new Vector2(-2, 5);

            }
            else
            {
                m_Rig.velocity = new Vector2(2, 10);
            }
            isTwoJump = true;
            print("ccc");
        }
        //BestJump();
    }

    private void DoubleClickJump()
    {
        //0.5s内，按键>1次才会生效
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Time.time - timelost < 0.5f)///0.5秒之内按下有效  
            {
                m_Rig.velocity = new Vector2(2, 10);  
            }

            timelost = Time.time;
        }
    }

    private void OnceClickJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isJump == false)
        {
            isJump = true;
            isSkying = true;
            if (transform.localScale.x == -1f)
            {
                m_Rig.velocity = new Vector2(-2, 5);

            }
            else
            {
                m_Rig.velocity = new Vector2(2, 5);
            }
        }
        //BestJump();

    }


    /// <summary>
    /// 没有用该方法
    /// </summary>
    private void BestJump()
    {
 
         if (m_Rig.velocity.y < 0)
        {
            m_Rig.velocity += Vector2.up * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (m_Rig.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            m_Rig.velocity += Vector2.up * (lowMultiplier - 1) * Time.deltaTime;
        }

    }

    private void LeftWalk()
    {
        if (Input.GetKey(KeyCode.A) && isJump == false)
        {
            transform.localScale = new Vector3(-1,1,1);
            transform.Translate(Vector3.left*Time.deltaTime*moveSpeed);
        }
    }

    private void RightWalk()
    {
        if (Input.GetKey(KeyCode.D) && isJump == false)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);

            //m_Rig.velocity = new Vector2(4, 0);
        }
    }



    /// <summary>
    /// 没有用该方法
    /// </summary>
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            isJump = true;
            m_Rig.AddForce(new Vector2(200, 500));
        }

    }

}
