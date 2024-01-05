using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Basketball : MonoBehaviour
{
    [SerializeField] float _speed = 10f;
    [SerializeField] Transform _ball;
    [SerializeField] Transform _arms;
    [SerializeField] Transform _posOverhead;
    [SerializeField] Transform _posDribble;
    [SerializeField] Transform _target; // ���������� "����" ��� ������ ����

    private bool _isBallInHands = true;
    private bool _isBallFlying = false; // ���� ��� � ������
    private float _t = 0f; // ������ ������� ������

    void Start()
    {
            
    }


    void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += direction * _speed * Time.deltaTime; // �������� ������� ������ ���������
        transform.LookAt(transform.position + direction); // ������ ����������� ������ ���������

        if (_isBallInHands) // ���� ��� � ����� � ����� ������
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _ball.position = _posOverhead.position; // �� �������� ������� ���� ��� �������
                _arms.localEulerAngles = Vector3.right * 180; // �������� ���� ��� ��� ������� �� ������ 

                transform.LookAt(_target.position); // ��� ������� ������� ����� �������� �� ����(_target)
            }

            else
            {                                         // ��� �� �������� ���������� ��� ���� ����� ��� ��� ������ �� ����
                _ball.position = _posDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5)); // ������ ������� ����� ���������
                _arms.localEulerAngles = Vector3.right * 0; // ��� ��������� ������� �� ����� ��������� ����

            }

            if (Input.GetKey(KeyCode.Space)) // ���� �� ��������� ������� ������, �� �� ��������� ���
            {
                _isBallInHands = false;
                _isBallFlying = true;
                _t = 0f;
            }
        }

        

        if (_isBallFlying)  // ���� ��� �����, �� �������� ������ �������
        {
            _t += Time.deltaTime;
            float duration = 0.5f; // �������� ���������� "�����������"
            float t01 = _t / duration; // ����� ������ / ������������

            Vector3 a = _posOverhead.position; // ������� ��� �������
            Vector3 b = _target.position; // ������� ��� �������
            Vector3 pos = Vector3.Lerp(a, b, t01); // ������ ������� �� � �� b �� t01 - ������� - ��� � ����� ������� ����

            Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f); // �� �� ������� ����, ���� �� �������� �� ����

            _ball.position = pos + arc;

            if (t01 >= 1) 
            {
                _isBallFlying = false; // ���� t01 >= 1, �� ��� ��������� ������
                _ball.GetComponent<Rigidbody>().isKinematic = false; // � ��� ����� ������
            }
        }
    }
    private void OnTriggerEnter(Collider other) // ������ ������� ��� ������ ���� � ����
    {
        if (!_isBallInHands && !_isBallFlying) 
        {
            _isBallInHands = true;
            _ball.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
