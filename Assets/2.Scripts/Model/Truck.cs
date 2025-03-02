using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    [SerializeField]
    private GameObject moveStartObj;
    [SerializeField]
    private GameObject moveEndObj;
    [SerializeField]
    private GameObject[] arrWheel;

    private float fWheelValue;

    private const float fMoveSpeed = 1.5f;
    private const float fWheelSpeed = 320f;

    public void Start()
    {
        transform.position = new Vector2(moveStartObj.transform.position.x, transform.position.y);
        fWheelValue = 0;
    }
    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector2(moveEndObj.transform.position.x, transform.position.y), Time.deltaTime * fMoveSpeed);

        if (transform.position.x < moveEndObj.transform.position.x)
        {
            fWheelValue -= Time.deltaTime * fWheelSpeed;
            for (int i = 0; i < arrWheel.Length; ++i)
            {
                arrWheel[i].transform.rotation = Quaternion.Euler(0, 0, fWheelValue);
            }
        }
    }
}
