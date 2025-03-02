using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFx : MonoBehaviour
{
    private GameObject targetObj;
    private float fLiveTime = 0;

    private const float fTimeLimit = 0.1f;
  
    public void Init(GameObject _targetObj)
    {
        targetObj = _targetObj;
    }
    public void Die()
    {
        gameObject.SetActive(false);
    }
    public void Update_Logic()
    {
        fLiveTime += Time.deltaTime;

        if (fLiveTime >= fTimeLimit)
        {
            FxManager.Instance.Die(this);
            fLiveTime = 0;
        }

        transform.position = targetObj.transform.position;
    }
}
