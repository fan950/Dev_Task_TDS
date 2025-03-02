using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int nAttack = 20;

    public GameObject truckObj;
    public GameObject weaponObj;

    private float fAttackSpeed = 0;

    private const float fWeaponEuler = -20.0f;
    private const float fAttackRange = 8.0f;
    private const float fAttackLimit = 3.0f;

    private void Start()
    {
        fAttackSpeed = 3.0f;
    }
    public void Update()
    {
        if (MonsterManager.Instance.lisActive_Monster.Count > 0)
        {
            Monster _mob = MonsterManager.Instance.GetAttackTarget(gameObject);
            //fAttackRange 보다 거리가 작아지면  Bullet을 발사
            float _fDist = Vector2.Distance(_mob.transform.position,transform.position);

            if (fAttackRange >= _fDist)
            {
                fAttackSpeed += Time.deltaTime;
                if (fAttackSpeed >= fAttackLimit)
                {
                    BulletManager.Instance.Attack(nAttack);
                    fAttackSpeed = 0;
                }

            }
            Vector3 dir = _mob.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            weaponObj.transform.rotation = Quaternion.AngleAxis(angle + fWeaponEuler, Vector3.forward);
        }
    }
}
