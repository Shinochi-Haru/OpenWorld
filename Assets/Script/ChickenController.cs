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
    [SerializeField] Transform player; // プレイヤーのTransformコンポーネント
    float distanceToPlayer = 0f;
    private float idleTimer; // アイドル状態の経過時間
    [SerializeField] private float moveTimer; // 方向転換のタイマー
    private Animator _anim;
    [SerializeField] float _walkRadius = 20f; // 追跡半径
    private Vector3 randomDestination; // ランダムな目的地
    private float moveDuration = 10f; // 方向転換の間隔
    [SerializeField] private float moveRadius = 10f; // 移動範囲の半径
    [SerializeField] float _runSpeed = 0;
    [SerializeField] float _walkSpeed = 0;
    [SerializeField]private float _runRadius;
    [SerializeField] GameObject itemPrefab; // アイテムのプレハブ
    [SerializeField] AudioClip destructionSound; // 破壊時の音
    [SerializeField] AudioClip walkSound;
    [SerializeField] AudioClip runSound;
    private AudioSource audioSource; // オーディオソース


    private void Start()
    {
        // 初期状態を設定
        currentState = EnemyState.Walk;
        // 初期位置を保存
        initialPosition = transform.position;
        // 初期のランダムな目的地を設定
        SetRandomDestination();
        audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // 状態に応じた処理を実行
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
        // プレイヤーと敵の距離を計算
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
    }

    private void WalkAround()
    {
        
        /// アイドル状態の処理
        idleTimer += Time.deltaTime;
        moveTimer += Time.deltaTime;
        if (distanceToPlayer <= _walkRadius) // プレイヤーが追跡半径に近づいた場合
        {
            currentState = EnemyState.RunAway;
        }
        else
        {
            if (moveTimer >= moveDuration || Vector3.Distance(transform.position, randomDestination) < 1f)
            {
                // 方向転換の間隔が経過したか、目的地に近づいた場合は新しい目的地を設定
                SetRandomDestination();
                moveTimer = 0f;
            }
            // 目的地に向かって移動
            transform.position = Vector3.MoveTowards(transform.position, randomDestination, _walkSpeed * Time.deltaTime);

            // 進んでいる方向に向きを変える
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
        // ランダムな方向に移動する目的地を設定
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 direction3D = new Vector3(randomDirection.x, 0f, randomDirection.y);
        randomDestination = initialPosition + direction3D * Random.Range(0f, moveRadius);
    }

    private void RunAwayFromPlayer()
    {// Playerとの方向ベクトルを計算
        Vector3 runDirection = transform.position - player.position;

        // 方向ベクトルを正規化して逃げる距離を設定
        runDirection.Normalize();
        float runDistance = detectionRadius * 2f; // 逃げる距離は検出半径の2倍とします

        // 逃げる位置を計算
        Vector3 runPosition = transform.position + runDirection * runDistance;

        // 逃げる位置に向かって移動
        transform.position = Vector3.MoveTowards(transform.position, runPosition, _runSpeed * Time.deltaTime);

        // 進んでいる方向に向きを変える
        if (runDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(runDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }

        if (distanceToPlayer >= _runRadius)// プレイヤーが追跡半径の範囲外の場合
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
        if (other.CompareTag("Axe")) // 斧のタグに応じて判定
        {
            AudioSource.PlayClipAtPoint(destructionSound, transform.position);
            // アイテムを生成
            Instantiate(itemPrefab, transform.position, Quaternion.identity);
            // 敵を破壊
            Destroy(gameObject);
        }
    }
}
