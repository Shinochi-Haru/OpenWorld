using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // �X�e�[�g�̗񋓌^
    private enum EnemyState
    {
        Idle,
        Chase,
        Attack
    }

    private EnemyState currentState; // ���݂̃X�e�[�g
    private float idleTimer; // �A�C�h����Ԃ̌o�ߎ���
    private float idleDuration = 100f; // �A�C�h����Ԃ̎�������
    private Vector3 idleDestination; // �A�C�h�����̖ړI�n
    [SerializeField] Transform player; // �v���C���[��Transform�R���|�[�l���g
    [SerializeField] float chaseRadius = 20f; // �ǐՔ��a
    float distanceToPlayer = 0f;
    [SerializeField]private float moveTimer; // �����]���̃^�C�}�[
    private float moveDuration = 10f; // �����]���̊Ԋu
    private Vector3 randomDestination; // �����_���ȖړI�n
    [SerializeField] private float moveRadius = 10f; // �ړ��͈͂̔��a
    private Vector3 initialPosition; // �����ʒu


    private void Start()
    {
        // ������Ԃ�ݒ�
        currentState = EnemyState.Idle;

        // �����ʒu��ۑ�
        initialPosition = transform.position;
        // �����̃����_���ȖړI�n��ݒ�
        SetRandomDestination();
    }

    private void Update()
    {
        // ��Ԃɉ��������������s
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
        // �v���C���[�ƓG�̋������v�Z
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
    }

    private void UpdateIdleState()
    {
        // �A�C�h����Ԃ̏���
        idleTimer += Time.deltaTime;
        moveTimer += Time.deltaTime;

        if (distanceToPlayer <= chaseRadius) // �v���C���[���ǐՔ��a�ɋ߂Â����ꍇ
        {
            currentState = EnemyState.Chase;
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
            transform.position = Vector3.MoveTowards(transform.position, randomDestination, 2f * Time.deltaTime);

            // �i��ł�������Ɍ�����ς���
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
        // �v���C���[�̕���������
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // �v���C���[�Ɍ������Ĉړ�����
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * 5f);

        if (distanceToPlayer >= chaseRadius)// �v���C���[���ǐՔ��a�͈̔͊O�̏ꍇ
        {
            currentState = EnemyState.Idle;
        }
    }

    private void UpdateAttackState()
    {
        
    }

    private void SetRandomDestination()
    {
        // �����_���ȕ����Ɉړ�����ړI�n��ݒ�
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 direction3D = new Vector3(randomDirection.x, 0f, randomDirection.y);
        randomDestination = initialPosition + direction3D * Random.Range(0f, moveRadius);
    }
}
