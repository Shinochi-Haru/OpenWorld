using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    Animator _anim;
    [SerializeField]public Collider attackCollider;
    [SerializeField]HpController _hpController;
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _damageSound;
    Damager damager;
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
            _anim.SetTrigger("Attack");    //マウスクリックで攻撃モーション
        }
        if (Input.GetMouseButtonDown(1))
        {
            _anim.SetTrigger("Attack2");
        }
    }

    //武器の判定を有効or無効切り替える
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

    //被ダメージアニメーションを発生させる
    private void OnTriggerEnter(Collider other)
    {
        Damager damager = other.GetComponent<Damager>();
        if (damager != null)
        {
            //敵の剣に当たったら被ダメアニメーション発生
            _anim.SetTrigger("Damage");
            _audio.PlayOneShot(_damageSound);
            _hpController.Damage(damager.damage);
            StartCoroutine(_hpController.Attacked(damager.damage));
        }
    }
}
