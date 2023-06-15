using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenController : MonoBehaviour
{
    private enum EnemyState
    {
        Walk,
        RunAway
    }

    private EnemyState currentState;
    private Vector3 initialPosition;
    [SerializeField]private float detectionRadius = 5f;
    [SerializeField] Transform player; // �v���C���[��Transform�R���|�[�l���g
    float distanceToPlayer = 0f;
    private float idleTimer; // �A�C�h����Ԃ̌o�ߎ���
    [SerializeField] private float moveTimer; // �����]���̃^�C�}�[
    private Animator _anim;
    [SerializeField] float _walkRadius = 20f; // �ǐՔ��a
    private Vector3 randomDestination; // �����_���ȖړI�n
    private float moveDuration = 10f; // �����]���̊Ԋu
    [SerializeField] private float moveRadius = 10f; // �ړ��͈͂̔��a
    [SerializeField] float _runSpeed = 0;
    [SerializeField] float _walkSpeed = 0;
    [SerializeField]private float _runRadius;
    [SerializeField] GameObject itemPrefab; // �A�C�e���̃v���n�u
    [SerializeField] AudioClip destructionSound; // �j�󎞂̉�
    [SerializeField] AudioClip walkSound;
    [SerializeField] AudioClip runSound;
    private AudioSource audioSource; // �I�[�f�B�I�\�[�X


    private void Start()
    {
        // ������Ԃ�ݒ�
        currentState = EnemyState.Walk;
        // �����ʒu��ۑ�
        initialPosition = transform.position;
        // �����̃����_���ȖړI�n��ݒ�
        SetRandomDestination();
        audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // ��Ԃɉ��������������s
        switch (currentState)
        {
            case EnemyState.Walk:
                audioSource.clip = walkSound;
                WalkAround();
                break;
            case EnemyState.RunAway:
                audioSource.clip = runSound;
                RunAwayFromPlayer();
                break;
        }
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        // �v���C���[�ƓG�̋������v�Z
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
    }

    private void WalkAround()
    {
        
        /// �A�C�h����Ԃ̏���
        idleTimer += Time.deltaTime;
        moveTimer += Time.deltaTime;
        if (distanceToPlayer <= _walkRadius) // �v���C���[���ǐՔ��a�ɋ߂Â����ꍇ
        {
            currentState = EnemyState.RunAway;
        }
        else
        {
            if (moveTimer >= moveDuration || Vector3.Distance(transform.position, randomDestination) < 1f)
            {
                // �����]���̊Ԋu���o�߂������A�ړI�n�ɋ߂Â����ꍇ�͐V�����ړI�n��ݒ�
                SetRandomDestination();
                moveTimer = 0f;
            }
            // �ړI�n�Ɍ������Ĉړ�
            transform.position = Vector3.MoveTowards(transform.position, randomDestination, _walkSpeed * Time.deltaTime);

            // �i��ł�������Ɍ�����ς���
            Vector3 direction = randomDestination - transform.position;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
            }
        }
    }
    private void SetRandomDestination()
    {
        // �����_���ȕ����Ɉړ�����ړI�n��ݒ�
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 direction3D = new Vector3(randomDirection.x, 0f, randomDirection.y);
        randomDestination = initialPosition + direction3D * Random.Range(0f, moveRadius);
    }

    private void RunAwayFromPlayer()
    {// Player�Ƃ̕����x�N�g�����v�Z
        Vector3 runDirection = transform.position - player.position;

        // �����x�N�g���𐳋K�����ē����鋗����ݒ�
        runDirection.Normalize();
        float runDistance = detectionRadius * 2f; // �����鋗���͌��o���a��2�{�Ƃ��܂�

        // ������ʒu���v�Z
        Vector3 runPosition = transform.position + runDirection * runDistance;

        // ������ʒu�Ɍ������Ĉړ�
        transform.position = Vector3.MoveTowards(transform.position, runPosition, _runSpeed * Time.deltaTime);

        // �i��ł�������Ɍ�����ς���
        if (runDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(runDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }

        if (distanceToPlayer >= _runRadius)// �v���C���[���ǐՔ��a�͈̔͊O�̏ꍇ
        {
            currentState = EnemyState.Walk;
        }

        _anim.SetBool("Run", true);
        if (distanceToPlayer >= _walkRadius)
        {
            _anim.SetBool("Run", false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Axe")) // ���̃^�O�ɉ����Ĕ���
        {
            AudioSource.PlayClipAtPoint(destructionSound, transform.position);
            // �A�C�e���𐶐�
            Instantiate(itemPrefab, transform.position, Quaternion.identity);
            // �G��j��
            Destroy(gameObject);
        }
    }
}
