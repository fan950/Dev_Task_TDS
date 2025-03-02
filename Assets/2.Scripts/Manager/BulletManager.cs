using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : Singleton<BulletManager>
{
    private Player player;

    private ObjcetPool<Bullet> bulletPool = new ObjcetPool<Bullet>();

    [HideInInspector]
    public List<Bullet> lisActiveBullet = new List<Bullet>();
    [HideInInspector]
    public List<Bullet> lisDieBullet = new List<Bullet>();

    [SerializeField]
    private GameObject bulletSpawn;
    private GameObject bulletPoint;

    private Vector3 vecBulletSize = new Vector3(0.5f, 0.5f, 0.5f);

    private const float fFollowSpeed = 100f;
    private const float fTopEuler = 127;
    private const float fBottomEuler = 107;
    private const string sBulletPath = "Bullet/Bullet";
    public override void Init()
    {
        bulletPoint = GameObject.FindGameObjectWithTag("BulletPoint");
        player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player>();

        int _nCount = 50;
        bulletPool.Init(sBulletPath, _nCount, transform);
    }
    public void CreateBullet(int _nAttack)
    {
        Bullet _bullet = bulletPool.Get();

        _bullet.Init(_nAttack);
        _bullet.transform.position = bulletSpawn.transform.position;

        _bullet.transform.localScale = vecBulletSize;
        float _fRamdom = Random.Range(fBottomEuler, fTopEuler);
        _bullet.transform.rotation = Quaternion.Euler(0, 0, player.weaponObj.transform.eulerAngles.z + _fRamdom);

        lisActiveBullet.Add(_bullet);
    }
    public void Die(Bullet _bullet)
    {
        if (!lisDieBullet.Contains(_bullet))
            lisDieBullet.Add(_bullet);
    }
    public void Update()
    {
        bulletSpawn.transform.position = Vector3.MoveTowards(bulletSpawn.transform.position, bulletPoint.transform.position, Time.deltaTime * fFollowSpeed);

        for (int i = 0; i < lisActiveBullet.Count; ++i)
        {
            if (lisActiveBullet[i].gameObject.activeSelf)
                lisActiveBullet[i].Update_Logic();
        }

        if (lisDieBullet.Count > 0)
        {
            for (int i = 0; i < lisDieBullet.Count; ++i)
            {
                lisDieBullet[i].Die();

                lisActiveBullet.Remove(lisDieBullet[i]);
                bulletPool.Return(lisDieBullet[i]);
            }
            lisDieBullet.Clear();
        }
    }
    public void Attack(int _nAttack) 
    {
        float _fRotat = 0.5f;
        int _nRandomCount = Random.Range(4, 7);
        for (int i = 0; i < _nRandomCount; ++i)
        {
            CreateBullet(_nAttack);
        }
        GameObject _fxObj = FxManager.Instance.CreateBulletFx(bulletSpawn);
        _fxObj.transform.rotation = Quaternion.Euler(0, 0, player.weaponObj.transform.eulerAngles.z + ((fTopEuler + fBottomEuler) * _fRotat));
    }
}
