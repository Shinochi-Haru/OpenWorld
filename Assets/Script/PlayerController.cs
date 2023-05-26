using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _upPower = 10f;
    [SerializeField] float _horizonPower = 10f;
    private Vector3 moveDirection; // キャラクターの移動方向

    Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        // 上下左右のキーの入力を取得
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // 移動する方向を決める
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // 移動する向きにキャラクターの向きを変える
        if (direction.magnitude >= 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        // Rigidbodyに力を加える
        Vector3 movement = direction * _horizonPower * Time.deltaTime;
        _rb.AddForce(movement, ForceMode.VelocityChange);
        //上昇する
        if (Input.GetKey(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * _upPower);
        }
        else
        {
            _rb.velocity = Vector3.zero;
        }
    }
}