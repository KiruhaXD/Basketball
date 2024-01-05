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
    [SerializeField] Transform _target; // переменная "цель" для броска мяча

    private bool _isBallInHands = true;
    private bool _isBallFlying = false; // если мяч в полете
    private float _t = 0f; // отсчет времени полета

    void Start()
    {
            
    }


    void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += direction * _speed * Time.deltaTime; // изменяем позицию нашего персонажа
        transform.LookAt(transform.position + direction); // меняем направление нашего персонажа

        if (_isBallInHands) // если мяч в руках и нажат пробел
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _ball.position = _posOverhead.position; // то изменяем позицию мяча над головой
                _arms.localEulerAngles = Vector3.right * 180; // изменяем угол рук при нажатии на пробел 

                transform.LookAt(_target.position); // при зажатии пробела будем смотреть на цель(_target)
            }

            else
            {                                         // тут мы добавили математики для того чтобы наш мяч прыгал по полу
                _ball.position = _posDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5)); // меняем позицию возле персонажа
                _arms.localEulerAngles = Vector3.right * 0; // при отускании пробела мы будем отпускать руки

            }

            if (Input.GetKey(KeyCode.Space)) // если мы отпускаем клавишу пробел, то мы запускаем мяч
            {
                _isBallInHands = false;
                _isBallFlying = true;
                _t = 0f;
            }
        }

        

        if (_isBallFlying)  // если мяч летит, то начинаем отсчет времени
        {
            _t += Time.deltaTime;
            float duration = 0.5f; // добавили переменную "длительнсть"
            float t01 = _t / duration; // время полета / длительность

            Vector3 a = _posOverhead.position; // позиция над головой
            Vector3 b = _target.position; // позиция над кольцом
            Vector3 pos = Vector3.Lerp(a, b, t01); // меняем позицию от а до b за t01 - времени - это и будет позиция мяча

            Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f); // ту мы придаем мячу, чтоб он двигался по дуге

            _ball.position = pos + arc;

            if (t01 >= 1) 
            {
                _isBallFlying = false; // если t01 >= 1, то мяч перестает лететь
                _ball.GetComponent<Rigidbody>().isKinematic = false; // и мяч будет падать
            }
        }
    }
    private void OnTriggerEnter(Collider other) // делаем функцию для взятия мяча в руки
    {
        if (!_isBallInHands && !_isBallFlying) 
        {
            _isBallInHands = true;
            _ball.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
