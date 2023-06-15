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

    public float currentSp; // ���݂̃X�^�~�i�l

    void Start()
    {
        animator = GetComponent<Animator>();
        _sp = _maxSp;
        currentSp = _maxSp; // �����l���ő�X�^�~�i�ɐݒ�
        UpdateUI(currentSp);
    }

    //UI(�X���C�_�[�ƃe�L�X�g)
    void UpdateUI(float animHP)
    {
        //UI��ύX
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
        //�_���[�W��^����O��HP���擾
        float startSP = currentSp;

        //�_���[�W��^�������HP���v�Z
        currentSp -= damage;
        if (currentSp < 0)
        {
            currentSp = 0;
        }

        // SP��ړI�̒l�܂ŏ��X�ɓ�����
        yield return DOTween.To(() => startSP, (x) => startSP = x, currentSp, animTime)
            .SetEase(Ease.Linear)
            .OnUpdate(() => UpdateUI(startSP));
    }
}
