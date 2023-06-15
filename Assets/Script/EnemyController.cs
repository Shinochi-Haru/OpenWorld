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

    private EnemyState _currentState; // ���݂̃X�e�[�g
    [SerializeField] Transform _player; // �v���C���[��Transform�R���|�[�l���g
    [SerializeField] float _chaseRadius = 20f; // �ǐՔ��a
    [SerializeField] float _attackRadius = 20f; // �ǐՔ��a
    float _distanceToPlayer = 0f;
    [SerializeField]private float moveTimer; // �����]���̃^�C�}�[
    private float moveDuration = 10f; // �����]���̊Ԋu
    private Vector3 randomDestination; // �����_���ȖړI�n
    [SerializeField] private float moveRadius = 10f; // �ړ��͈͂̔��a
    private Vector3 initialPosition; // �����ʒu
    [SerializeField] float _walkSpeed = 0;
    [SerializeField] float _chaseSpeed = 0;
    private Animator _anim; // Animator�R���|�[�l���g
    [SerializeField]public Collider _enemyAttackCollider;
    private float attackTimer; // �ߐڍU���̃^�C�}�[
    [SerializeField]private float attackInterval = 3f; // �ߐڍU���̊Ԋu
    [SerializeField] private HpController _hpController;
    [SerializeField]Damager damager;
    [SerializeField] AudioClip destructionSound; // �j�󎞂̉�
    [SerializeField] AudioClip idleSound;
    [SerializeField] AudioClip chaseSound;
    [SerializeField] AudioClip _damageSound;
    private AudioSource audioSource; // �I�[�f�B�I�\�[�X

    private void Start()
    {
        _enemyAttackCollider.enabled = false;
        // ������Ԃ�ݒ�
        _currentState = EnemyState.Idle;
        // �����ʒu��ۑ�
        initialPosition = transform.position;
        // �����̃����_���ȖړI�n��ݒ�
        SetRandomDestination();
        _hpController = GetComponent<HpController>();
        _anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // ��Ԃɉ��������������s
        switch (_currentState)
        {
            case EnemyState.Idle:
                UpdateIdleState();
                audioSource.clip = idleSound;
                break;
            case EnemyState.Chase:
                UpdateChaseState();
                audioSource.clip = chaseSound;
                break;
            case EnemyState.Attack:
                UpdateAttackState();
                break;
        }
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        // �v���C���[�ƓG�̋������v�Z
        _distanceToPlayer = Vector3.Distance(transform.position, _player.position);
    }

    private void UpdateIdleState()
    {
        // �A�C�h����Ԃ̏���
        moveTimer += Time.deltaTime;

        if (_distanceToPlayer <= _chaseRadius) // �v���C���[���ǐՔ��a�ɋ߂Â����ꍇ
        {
            _currentState = EnemyState.Chase;
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
            transform.position = Vector3.MoveTowards(transform.position, randomDestination, _walkSpeed* Time.deltaTime);

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
        Vector3 direction = (_player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // �v���C���[�Ɍ������Ĉړ�����
        transform.position = Vector3.MoveTowards(transform.position, _player.position, Time.deltaTime * _chaseSpeed);
        _anim.SetBool("Chase", true);

        if (_distanceToPlayer >= _chaseRadius)// �v���C���[���ǐՔ��a�͈̔͊O�̏ꍇ
        {
            _currentState = EnemyState.Idle;
            _anim.SetBool("Chase", false);
        }
        if (_distanceToPlayer <= _attackRadius)// �v���C���[���ǐՔ��a�͈͓̔��̏ꍇ
        {
            _currentState = EnemyState.Attack;
            _anim.SetBool("Chase", false);
        }
    }

    private void UpdateAttackState()
    {
        if (_distanceToPlayer >= _attackRadius && _distanceToPlayer <= _chaseRadius)// �v���C���[���ǐՔ��a�͈̔͊O�̏ꍇ
        {
            _currentState = EnemyState.Chase;
        }
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackInterval)
        {
            _anim.SetTrigger("EnemyAttack");
            attackTimer = 0f;
        }
    }

    private void SetRandomDestination()
    {
        // �����_���ȕ����Ɉړ�����ړI�n��ݒ�
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 direction3D = new Vector3(randomDirection.x, 0f, randomDirection.y);
        randomDestination = initialPosition + direction3D * Random.Range(0f, moveRadius);
    }

    //��_���[�W�A�j���[�V�����𔭐�������
    private void OnTriggerEnter(Collider other)
    {
        damager = other.GetComponent<Damager>();
        if (damager != null)
        {
            //�G�̌��ɓ����������_���A�j���[�V��������
            _anim.SetTrigger("Damage");
            _hpController.Damage(damager.damage);
            audioSource.PlayOneShot(_damageSound);
            StartCoroutine(_hpController.Attacked(damager.damage));
        }
    }
    //����̔����L��or�����؂�ւ���
    public void EnemyOffColliderAttack()
    {
        _enemyAttackCollider.enabled = false;
        Debug.Log("Emeny Off");
    }
    public void EnemyOnColliderAttack()
    {
        _enemyAttackCollider.enabled = true;
        Debug.Log("Emeny On");
    }
}
