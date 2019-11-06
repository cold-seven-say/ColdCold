using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class barrageEffect : MonoBehaviour {

    public float moveSpeed = 4f;
    public float scaleSpeed = 0f;
    private GameObject gressBarrage;
    private bool isBoom = false;
    private bool isOnce = false;
    private Vector3 boomPos;

    private Transform gressBarrageTransform; //存放所有苍蝇弹幕的收纳物体

    private void Awake()
    {

        // 通过单独的管理器容器类的静态方法，获取其中存储的对象预制体信息
        gressBarrage = ManagerVars.GetManagerVars().gressBarrage;
        gressBarrageTransform = GameObject.Find("GressBarrageTransform").GetComponent<Transform>();

    }

    // Use this for initialization
    void Start () {
        
    }


    // Update is called once per frame
    void Update () {

        scaleSpeed += 0.1f;
        //朝自身左方向移动
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.Self);
        transform.localScale = new Vector3(0.2f + scaleSpeed * Time.deltaTime, 0.2f + scaleSpeed * Time.deltaTime, 0.2f + scaleSpeed * Time.deltaTime);

        BarrageBoom();
    }

    /// <summary>
    /// 碰到空气墙后，删除自身;碰到木头，炸开弹幕
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Obstruct"|| collision.tag == "Wood")
        {
            Destroy(gameObject,0.01f);

        }


        if (collision.tag == "Wood")
        {
            isBoom = true;
            boomPos.x = transform.position.x;
            boomPos.y = transform.position.y;
        }

    }

    private void BarrageBoom()
    {
        //这种情况下，只能用射线检测了

        if (isBoom)
        {
            if (!isOnce)
            {
                int num = 40;
                float radius = 1f;
                for (int j = 0; j <num ; j++)
                {
                    float randianValue = (float)360 / num/180 *j*(float)Math.PI;//求弧度值
                    Vector3 newPos = new Vector3((float)Math.Cos(randianValue)* radius + boomPos.x, (float)Math.Sin(randianValue) * radius + boomPos.y,0); 

                    Instantiate(gressBarrage, newPos, Quaternion.Euler(0, 0, 0 + (float)360 / num * j), gressBarrageTransform);

                    //这表示的除法的浮点数
                    print((float)360 /80);
                }

                isOnce = true;
            }
            
        }
    }

}
