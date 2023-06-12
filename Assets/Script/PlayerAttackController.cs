using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    Animator _anim;
    [SerializeField]public Collider attackCollider;
    private HpController _hpController;
    void Start()
    {
        attackCollider.enabled = false;
        _anim = GetComponent<Animator>();
        _hpController = GetComponent<HpController>();
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

    //��_���[�W�A�j���[�V�����𔭐�������
    private void OnTriggerEnter(Collider other)
    {
        Damager damager = other.GetComponent<Damager>();
        if (damager != null)
        {
            //�G�̌��ɓ����������_���A�j���[�V��������
            _anim.SetTrigger("Damage");
            _hpController.Damage(damager.damage);
        }
    }
}
