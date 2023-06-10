using UnityEngine;

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

    private void Start()
    {
        // 初期状態を設定
        currentState = EnemyState.Idle;
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

        if (distanceToPlayer <= chaseRadius)// プレイヤーが追跡半径に近づいた場合
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            // ランダムな方向に歩き回る
            if (Random.Range(0f, 1f) < 0.01f || Vector3.Distance(transform.position, idleDestination) < 1f)
            {
                // 10メートル以上移動する目的地を設定
                Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
                idleDestination = transform.position + randomDirection * Random.Range(20f, 40f);
            }
            // 目的地に向かって移動
            transform.position = Vector3.MoveTowards(transform.position, idleDestination, 2f * Time.deltaTime);
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
}
