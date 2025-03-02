using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDamageGroup : MonoBehaviour
{
    [SerializeField]
    private GameObject hpBarGroup;
    [SerializeField]
    private GameObject textGroup;

    private ObjcetPool<UIHpBar> uiHpBarPool = new ObjcetPool<UIHpBar>();
    private ObjcetPool<UIDamageTxt> uiDamageTxtPool = new ObjcetPool<UIDamageTxt>();

    private Dictionary<Monster, UIHpBar> dicMonserHpBar = new Dictionary<Monster, UIHpBar>();
    private const string sHpBarPath = "UI/UIHpBar";
    private const string sDamageTxtPath = "UI/UIDamageTxt";
    public void Init()
    {
        int _nCount = 30;
        uiHpBarPool.Init(sHpBarPath, _nCount, hpBarGroup.transform);
        uiDamageTxtPool.Init(sDamageTxtPath, _nCount, textGroup.transform);

        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

        GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
    }

    public void ShowMonster(int _nDamage, Monster _mob)
    {
        if (!dicMonserHpBar.ContainsKey(_mob))
        {
            UIHpBar _hpBar = uiHpBarPool.Get();
            _hpBar.Init(_mob, (UIHpBar _bar) =>
            {
                uiHpBarPool.Return(_bar);
                dicMonserHpBar.Remove(_bar.monster);
            });
            dicMonserHpBar.Add(_mob, _hpBar);
        }
        else
        {
            dicMonserHpBar[_mob].Init(_mob, (UIHpBar _bar) =>
            {
                uiHpBarPool.Return(_bar);
                dicMonserHpBar.Remove(_bar.monster);
            });
        }

        UIDamageTxt _damageTxt = uiDamageTxtPool.Get();
        _damageTxt.Init(_nDamage, _mob, (UIDamageTxt _damage) =>
        {
            uiDamageTxtPool.Return(_damage);
        });
    }
}
