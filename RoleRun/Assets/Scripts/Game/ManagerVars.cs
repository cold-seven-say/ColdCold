using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 单独的管理器容器类，存储了游戏中要用的信息；所有的信息都可以放到这个容器中
/// </summary>

//在Assets-Create-下创建一个菜单项，名字叫做CreatManagerVarsContainer
[CreateAssetMenu(menuName ="CreatManagerVarsContainer")]
public class ManagerVars : ScriptableObject {

    public static ManagerVars GetManagerVars()
    {
        return Resources.Load<ManagerVars>("ManagerVarsContainer");
    }

    
    public GameObject barragePrefab;    //金币弹幕预制体
    public GameObject gressBarrage;   //苍蝇弹幕预制体

    public GameObject bulletPrefab; //主角子弹预制体



}
