using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action_Type
{
    Idle,
    Attack,
    Die,
}
public class Monster : MonoBehaviour
{
    [HideInInspector]
    public int nMobIndex;
    private Action_Type action_Type;

    private Animator aniMonster;

    [SerializeField]
    private CapsuleCollider2D bodyCollider;
    [SerializeField]
    private BoxCollider2D leftCollider;
    public BoxCollider2D headCollider;

    [SerializeField]
    private SpriteRenderer[] arrSprite;
    private int[] arrRendererLayer;

    private Monster upMonster;
    private Monster leftMonster;
    private Monster rightMonster;
    private Monster downMonster;

    private string sAniName;

    private bool bJump = false;
    private float fJumpTime = 0;
    private bool bBack = false;

    private Coroutine KnockBackCoro;

    [HideInInspector]
    public int nAttack = 10;
    [HideInInspector]
    public int nHp = 100;
    [HideInInspector]
    public int nMaxHp = 100;

    private float fAttackTime = 0;

    private const float fJumpTimeLimit = 1.0f;
    private const string sAttackAniName = "IsAttacking";
    private const string sIdleAniName = "IsIdle";
    private const string sDieAniName = "IsDead";
    public void Init(int _nIndex)
    {
        nMobIndex = _nIndex;

        if (aniMonster == null)
            aniMonster = GetComponent<Animator>();

        sAniName = sIdleAniName;
        arrRendererLayer = new int[arrSprite.Length];
        for (int i = 0; i < arrSprite.Length; ++i)
        {
            arrRendererLayer[i] = arrSprite[i].sortingOrder;
        }

        fJumpTime = 1;
        nHp = nMaxHp;
        SetAction(Action_Type.Idle);
    }

    public void Update_Logic()
    {
        UDLR_Monster();

        if (action_Type == Action_Type.Idle)
        {
            if (KnockBackCoro != null)
                return;

            fAttackTime = 0;
            Idle();
        }
        else if (action_Type == Action_Type.Attack)
        {
            fAttackTime += Time.deltaTime;
            if (downMonster != null)
            {
                float _fLimit = 1f;
                if (fAttackTime >= _fLimit)
                {
                    if (KnockBackCoro == null)
                    {
                        float _fGravity = 2.5f;
                        transform.Translate(Vector3.down * (Time.deltaTime * _fGravity));
                    }
                    downMonster.SetKnockBack();
                }
            }
        }

    }

    // ���� ��,�Ʒ�,����,�����ʿ� �ٸ� ���� üũ
    //  => ������ ���� ������ ���� �ڷ� �и��� ������ �ؾ� �ϱ� ������ üũ
    public void UDLR_Monster()
    {
        MonsterManager _monsterManager = MonsterManager.Instance;

        upMonster = null;
        downMonster = null;
        leftMonster = null;
        rightMonster = null;

        for (int i = 0; i < _monsterManager.lisActive_Monster.Count; ++i)
        {
            if (_monsterManager.lisActive_Monster[i].gameObject == gameObject)
                continue;

            if (headCollider.bounds.Intersects(_monsterManager.lisActive_Monster[i].bodyCollider.bounds))
            {
                upMonster = _monsterManager.lisActive_Monster[i];
            }
            else if (bodyCollider.bounds.Intersects(_monsterManager.lisActive_Monster[i].headCollider.bounds))
            {
                downMonster = _monsterManager.lisActive_Monster[i];
            }
            else if (leftCollider.bounds.Intersects(_monsterManager.lisActive_Monster[i].bodyCollider.bounds))
            {
                leftMonster = _monsterManager.lisActive_Monster[i];
            }
            else if (bodyCollider.bounds.Intersects(_monsterManager.lisActive_Monster[i].leftCollider.bounds))
            {
                rightMonster = _monsterManager.lisActive_Monster[i];
            }
        }
    }

    //leftMonster�� ������ ���� ������ ������ ����
    public void Idle()
    {
        if (leftMonster)
        {
            bBack = leftMonster.bBack;

            //���� ���Ͱ� �ڷ� �и��� ������ ���� �и��� �ϴ� ���ǹ�
            if (bBack && !bJump)
            {
                float _fKnockBackValue = 2f;
                transform.position = new Vector3(transform.position.x + Time.deltaTime * _fKnockBackValue, transform.position.y, 0);
                return;
            }
            //���� ������ Y���� ���� ���� ��� �ݶ��̴��� ���� ��찡 �־ �ڷ� �̴� ���ǹ�
            if (leftMonster.transform.position.y > transform.position.y)
                transform.position = new Vector3(transform.position.x + Time.deltaTime, transform.position.y, 0);

            fJumpTime += Time.deltaTime;

            //���� ���� ���� ���Ͱ� ���� ��� ������ ���ϰ� ����� ���ǹ�
            if (upMonster != null || leftMonster.upMonster != null)
                return;

            //�� ���� ���Ͱ� ������ �� ��� ������ ��ġ�� �ʰ� �ٽ� ���� ��Ÿ���� 0���� ����� ���ǹ�
            if (leftMonster.bJump || (rightMonster != null && rightMonster.bJump))
            {
                fJumpTime = 0;
                return;
            }

            if (fJumpTime < fJumpTimeLimit)
                return;

            float _fSpeedX = 4f;
            float _fSpeedY = 7f;
            transform.position = new Vector3(transform.position.x - Time.deltaTime * _fSpeedX, transform.position.y + Time.deltaTime * _fSpeedY, 0);
            bJump = true;
        }
        else
        {
            bBack = false;
            bJump = false;

            float _fSpeedX = 2f;
            transform.position = new Vector3(transform.position.x - Time.deltaTime * _fSpeedX, transform.position.y, 0);
        }
    }

    public void SetKnockBack()
    {
        if (KnockBackCoro != null)
            return;
        KnockBackCoro = StartCoroutine(KnockBack());
    }
    //���͸� �ڷ� �̴� �ڷ�ƾ
    public IEnumerator KnockBack()
    {
        bBack = true;

        float _fTimeLimit = 0.3f;
        float _fPosX = transform.position.x + 0.6f;
        float _fKnockBackTime = 0;
        float _fSpeed = 2;

        while (true)
        {
            yield return null;

            _fKnockBackTime += Time.deltaTime;
            if (rightMonster != null)
                _fSpeed = 5;
            else
                _fSpeed = 2;

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(_fPosX, transform.position.y), Time.deltaTime * _fSpeed);
            if (_fKnockBackTime > _fTimeLimit)
            {
                bBack = false;
                KnockBackCoro = null;
                break;
            }
        }
    }
    public void SetAction(Action_Type _action_Type)
    {
        switch (_action_Type)
        {
            case Action_Type.Idle:
                Play_Ani(sIdleAniName);
                break;
            case Action_Type.Attack:
                Play_Ani(sAttackAniName);
                break;
            case Action_Type.Die:
                Play_Ani(sDieAniName);
                break;

        }
        action_Type = _action_Type;
    }

    public void OnDie()
    {
        MonsterManager.Instance.Die(this);
    }
    private void Play_Ani(string _playName)
    {
        aniMonster.SetBool(sAniName, false);
        aniMonster.SetBool(_playName, true);

        sAniName = _playName;
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Box")
        {
            SetAction(Action_Type.Attack);
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Box")
        {
            SetAction(Action_Type.Idle);
        }
    }
    public void OnAttack() { }
}

