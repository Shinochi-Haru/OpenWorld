using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    Animator _anim;
    [SerializeField]public Collider attackCollider;
    [SerializeField]HpController _hpController;
    Damager damager;
    [SerializeField] private float knockbackForce = 10f; // ノックバックの力
    [SerializeField] private float knockbackAngle = 45f; // ノックバックの角度（扇の開き具合）
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
    public void ApplyKnockback()
    {
        // 前方向ベクトルを取得
        Vector3 forward = transform.forward;

        // 扇状の範囲に対してプレーヤーに向かっての方向ベクトルを計算
        Vector3 direction = Quaternion.AngleAxis(-knockbackAngle / 2, Vector3.up) * forward;

        // プレーヤーに対しての方向ベクトルをノックバック力で乗算して力を与える
        GetComponent<Rigidbody>().AddForce(direction * knockbackForce, ForceMode.Impulse);
    }

    //被ダメージアニメーションを発生させる
    private void OnTriggerEnter(Collider other)
    {
        Damager damager = other.GetComponent<Damager>();
        if (damager != null)
        {
            //敵の剣に当たったら被ダメアニメーション発生
            _anim.SetTrigger("Damage");
            _hpController.Damage(damager.damage);
            StartCoroutine(_hpController.Attacked(damager.damage));
        }
    }
}
