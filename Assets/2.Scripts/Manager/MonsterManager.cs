using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    private GameObject monsterSpawnObj;

    private Dictionary<int, ObjcetPool<Monster>> dicMonsterPool = new Dictionary<int, ObjcetPool<Monster>>();
    private List<int> lisMonsterKeyIndex = new List<int>();
    [HideInInspector]
    public List<Monster> lisActive_Monster = new List<Monster>();
    [HideInInspector]
    public List<Monster> lisDie_Monster = new List<Monster>();

    private float fRespawnTime = 0;
    private const float fRespawn = 1.5f;
    private const string sMonsterPath = "Monster/Zombie_";
    public override void Init()
    {
        monsterSpawnObj = GameObject.FindGameObjectWithTag("Respawn");

        int _nCount = 30;
        int _nMonsterIndexCount = 5;
        for (int i = 1; i < _nMonsterIndexCount; ++i)
        {
            ObjcetPool<Monster> _pool = new ObjcetPool<Monster>();
            _pool.Init(sMonsterPath + i, _nCount, transform);
            dicMonsterPool.Add(i, _pool);
            lisMonsterKeyIndex.Add(i);
        }
    }

    public void CreateMonster()
    {
        int _nIndex = lisMonsterKeyIndex[Random.Range(0, lisMonsterKeyIndex.Count)];
        Monster _mob = dicMonsterPool[_nIndex].Get();
        _mob.Init(_nIndex);
        _mob.transform.position = new Vector3(monsterSpawnObj.transform.position.x, monsterSpawnObj.transform.position.y, 0);

        lisActive_Monster.Add(_mob);
    }

    public void Update()
    {
        fRespawnTime += Time.deltaTime;

        if (fRespawnTime >= fRespawn)
        {
            CreateMonster();
            fRespawnTime = 0;
        }

        for (int i = 0; i < lisActive_Monster.Count; ++i)
        {
            if (lisActive_Monster[i].gameObject.activeSelf)
                lisActive_Monster[i].Update_Logic();
        }

        if (lisDie_Monster.Count > 0)
        {
            for (int i = 0; i < lisDie_Monster.Count; ++i)
            {
                lisActive_Monster.Remove(lisDie_Monster[i]);
                dicMonsterPool[lisDie_Monster[i].nMobIndex].Return(lisDie_Monster[i]);
            }
            lisDie_Monster.Clear();
        }
    }

    //lisActive_Monster 에서 가까운 몬스터를 찾는 함수
    public Monster GetAttackTarget(GameObject _player)
    {
        float _fDist = 999;
        Monster _mob = lisActive_Monster[0];

        for (int i = 0; i < lisActive_Monster.Count; ++i)
        {
            float _fMobDist = Vector2.Distance(lisActive_Monster[i].transform.position, _player.transform.position);
            if (_fDist > _fMobDist)
            {
                _mob = lisActive_Monster[i];
                _fDist = _fMobDist;
            }
        }
        return _mob;
    }
    public void DamageMonster(int _nAttack, GameObject _obj)
    {
        Monster _mob = _obj.GetComponent<Monster>();
        _mob.nHp -= _nAttack;

        if (_mob.nHp <= 0)
        {
            _mob.SetAction(Action_Type.Die);
        }

        UIManager.Instance.UIDamageMonster(_nAttack, _mob);
    }

    public void Die(Monster _mob)
    {
        if (!lisDie_Monster.Contains(_mob))
            lisDie_Monster.Add(_mob);
    }

}
