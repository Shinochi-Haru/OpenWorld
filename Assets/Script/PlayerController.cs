using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField]public float speed = 10.0f;                     // �L�����N�^�[�̈ړ����x
    [SerializeField] public float sensitivity = 30.0f;               // �}�E�X�̊��x
    [SerializeField] float WaterHeight = 15.5f;               // ���ʂ̍���
    Rigidbody rb;                                   // Rigidbody �R���|�[�l���g�ւ̎Q��
    [SerializeField] public GameObject cam;                          // �J�����I�u�W�F�N�g�ւ̎Q��
    float moveFB, moveLR;                            // �O�㍶�E�̈ړ���
    float rotX, rotY;                                // �}�E�X�̉�]��
    [SerializeField] public bool webGLRightClickRotation = true;      // WebGL �ɂ����ĉE�N���b�N�ł̉�]��L���ɂ��邩�̃t���O
    [SerializeField]float gravity = -9.8f;                           // �d�͂̒l
    private Animator animator;
    Vector3 movement;
    [SerializeField] float _jumpPower;
    [SerializeField] LayerMask groundLayer; // �n�ʂƔ��肷�郌�C���[
    private Vector3 _groundCheckStartOffset;
    bool _isGrounded;

    void Start()
    {
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
        moveFB = Input.GetAxis("Horizontal") * speed;      // ���������i���E�j�̓��͂Ɋ�Â��Ĉړ��ʂ��v�Z
        moveLR = Input.GetAxis("Vertical") * speed;        // ���������i�O��j�̓��͂Ɋ�Â��Ĉړ��ʂ��v�Z

        movement = new Vector3(moveFB, gravity, moveLR);   // �ړ��ʂ��x�N�g���Ƃ��Đݒ�
        movement = transform.rotation * movement;            // �ړ��ʂ��L�����N�^�[�̃��[�J�����W�n�ɕϊ�
        rb.velocity = movement;

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
        transform.Rotate(0, rotX * Time.deltaTime, 0);       // �L�����N�^�[������]
        cam.transform.Rotate(-rotY * Time.deltaTime, 0, 0);  // �J�������c��]
    }

    ////bool IsGrounded()
    ////{
    ////    ���C�L���X�g�𔭎˂���J�n�n�_�ƕ�����ݒ�
    ////   Vector3 rayStart = transform.position + _groundCheckStartOffset;
    ////    Vector3 rayDirection = Vector3.down;

    ////    ���C�L���X�g�𔭎˂��Ēn�ʂƂ̓����蔻����擾
    ////    if (Physics.Raycast(rayStart, rayDirection, out RaycastHit hit, Mathf.Infinity, groundLayer))
    ////    {
    ////        �n�ʂƂ̋��������ȉ��ł���ΐڒn���Ă���Ɣ��肷��
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
