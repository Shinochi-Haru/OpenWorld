using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    Animator _anim;
    [SerializeField]public Collider attackCollider;
    [SerializeField]HpController _hpController;
    Damager damager;
    [SerializeField] private float knockbackForce = 10f; // �m�b�N�o�b�N�̗�
    [SerializeField] private float knockbackAngle = 45f; // �m�b�N�o�b�N�̊p�x�i��̊J����j
    void Start()
    {
        attackCollider.enabled = false;
        _anim = GetComponent<Animator>();
        _hpController = GetComponent<HpController>();
        damager = GetComponent<Damager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _anim.SetTrigger("Attack");    //�}�E�X�N���b�N�ōU�����[�V����
        }
        if (Input.GetMouseButtonDown(1))
        {
            _anim.SetTrigger("Attack2");    
        }
    }

    //����̔����L��or�����؂�ւ���
    public void OffColliderAttack()
    {
        attackCollider.enabled = false;
        Debug.Log("off");
    }
    public void OnColliderAttack()
    {
        attackCollider.enabled = true;
        Debug.Log("on");
    }
    public void ApplyKnockback()
    {
        // �O�����x�N�g�����擾
        Vector3 forward = transform.forward;

        // ���͈̔͂ɑ΂��ăv���[���[�Ɍ������Ă̕����x�N�g�����v�Z
        Vector3 direction = Quaternion.AngleAxis(-knockbackAngle / 2, Vector3.up) * forward;

        // �v���[���[�ɑ΂��Ă̕����x�N�g�����m�b�N�o�b�N�͂ŏ�Z���ė͂�^����
        GetComponent<Rigidbody>().AddForce(direction * knockbackForce, ForceMode.Impulse);
    }

    //��_���[�W�A�j���[�V�����𔭐�������
    private void OnTriggerEnter(Collider other)
    {
        Damager damager = other.GetComponent<Damager>();
        if (damager != null)
        {
            //�G�̌��ɓ����������_���A�j���[�V��������
            _anim.SetTrigger("Damage");
            _hpController.Damage(damager.damage);
            StartCoroutine(_hpController.Attacked(damager.damage));
        }
    }
}
