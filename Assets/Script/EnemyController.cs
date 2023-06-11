using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // ステートの列挙型
    private enum EnemyState
    {
        Idle,
        Chase,
        Attack
    }

    private EnemyState currentState; // 現在のステート
    private float idleTimer; // アイドル状態の経過時間
    private float idleDuration = 100f; // アイドル状態の持続時間
    private Vector3 idleDestination; // アイドル時の目的地
    [SerializeField] Transform player; // プレイヤーのTransformコンポーネント
    [SerializeField] float chaseRadius = 20f; // 追跡半径
    float distanceToPlayer = 0f;
    [SerializeField]private float moveTimer; // 方向転換のタイマー
    private float moveDuration = 10f; // 方向転換の間隔
    private Vector3 randomDestination; // ランダムな目的地
    [SerializeField] private float moveRadius = 10f; // 移動範囲の半径
    private Vector3 initialPosition; // 初期位置


    private void Start()
    {
        // 初期状態を設定
        currentState = EnemyState.Idle;

        // 初期位置を保存
        initialPosition = transform.position;
        // 初期のランダムな目的地を設定
        SetRandomDestination();
    }

    private void Update()
    {
        // 状態に応じた処理を実行
        switch (currentState)
        {
            case EnemyState.Idle:
                UpdateIdleState();
                break;
            case EnemyState.Chase:
                UpdateChaseState();
                break;
            case EnemyState.Attack:
                UpdateAttackState();
                break;
        }
        // プレイヤーと敵の距離を計算
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
    }

    private void UpdateIdleState()
    {
        // アイドル状態の処理
        idleTimer += Time.deltaTime;
        moveTimer += Time.deltaTime;

        if (distanceToPlayer <= chaseRadius) // プレイヤーが追跡半径に近づいた場合
        {
            currentState = EnemyState.Chase;
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
            transform.position = Vector3.MoveTowards(transform.position, randomDestination, 2f * Time.deltaTime);

            // 進んでいる方向に向きを変える
            Vector3 direction = randomDestination - transform.position;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
            }
        }
    }

    private void UpdateChaseState()
    {
        // プレイヤーの方向を向く
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // プレイヤーに向かって移動する
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * 5f);

        if (distanceToPlayer >= chaseRadius)// プレイヤーが追跡半径の範囲外の場合
        {
            currentState = EnemyState.Idle;
        }
    }

    private void UpdateAttackState()
    {
        
    }

    private void SetRandomDestination()
    {
        // ランダムな方向に移動する目的地を設定
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 direction3D = new Vector3(randomDirection.x, 0f, randomDirection.y);
        randomDestination = initialPosition + direction3D * Random.Range(0f, moveRadius);
    }
}
