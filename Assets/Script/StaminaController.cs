using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    [SerializeField] public int _maxSp = 100;
    Animator animator;
    int _sp = 0;
    //UI
    [SerializeField] Text text;
    [SerializeField] Slider slider;
    [SerializeField] Image sliderFill;
    //アニメーションの時間
    [SerializeField] float animTime;
    [SerializeField] float staminaDecreaseRate = 1f; // スタミナの減少速度

    void Start()
    {
        animator = GetComponent<Animator>();
        _sp = _maxSp;
        UpdateUI(_sp);
    }

    //UI(スライダーとテキスト)
    void UpdateUI(float animHP)
    {
        //UIを変更
        //text.text = $"{(int)animHP}/{_maxHp}";
        slider.value = animHP / _maxSp;

        sliderFill.color = Color.yellow;
    }

    void Update()
    {
        // スタミナを時間経過で減少させる
        DecreaseStamina(Time.deltaTime * staminaDecreaseRate);
    }

    // スタミナを減少させるメソッド
    void DecreaseStamina(float amount)
    {
        _sp -= (int)amount;
        if (_sp < 0)
        {
            _sp = 0;
        }

        UpdateUI(_sp);
    }

    public void RecoverStamina(int amount)
    {
        _sp += amount;
        if (_sp > _maxSp)
        {
            _sp = _maxSp;
        }

        UpdateUI(_sp);
    }

    public IEnumerator Attacked(int damage)
    {
        //ダメージを与える前のHPを取得
        float animSP = _sp;

        //ダメージを与えた後のHPを計算
        _sp -= damage;

        //HPを目的の値まで動かす
        yield return DOTween.To(() => animSP, (x) => animSP = x, _sp, animTime)
            .SetEase(Ease.Linear)
            .OnUpdate(() => UpdateUI(animSP));
    }
}