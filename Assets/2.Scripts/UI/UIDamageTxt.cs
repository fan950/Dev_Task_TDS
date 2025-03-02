using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class UIDamageTxt : MonoBehaviour
{
    [HideInInspector]
    public Monster monster;
    [SerializeField]
    private Text damageTxt;
    private RectTransform rectTransform;
    private Action<UIDamageTxt> action;

    public void Init(int _nDamage, Monster _mob, Action<UIDamageTxt> _action)
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        damageTxt.text = _nDamage.ToString();
        monster = _mob;

        action = _action;
        gameObject.SetActive(true);
    }

    public void ActiveOff()
    {
        if (action != null)
            action(this);
    }

    public void Update()
    {
        UIManager.Instance.SetObjPos(rectTransform, monster.headCollider.gameObject);
    }
}
