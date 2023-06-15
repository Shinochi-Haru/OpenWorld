using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField]public float _speed = 10.0f;                     // キャラクターの移動速度
    [SerializeField] public float _slowspeed = 10.0f;
    [SerializeField] public float sensitivity = 30.0f;               // マウスの感度
    [SerializeField] float WaterHeight = 15.5f;               // 水面の高さ
    Rigidbody rb;                                   // Rigidbody コンポーネントへの参照
    [SerializeField] public GameObject cam;                          // カメラオブジェクトへの参照
    float moveFB, moveLR;                            // 前後左右の移動量
    float _slowmoveFB, _slowmoveLR;
    float rotX, rotY;                                // マウスの回転量
    [SerializeField] public bool webGLRightClickRotation = true;      // WebGL において右クリックでの回転を有効にするかのフラグ
    [SerializeField]float gravity = -9.8f;                           // 重力の値
    private Animator animator;
    Vector3 movement;
    Vector3 _slowmovement;
    [SerializeField] float _jumpPower;
    [SerializeField] LayerMask groundLayer; // 地面と判定するレイヤー
    private Vector3 _groundCheckStartOffset;
    bool _isGrounded;
    [SerializeField] private AudioClip[] outdoorFootstepSounds; // 野外の足音の効果音の配列
    [SerializeField] private AudioClip[] indoorFootstepSounds; // 建物の足音の効果音の配列
    [SerializeField] private AudioClip waterFootstepSounds;
    [SerializeField] private AudioClip axeSounds;
    [SerializeField] private AudioClip shildSounds;
    [SerializeField] private AudioSource audioSource; // 効果音を再生するためのAudioSource
    [SerializeField] private AudioSource audioSource2;
    StaminaController _stamina;



    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _stamina = GetComponent<StaminaController>();
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
        moveFB = Input.GetAxis("Horizontal") * _speed;      // 水平方向（左右）の入力に基づいて移動量を計算
        moveLR = Input.GetAxis("Vertical") * _speed;        // 垂直方向（前後）の入力に基づいて移動量を計算
        _slowmoveFB = Input.GetAxis("Horizontal") * _slowspeed;      // 水平方向（左右）の入力に基づいて移動量を計算
        _slowmoveLR = Input.GetAxis("Vertical") * _slowspeed;        // 垂直方向（前後）の入力に基づいて移動量を計算
        if(_stamina.currentSp > 0)
        {
            movement = new Vector3(moveFB, gravity, moveLR);   // 移動量をベクトルとして設定
            movement = transform.rotation * movement;            // 移動量をキャラクターのローカル座標系に変換
            rb.velocity = movement;
            ResetAnimationSpeed();
            ResetAudioSpeed();
        }
        else if(_stamina.currentSp <= 0)
        {
            _slowmovement = new Vector3(_slowmoveFB, gravity, _slowmoveLR);   // 移動量をベクトルとして設定
            _slowmovement = transform.rotation * _slowmovement;            // 移動量をキャラクターのローカル座標系に変換
            rb.velocity = _slowmovement;
            SlowDownAnimationSpeed();
            SlowDownAudioSpeed();
        }

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
    [SerializeField] private float _audioSlowSpeed = 0.5f; // 遅い再生スピードの設定値

    // 再生スピードを遅くする
    public void SlowDownAudioSpeed()
    {
        audioSource.pitch = _audioSlowSpeed;
    }

    // 再生スピードを元の速度に戻す
    public void ResetAudioSpeed()
    {
        audioSource.pitch = 1f;
    }
    [SerializeField] private float slowSpeed = 0.5f; // 遅い再生スピードの設定値

    // 再生スピードを遅くする
    public void SlowDownAnimationSpeed()
    {
        animator.speed = slowSpeed;
    }

    // 再生スピードを元の速度に戻す
    public void ResetAnimationSpeed()
    {
        animator.speed = 1f;
    }
    void CheckForWaterHeight()
    {
        if (transform.position.y < WaterHeight)
        {
            gravity = 0f;                                  // キャラクターが水面より下にいる場合は重力を無効化
            audioSource2.clip = waterFootstepSounds;
            audioSource2.Play();
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
            if (IsIndoor())
            {
                PlayRandomFootstepSound(indoorFootstepSounds);
            }
            else
            {
                PlayRandomFootstepSound(outdoorFootstepSounds);
            }
        }
        else
        {
            animator.SetFloat("IsRunning", 0f);
        }

        if (Input.GetKey("d"))
        {
            animator.SetFloat("IsRunningD", movement.magnitude);
        }
        else
        {
            animator.SetFloat("IsRunningD", 0f);
        }
        if (Input.GetKey("a"))
        {
            animator.SetFloat("IsRunningA", movement.magnitude);
        }
        else
        {
            animator.SetFloat("IsRunningA", 0f);
        }
        if (Input.GetKey("s"))
        {
            animator.SetFloat("IsRunningS", movement.magnitude);
        }
        else
        {
            animator.SetFloat("IsRunningS", 0f);
        }

        if (Input.GetButtonDown("Jump"))
        {

            animator.SetBool("Ground", _isGrounded);
        }
        else if(_isGrounded == false)
        {
            animator.SetBool("Ground", _isGrounded);
            audioSource.Stop();
        }
        else
        {
            animator.SetBool("Ground", _isGrounded);
        }
        if (!Input.GetKey("w") && !Input.GetKey("d") && !Input.GetKey("a") && !Input.GetKey("s"))
        {
            if (audioSource.clip != outdoorFootstepSounds[0] && audioSource.clip != indoorFootstepSounds[0])
            {
                audioSource.Stop();
            }
        }
    }
    void Audio1()
    {
        audioSource2.PlayOneShot(axeSounds);
    }
    void Audio2()
    {
        audioSource2.PlayOneShot(shildSounds);
    }
    private bool IsIndoor()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Building"))
            {
                return true;
            }
        }
        return false;
    }
    private void PlayRandomFootstepSound(AudioClip[] footstepSounds)
    {
        // 効果音を再生
        if (!audioSource.isPlaying)
        {
            // footstepSounds配列からランダムな効果音を選択
            AudioClip footstepSound = footstepSounds[Random.Range(0, footstepSounds.Length)];
            audioSource.PlayOneShot(footstepSound);
        }
    }

    void CameraRotation(GameObject cam, float rotX, float rotY)
    {
        transform.Rotate(0, rotX * Time.deltaTime, 0);       // キャラクターを横回転
        //cam.transform.Rotate(-rotY * Time.deltaTime, 0, 0);  // カメラを縦回転
    }
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
