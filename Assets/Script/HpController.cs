using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpController : MonoBehaviour
{
    [SerializeField] public int _maxHp = 100;
    Animator animator;
    int _hp = 0;
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
    }

    void Update()
    {
        
    }
    public void Damage(int damage)
    {
        CurrentHp -= damage;
        if (CurrentHp <= 0)
        {
            CurrentHp = 0;
            animator.SetTrigger("Death");
            Destroy(gameObject, 10f);
        }
    }
}
