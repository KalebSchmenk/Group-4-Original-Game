using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyAIController : MonoBehaviour, EnemyHealthInterface
{
    private enum AIState
    {
        Roam,
        Search,
        Attack,
        Chase
    }

    private AIState _AIState;
    private NavMeshAgent _navMeshAgent;
    private Rigidbody _rb;

    [SerializeField]
    [Range(0.0f, 250.0f)] private float _walkRadius;

    [SerializeField]
    [Range(1.0f, 15.0f)] private float _waitTime;

    [SerializeField]
    [Range(0.0f, 75.0f)] private float _proximityRange;

    [SerializeField] private float _proxCheckWaitTime;
    [SerializeField] private float _navMeshAgentSprintSpeed = 5.0f;
    [SerializeField] private float _navMeshAgentJogSpeed = 4.0f;
    [SerializeField] private float _alwaysSeeRadius = 6.5f;
    [SerializeField] private float _attackCooldown = 2.5f;
    private float _navMeshSpeed;

    private bool _canSeePlayer;
    private bool _IAmWaiting = false;

    private Vector3 _lastPosition;
    private Vector3 _lastKnownLocation;

    private GameObject _player;
    private TestingPlayerController _playerScript;

    [SerializeField] private GameObject _attackSphere;
    [SerializeField] private Transform _attackSphereLocation;

    [SerializeField] protected int _health = 5;
    public int health { get { return _health; } set { _health = value; } }

    [SerializeField] protected Transform _lightingStrikeLocation;
    public Transform hitLocation { get { return _lightingStrikeLocation; } set { _lightingStrikeLocation = value; } }

    private Animator _animation;
    private bool _inAttackCooldown = false;
    

    void Start()
    {
        _AIState = AIState.Roam;
        _navMeshAgent = GetComponent<NavMeshAgent>();
       // _animation = GetComponent<Animator>();

        _player = GameObject.FindGameObjectWithTag("Player");
        //_playerScript = _player.GetComponent<TestingPlayerController>();

        _navMeshSpeed = _navMeshAgent.speed;

        _rb = GetComponent<Rigidbody>();

        NavMesh.avoidancePredictionTime = 0.5f;

        StartCoroutine(ProximityCheck());
    }

    void Update()
    {
        CheckDistance();
        CheckHealth();

        _rb.angularVelocity = Vector3.zero;

        switch (_AIState)
        {

            // Roam around the nav mesh randomly state
            case AIState.Roam:
                Wander();

                if (_canSeePlayer == true) _AIState = AIState.Chase;
                break;

            // Chase the player state
            case AIState.Chase:
                Chase();

                if (_canSeePlayer == false)
                {
                    _AIState = AIState.Search;
                    _lastKnownLocation = _player.transform.position;
                }
                break;

            // Search for the player at their last known position state
            case AIState.Search:
                Search();

                if (_canSeePlayer == true) _AIState = AIState.Chase;

                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && _canSeePlayer == false)
                {
                    _AIState = AIState.Roam;
                }
                break;

            // Attacks player
            case AIState.Attack:
                AttackPlayer();
                _rb.velocity = Vector3.zero;
                break;

        }
    }

    private void CheckHealth()
    {
        if (_health <= 0)
        {
            // Kill enemy
            Destroy(this.gameObject);
        }
    }

    private void CheckDistance()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < 1.5f)
        {
            _AIState = AIState.Attack;
        }
    }

    private void AttackPlayer()
    {
        _navMeshAgent.enabled = false;

        transform.LookAt(_player.transform.position);

        if (_inAttackCooldown) return;

        // Do attack animation

        Instantiate(_attackSphere, _attackSphereLocation.position, Quaternion.identity);

        _inAttackCooldown = true;

        StartCoroutine(AttackCooldown());

        if (Vector3.Distance(transform.position, _player.transform.position) > 1.5f)
        {
            _navMeshAgent.enabled = true;
            _AIState = AIState.Chase;
        }
    }

    // Wanders to a random point
    private void Wander()
    {
        if (_navMeshAgent.remainingDistance >= _navMeshAgent.stoppingDistance) _IAmWaiting = false;

        if (_IAmWaiting == false && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.speed = _navMeshSpeed;
            _IAmWaiting = true;
            StartCoroutine(WaitTimer());
        }
    }

    // Finds a random point on the nav mesh to roam to
    private Vector3 RandomNavMeshLocation()
    {
        // Loop may be performance heavy but ensures
        // that the position is on the nav mesh
        while (true)
        {
            Vector3 targetPosition = Vector3.zero;
            Vector3 randomPosition = Random.insideUnitSphere * _walkRadius;
            randomPosition += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, _walkRadius, 1) && hit.position != _lastPosition)
            {
                targetPosition = hit.position;

                _lastPosition = targetPosition;

                return targetPosition;

            }
        }
    }

    // Guard Chases Player
    private void Chase()
    {
        _navMeshAgent.SetDestination(_player.transform.position);
        _navMeshAgent.speed = _navMeshAgentSprintSpeed;
    }

    // Guard Searches Players Last Known Position
    private void Search()
    {
        _IAmWaiting = false;
        _navMeshAgent.SetDestination(_lastKnownLocation);
        _navMeshAgent.speed = _navMeshAgentJogSpeed;
    }



    // Wait co-routine that halts the guard for a moment after it reaches its destination
    private IEnumerator WaitTimer()
    {
        _IAmWaiting = true;
        yield return new WaitForSeconds(_waitTime);

        // State Roam is the only caller of this function. This is why setting a destination here is acceptable
        _navMeshAgent.SetDestination(RandomNavMeshLocation());
    }

    // Guard Shoots Raycast At Max Range _proximityRange Towards Player To See If Player Is Close Enough To Be Chased
    private IEnumerator ProximityCheck()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, _player.transform.position - transform.position, out hit, _proximityRange))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                Vector3 toPlayer = (_player.transform.position - transform.position).normalized;

                // FOV check using a dot product. If the dot product of the guards forward vector and the direction to the player is greater than
                // 0.55f, we consider the player to be in the visual FOV of the guard
                if (Vector3.Dot(transform.TransformDirection(Vector3.forward).normalized, toPlayer) > 0.55f)
                {
                    _canSeePlayer = true;
                }
                else
                {
                    _canSeePlayer = false;
                }
            }
            else
            {
                _canSeePlayer = false;
            }
        }
        else
        {
            _canSeePlayer = false;
        }

        // If player is very close to the guard, we assume the guard would notice the players presence and consider them as seeing the player
        if (Vector3.Distance(transform.position, _player.transform.position) < _alwaysSeeRadius)
        {
            _canSeePlayer = true;
        }

        yield return new WaitForSeconds(_proxCheckWaitTime);

        StartCoroutine(ProximityCheck());
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);

        _inAttackCooldown = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerAttackSphere"))
        {
            TakeDamage(2);
        }
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        CheckHealth();
        // Play damage sound and any anim
    }

}
