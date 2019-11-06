using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 标签 [System.Serializable] 是指对该类进行串行化示例
/// 串行化是指存储和获取磁盘文件、内存或其他地方中的对象。在串行化时，所有的实例数据都保存到存储介质上，在取消串行化时，对象会被还原，且不能与其原实例区别开来
/// 只需给类添加Serializable属性，就可以实现串行化实例的成员。
/// </summary>
[System.Serializable]
public class GameDate {

    //上面为将要存储的所有本地数据
    private bool isWine;
    private int life;
    private int hp;
    private bool isFirstGame;
    


    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetIsWine(bool isWine)
    {
        this.isWine = isWine;
    }
    public void SetLife(int life)
    {
        this.life = life;
    }
    public void SetHp(int hp)
    {
        this.hp = hp;
    }
    public void SetIsFirstGame(bool  isFirstGame)
    {
        this.isFirstGame = isFirstGame;
    }

    /// <summary>
    /// 获取数据
    /// </summary>
    public bool GetIsWine()
    {
        return this.isWine;
    }
    public int  GetLife()
    {
        return this.life;
    }
    public int  GetHp()
    {
        return this.hp;
    }
    public bool GetIsFirstGame()
    {
        return this.isFirstGame;
    }


}
