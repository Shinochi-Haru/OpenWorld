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

    public float currentSp; // 現在のスタミナ値

    void Start()
    {
        animator = GetComponent<Animator>();
        _sp = _maxSp;
        currentSp = _maxSp; // 初期値を最大スタミナに設定
        UpdateUI(currentSp);
    }

    //UI(スライダーとテキスト)
    void UpdateUI(float animHP)
    {
        //UIを変更
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
        currentSp -= amount;
        if (currentSp < 0)
        {
            currentSp = 0;
        }

        UpdateUI(currentSp);
    }

    public void RecoverStamina(int amount)
    {
        currentSp += amount;
        if (currentSp > _maxSp)
        {
            currentSp = _maxSp;
        }

        UpdateUI(currentSp);
    }

    public IEnumerator Attacked(int damage)
    {
        //ダメージを与える前のHPを取得
        float startSP = currentSp;

        //ダメージを与えた後のHPを計算
        currentSp -= damage;
        if (currentSp < 0)
        {
            currentSp = 0;
        }

        // SPを目的の値まで徐々に動かす
        yield return DOTween.To(() => startSP, (x) => startSP = x, currentSp, animTime)
            .SetEase(Ease.Linear)
            .OnUpdate(() => UpdateUI(startSP));
    }
}
