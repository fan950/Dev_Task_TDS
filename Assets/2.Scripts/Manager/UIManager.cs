using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    public RectTransform uiCanvasRect;
    [SerializeField]
    public Camera uiCamera;

    private UIDamageGroup uiDamageGroup;

    private const string sUIDamageGroupPath = "UI/UIDamageGroup";
    public override void Init()
    {
        GameObject obj = Resources.Load(sUIDamageGroupPath) as GameObject;
        GameObject Instan = Instantiate(obj, uiCanvasRect);

        uiDamageGroup = Instan.GetComponent<UIDamageGroup>();
        uiDamageGroup.Init();
    }
    public void UIDamageMonster(int nDamage, Monster monster)
    {
        uiDamageGroup.ShowMonster(nDamage, monster);
    }
    public void SetObjPos(RectTransform pos, GameObject obj)
    {
        Vector2 _uiPos;
        Vector2 _screenPos = Camera.main.WorldToScreenPoint(obj.transform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(uiCanvasRect, _screenPos, uiCamera, out _uiPos);

        pos.localPosition = _uiPos;
    }
}
