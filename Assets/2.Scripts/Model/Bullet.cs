using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int nAttack;
    private float fLiveTime = 0;

    private const float fLiveMoveTime = 2.0f;
    private const float fMoveSpeed = 30f;
    public void Init(int _nAttack)
    {
        nAttack = _nAttack;
    }

    public void Update_Logic()
    {
        transform.Translate(Vector2.down * Time.deltaTime * fMoveSpeed, Space.Self);

        fLiveTime += Time.deltaTime;
        if (fLiveTime >= fLiveMoveTime)
        {
            BulletManager.Instance.Die(this);
            fLiveTime = 0;
        }
    }
    public void Die()
    {
        gameObject.SetActive(false);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            BulletManager.Instance.Die(this);
            MonsterManager.Instance.DamageMonster(nAttack, collision.gameObject);
        }
        else if (collision.tag == "Background") 
        {
            BulletManager.Instance.Die(this);
        }
    }
}
