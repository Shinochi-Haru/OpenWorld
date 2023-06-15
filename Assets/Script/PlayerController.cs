using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField]public float _speed = 10.0f;                     // �L�����N�^�[�̈ړ����x
    [SerializeField] public float _slowspeed = 10.0f;
    [SerializeField] public float sensitivity = 30.0f;               // �}�E�X�̊��x
    [SerializeField] float WaterHeight = 15.5f;               // ���ʂ̍���
    Rigidbody rb;                                   // Rigidbody �R���|�[�l���g�ւ̎Q��
    [SerializeField] public GameObject cam;                          // �J�����I�u�W�F�N�g�ւ̎Q��
    float moveFB, moveLR;                            // �O�㍶�E�̈ړ���
    float _slowmoveFB, _slowmoveLR;
    float rotX, rotY;                                // �}�E�X�̉�]��
    [SerializeField] public bool webGLRightClickRotation = true;      // WebGL �ɂ����ĉE�N���b�N�ł̉�]��L���ɂ��邩�̃t���O
    [SerializeField]float gravity = -9.8f;                           // �d�͂̒l
    private Animator animator;
    Vector3 movement;
    Vector3 _slowmovement;
    [SerializeField] float _jumpPower;
    [SerializeField] LayerMask groundLayer; // �n�ʂƔ��肷�郌�C���[
    private Vector3 _groundCheckStartOffset;
    bool _isGrounded;
    [SerializeField] private AudioClip[] outdoorFootstepSounds; // ��O�̑����̌��ʉ��̔z��
    [SerializeField] private AudioClip[] indoorFootstepSounds; // �����̑����̌��ʉ��̔z��
    [SerializeField] private AudioClip[] waterFootstepSounds;
    [SerializeField] private AudioSource audioSource; // ���ʉ����Đ����邽�߂�AudioSource
    StaminaController _stamina;



    void Start()
    {
        _stamina = GetComponent<StaminaController>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();                 // ���g�ɃA�^�b�`����Ă���Rigidbody�R���|�[�l���g���擾
        if (Application.isEditor)
        {
            webGLRightClickRotation = false;                // �G�f�B�^��ł� WebGL �ł̉E�N���b�N��]�𖳌���
            sensitivity = sensitivity * 1.5f;               // �G�f�B�^��ł͊��x��1.5�{�ɑ��₷
        }
    } 

    private void Update()
    {
        moveFB = Input.GetAxis("Horizontal") * _speed;      // ���������i���E�j�̓��͂Ɋ�Â��Ĉړ��ʂ��v�Z
        moveLR = Input.GetAxis("Vertical") * _speed;        // ���������i�O��j�̓��͂Ɋ�Â��Ĉړ��ʂ��v�Z
        _slowmoveFB = Input.GetAxis("Horizontal") * _slowspeed;      // ���������i���E�j�̓��͂Ɋ�Â��Ĉړ��ʂ��v�Z
        _slowmoveLR = Input.GetAxis("Vertical") * _slowspeed;        // ���������i�O��j�̓��͂Ɋ�Â��Ĉړ��ʂ��v�Z
        if(_stamina.currentSp > 0)
        {
            movement = new Vector3(moveFB, gravity, moveLR);   // �ړ��ʂ��x�N�g���Ƃ��Đݒ�
            movement = transform.rotation * movement;            // �ړ��ʂ��L�����N�^�[�̃��[�J�����W�n�ɕϊ�
            rb.velocity = movement;
        }
        else if(_stamina.currentSp <= 0)
        {
            _slowmovement = new Vector3(_slowmoveFB, gravity, _slowmoveLR);   // �ړ��ʂ��x�N�g���Ƃ��Đݒ�
            _slowmovement = transform.rotation * _slowmovement;            // �ړ��ʂ��L�����N�^�[�̃��[�J�����W�n�ɕϊ�
            rb.velocity = _slowmovement;
        }

        rotX = Input.GetAxis("Mouse X") * sensitivity;     // �}�E�X�� X ���ړ��ʂɊ�Â��ĉ���]�ʂ��v�Z
        rotY = Input.GetAxis("Mouse Y") * sensitivity;     // �}�E�X�� Y ���ړ��ʂɊ�Â��ďc��]�ʂ��v�Z

        if (webGLRightClickRotation)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                CameraRotation(cam, rotX, rotY);             // WebGL �ł̉E�N���b�N��]���L���ȏꍇ�A�}�E�X�̈ړ��ʂɊ�Â��ăJ��������]
            }
        }
        else if (!webGLRightClickRotation)
        {
            CameraRotation(cam, rotX, rotY);                 // WebGL �ł̉E�N���b�N��]�������ȏꍇ�A��Ƀ}�E�X�̈ړ��ʂɊ�Â��ăJ��������]
        }
    }

    void CheckForWaterHeight()
    {
        if (transform.position.y < WaterHeight)
        {
            gravity = 0f;                                  // �L�����N�^�[�����ʂ�艺�ɂ���ꍇ�͏d�͂𖳌���
            PlayRandomFootstepSound(waterFootstepSounds);
        }
        else
        {
            gravity = -5f;                               // �L�����N�^�[�����ʂ���ɂ���ꍇ�͏d�͂�L����
        }
    }

    private void FixedUpdate()
    {
        CheckForWaterHeight();                             // ���ʂ̍������`�F�b�N���A�K�v�ɉ����ďd�͂𒲐�
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
            audioSource.Stop();
        }

    }
    private bool IsIndoor()
    {
        // ���������ǂ����̔��胍�W�b�N����������
        // ��: ��������Collider�ɐڐG���Ă��邩�𔻒肵�ĕԂ�
        // ���̗�ł�"Building"�Ƃ����^�O������Collider��������\���Ƃ��Ă��܂�
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
        // ���ʉ����Đ�
        if (!audioSource.isPlaying)
        {
            // footstepSounds�z�񂩂烉���_���Ȍ��ʉ���I��
            AudioClip footstepSound = footstepSounds[Random.Range(0, footstepSounds.Length)];
            audioSource.PlayOneShot(footstepSound);
        }
    }

    void CameraRotation(GameObject cam, float rotX, float rotY)
    {
        transform.Rotate(0, rotX * Time.deltaTime, 0);       // �L�����N�^�[������]
        //cam.transform.Rotate(-rotY * Time.deltaTime, 0, 0);  // �J�������c��]
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
