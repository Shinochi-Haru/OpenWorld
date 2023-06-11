using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField]public float speed = 10.0f;                     // キャラクターの移動速度
    [SerializeField] public float sensitivity = 30.0f;               // マウスの感度
    [SerializeField] float WaterHeight = 15.5f;               // 水面の高さ
    Rigidbody rb;                                   // Rigidbody コンポーネントへの参照
    [SerializeField] public GameObject cam;                          // カメラオブジェクトへの参照
    float moveFB, moveLR;                            // 前後左右の移動量
    float rotX, rotY;                                // マウスの回転量
    [SerializeField] public bool webGLRightClickRotation = true;      // WebGL において右クリックでの回転を有効にするかのフラグ
    [SerializeField]float gravity = -9.8f;                           // 重力の値
    private Animator animator;
    Vector3 movement;
    [SerializeField] float _jumpPower;
    [SerializeField] LayerMask groundLayer; // 地面と判定するレイヤー
    private Vector3 _groundCheckStartOffset;
    bool _isGrounded;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();                 // 自身にアタッチされているRigidbodyコンポーネントを取得
        if (Application.isEditor)
        {
            webGLRightClickRotation = false;                // エディタ上では WebGL での右クリック回転を無効化
            sensitivity = sensitivity * 1.5f;               // エディタ上では感度を1.5倍に増やす
        }
    } 

    private void Update()
    {
        moveFB = Input.GetAxis("Horizontal") * speed;      // 水平方向（左右）の入力に基づいて移動量を計算
        moveLR = Input.GetAxis("Vertical") * speed;        // 垂直方向（前後）の入力に基づいて移動量を計算

        movement = new Vector3(moveFB, gravity, moveLR);   // 移動量をベクトルとして設定
        movement = transform.rotation * movement;            // 移動量をキャラクターのローカル座標系に変換
        rb.velocity = movement;

        rotX = Input.GetAxis("Mouse X") * sensitivity;     // マウスの X 軸移動量に基づいて横回転量を計算
        rotY = Input.GetAxis("Mouse Y") * sensitivity;     // マウスの Y 軸移動量に基づいて縦回転量を計算

        if (webGLRightClickRotation)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                CameraRotation(cam, rotX, rotY);             // WebGL での右クリック回転が有効な場合、マウスの移動量に基づいてカメラを回転
            }
        }
        else if (!webGLRightClickRotation)
        {
            CameraRotation(cam, rotX, rotY);                 // WebGL での右クリック回転が無効な場合、常にマウスの移動量に基づいてカメラを回転
        }
    }

    void CheckForWaterHeight()
    {
        if (transform.position.y < WaterHeight)
        {
            gravity = 0f;                                  // キャラクターが水面より下にいる場合は重力を無効化
        }
        else
        {
            gravity = -5f;                               // キャラクターが水面より上にいる場合は重力を有効化
        }
    }

    private void FixedUpdate()
    {
        CheckForWaterHeight();                             // 水面の高さをチェックし、必要に応じて重力を調整
        //_isGround = IsGrounded();
        if (_isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(transform.up * _jumpPower, ForceMode.Impulse);
        }
    }

    void LateUpdate()
    {
        if (Input.GetKey("w"))
        {
            animator.SetFloat("IsRunning", movement.magnitude);
        }
        else
        {
            animator.SetFloat("IsRunning", 0f);
        }

        if (Input.GetButtonDown("Jump"))
        {

            animator.SetBool("Ground", _isGrounded);
        }
        else if(_isGrounded == false)
        {
            animator.SetBool("Ground", _isGrounded);
        }
        else
        {
            animator.SetBool("Ground", _isGrounded);
        }
    }

    void CameraRotation(GameObject cam, float rotX, float rotY)
    {
        transform.Rotate(0, rotX * Time.deltaTime, 0);       // キャラクターを横回転
        cam.transform.Rotate(-rotY * Time.deltaTime, 0, 0);  // カメラを縦回転
    }

    ////bool IsGrounded()
    ////{
    ////    レイキャストを発射する開始地点と方向を設定
    ////   Vector3 rayStart = transform.position + _groundCheckStartOffset;
    ////    Vector3 rayDirection = Vector3.down;

    ////    レイキャストを発射して地面との当たり判定を取得
    ////    if (Physics.Raycast(rayStart, rayDirection, out RaycastHit hit, Mathf.Infinity, groundLayer))
    ////    {
    ////        地面との距離が一定以下であれば接地していると判定する
    ////        float groundDistanceThreshold = 1f;
    ////        if (hit.distance <= groundDistanceThreshold)
    ////        {
    ////            return true;
    ////        }
    ////    }

    ////    return false;
    ////}
    private void OnTriggerEnter(Collider other)
    {
        _isGrounded = true;
    }

    private void OnTriggerStay(Collider other)
    {
        _isGrounded = true;
    }
    private void OnTriggerExit(Collider other)
    {
        _isGrounded = false;
    }
}
