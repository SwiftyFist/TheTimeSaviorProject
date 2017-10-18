using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackArm : MonoBehaviour
{
    private Transform _myTransform;
    private Transform _backPointTransform;
    private Transform _initialPointTransform;

    public float BackVelocity = 10;
    public float KnockBackVelocity = 4;    

    public void Awake()
    {
        _myTransform = GetComponent<Transform>();
        _backPointTransform = GameObject.Find("BackPoint").GetComponent<Transform>();
        _initialPointTransform = GameObject.Find("InitialPoint").GetComponent<Transform>();
    }

    void Update()
    {
        BackInPosition();
    }

    public void KnockBack()
    {
        _myTransform.position = Vector3.MoveTowards(
            _myTransform.position,
            _backPointTransform.position,
            KnockBackVelocity * Time.deltaTime
        );                   
    }

    void BackInPosition()
    {
        if (_myTransform.position != _initialPointTransform.position)
            _myTransform.position = Vector3.MoveTowards(
                _myTransform.position,
                _initialPointTransform.position,
                BackVelocity * Time.deltaTime
            );
    }

    bool IsInPosition()
    {
        if (_myTransform.position == _initialPointTransform.position)
            return true;
        else
            return false;
    }
}
