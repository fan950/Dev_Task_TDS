using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIHpBar : MonoBehaviour
{
    [SerializeField]
    private Image hpBar;
    private RectTransform rectTransform;

    [HideInInspector]
    public Monster monster;
    private float fLiveTime = 0;
    private const float fLive = 1.5f;
    private Action<UIHpBar> action;

    public void Init(Monster _mob,Action<UIHpBar> _action)
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        monster = _mob;
        fLiveTime = 0;
        action = _action;

        gameObject.SetActive(true);
    }

    public void Update()
    {
        fLiveTime += Time.deltaTime;
        if (fLiveTime >= fLive || monster.nHp <= 0)
        {
            if (action != null)
                action(this);
            fLiveTime = 0;
            return;
        }

        hpBar.fillAmount = monster.nHp / (monster.nMaxHp * 1.0f);
        UIManager.Instance.SetObjPos(rectTransform, monster.headCollider.gameObject);
    }
}
