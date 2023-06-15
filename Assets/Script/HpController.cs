using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HpController : MonoBehaviour
{
    [SerializeField] public int _maxHp = 100;
    Animator animator;
    int _hp = 0;
    //UI
    [SerializeField] Text text;
    [SerializeField] Slider slider;
    [SerializeField] Image sliderFill;
    [SerializeField] Color color25;
    [SerializeField] Color color05;
    [SerializeField] Color color1;
    //アニメーションの時間
    [SerializeField] float animTime;

    public int MaxHp
    {
        get { return _maxHp; }
        set { _maxHp = value; }
    }

    public int CurrentHp
    {
        get { return _hp; }
        private set { _hp = value; }
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        _hp = _maxHp;
        UpdateUI(_hp);
    }

    //UI(スライダーとテキスト)
    void UpdateUI(float animHP)
    {
        //UIを変更
        //text.text = $"{(int)animHP}/{_maxHp}";
        slider.value = animHP / _maxHp;

        //残りHPに応じてスライダーの色を変更
        if (slider.value < 0.25f)
        {
            sliderFill.color = Color.red;
        }
        else if (slider.value < 0.5f)
        {
            sliderFill.color = Color.yellow;
        }
        else
        {
            sliderFill.color = Color.green;
        }
    }
    void Update()
    {
        
    }

    public IEnumerator Attacked(int damage)
    {
        //ダメージを与える前のHPを取得
        float animHP = _hp;

        //ダメージを与えた後のHPを計算
        _hp -= damage;

        //HPを目的の値まで動かす
        yield return DOTween.To(() => animHP, (x) => animHP = x, _hp, animTime)
            .SetEase(Ease.Linear)
            .OnUpdate(() => UpdateUI(animHP));
    }

    public void RecoverHealth(int amount)
    {
        CurrentHp += amount;
        if (CurrentHp > _maxHp)
        {
            CurrentHp = _maxHp;
        }
        UpdateUI(CurrentHp);
    }
    public void Damage(int damage)
    {
        CurrentHp -= damage;
        if (CurrentHp <= 0)
        {
            CurrentHp = 0;
            animator.SetTrigger("Death");
            Destroy(gameObject, 1f);
        }
        UpdateUI(CurrentHp);
    }
}
