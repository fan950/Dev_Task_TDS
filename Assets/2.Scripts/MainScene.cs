using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public void Awake()
    {
        MonsterManager.Instance.Init();
        UIManager.Instance.Init();
        FxManager.Instance.Init();
        BulletManager.Instance.Init();
    }
}
