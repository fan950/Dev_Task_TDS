using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxManager : Singleton<FxManager>
{
    private ObjcetPool<BulletFx> bulletFxPool = new ObjcetPool<BulletFx>();

    [HideInInspector]
    public List<BulletFx> lisActiveBulletFx = new List<BulletFx>();
    [HideInInspector]
    public List<BulletFx> lisDieBulletFx = new List<BulletFx>();

    private const string sBulletFxPath = "Fx/BulletFx";
    public override void Init()
    {
        int _nCount = 3;
        bulletFxPool.Init(sBulletFxPath, _nCount, transform);
    }
    public GameObject CreateBulletFx(GameObject _obj)
    {
        BulletFx _fx = bulletFxPool.Get();

        _fx.Init(_obj);
        lisActiveBulletFx.Add(_fx);

        return _fx.gameObject;
    }
    public void Die(BulletFx _fx)
    {
        if (!lisDieBulletFx.Contains(_fx))
            lisDieBulletFx.Add(_fx);
    }
    public void Update()
    {
        for (int i = 0; i < lisActiveBulletFx.Count; ++i)
        {
            if (lisActiveBulletFx[i].gameObject.activeSelf)
                lisActiveBulletFx[i].Update_Logic();
        }

        if (lisDieBulletFx.Count > 0)
        {
            for (int i = 0; i < lisDieBulletFx.Count; ++i)
            {
                lisDieBulletFx[i].Die();

                lisActiveBulletFx.Remove(lisDieBulletFx[i]);
                bulletFxPool.Return(lisDieBulletFx[i]);
            }
            lisDieBulletFx.Clear();
        }
    }
}
