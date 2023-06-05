using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;                     // �L�����N�^�[�̈ړ����x
    public float sensitivity = 30.0f;               // �}�E�X�̊��x
    public float WaterHeight = 15.5f;               // ���ʂ̍���
    Rigidbody rb;                                   // Rigidbody �R���|�[�l���g�ւ̎Q��
    public GameObject cam;                          // �J�����I�u�W�F�N�g�ւ̎Q��
    float moveFB, moveLR;                            // �O�㍶�E�̈ړ���
    float rotX, rotY;                                // �}�E�X�̉�]��
    public bool webGLRightClickRotation = true;      // WebGL �ɂ����ĉE�N���b�N�ł̉�]��L���ɂ��邩�̃t���O
    float gravity = -9.8f;                           // �d�͂̒l

    void Start()
    {
        rb = GetComponent<Rigidbody>();                 // ���g�ɃA�^�b�`����Ă���Rigidbody�R���|�[�l���g���擾
        if (Application.isEditor)
        {
            webGLRightClickRotation = false;                // �G�f�B�^��ł� WebGL �ł̉E�N���b�N��]�𖳌���
            sensitivity = sensitivity * 1.5f;               // �G�f�B�^��ł͊��x��1.5�{�ɑ��₷
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
            gravity = -9.8f;                               // �L�����N�^�[�����ʂ���ɂ���ꍇ�͏d�͂�L����
        }
    }

    void Update()
    {
        moveFB = Input.GetAxis("Horizontal") * speed;      // ���������i���E�j�̓��͂Ɋ�Â��Ĉړ��ʂ��v�Z
        moveLR = Input.GetAxis("Vertical") * speed;        // ���������i�O��j�̓��͂Ɋ�Â��Ĉړ��ʂ��v�Z

        rotX = Input.GetAxis("Mouse X") * sensitivity;     // �}�E�X�� X ���ړ��ʂɊ�Â��ĉ���]�ʂ��v�Z
        rotY = Input.GetAxis("Mouse Y") * sensitivity;     // �}�E�X�� Y ���ړ��ʂɊ�Â��ďc��]�ʂ��v�Z

        CheckForWaterHeight();                             // ���ʂ̍������`�F�b�N���A�K�v�ɉ����ďd�͂𒲐�

        Vector3 movement = new Vector3(moveFB, gravity, moveLR);   // �ړ��ʂ��x�N�g���Ƃ��Đݒ�

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

        movement = transform.rotation * movement;            // �ړ��ʂ��L�����N�^�[�̃��[�J�����W�n�ɕϊ�
        rb.velocity = movement;                              // Rigidbody�̑��x��ݒ�
    }

    void CameraRotation(GameObject cam, float rotX, float rotY)
    {
        transform.Rotate(0, rotX * Time.deltaTime, 0);       // �L�����N�^�[������]
        cam.transform.Rotate(-rotY * Time.deltaTime, 0, 0);  // �J�������c��]
    }
}