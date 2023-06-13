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
    //�A�j���[�V�����̎���
    [SerializeField] float animTime;
    [SerializeField] float staminaDecreaseRate = 1f; // �X�^�~�i�̌������x

    void Start()
    {
        animator = GetComponent<Animator>();
        _sp = _maxSp;
        UpdateUI(_sp);
    }

    //UI(�X���C�_�[�ƃe�L�X�g)
    void UpdateUI(float animHP)
    {
        //UI��ύX
        //text.text = $"{(int)animHP}/{_maxHp}";
        slider.value = animHP / _maxSp;

        sliderFill.color = Color.yellow;
    }

    void Update()
    {
        // �X�^�~�i�����Ԍo�߂Ō���������
        DecreaseStamina(Time.deltaTime * staminaDecreaseRate);
    }

    // �X�^�~�i�����������郁�\�b�h
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
        //�_���[�W��^����O��HP���擾
        float animSP = _sp;

        //�_���[�W��^�������HP���v�Z
        _sp -= damage;

        //HP��ړI�̒l�܂œ�����
        yield return DOTween.To(() => animSP, (x) => animSP = x, _sp, animTime)
            .SetEase(Ease.Linear)
            .OnUpdate(() => UpdateUI(animSP));
    }
}