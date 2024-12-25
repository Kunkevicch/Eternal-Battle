using System.Collections;
using Unity.Behavior;
using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public abstract class EnemyBase : MonoBehaviour
    {
        [SerializeField] protected WeaponConfig _weaponConfig;
        [SerializeField] private LayerMask _visiubleLayer;
        [SerializeField] private int _reward;
        private int _id = -1;

        protected bool _canAttack = true;
        protected EnemyCombatBase _combat;
        protected EnemyAnimatorBase _animator;
        protected Health _health;
        protected BehaviorGraphAgent _agent;
        protected CapsuleCollider _bodyCollider;
        protected EventBus _eventBus;

        [Inject]
        public void Construct(EventBus eventBus)
        {
            _eventBus = eventBus;
        }
        public int ID => _id;
        public int Reward => _reward;
        public bool CanAtack => _canAttack;

        public LayerMask VisiubleLayer => _visiubleLayer;
        public bool IsDead { get; private set; }

        protected virtual void Awake()
        {
            _health = GetComponent<Health>();
            _bodyCollider = GetComponent<CapsuleCollider>();
            _animator = GetComponent<EnemyAnimatorBase>();
            _combat = GetComponent<EnemyCombatBase>();
            _agent = GetComponent<BehaviorGraphAgent>();
        }

        protected virtual void OnEnable() => _health.Dead += OnDead;

        protected virtual void OnDisable() => _health.Dead -= OnDead;

        public void SetID(int id) => _id = id;
        public void SetPlayer(GameObject player) => _agent.SetVariableValue("Player", player);

        public virtual void Activate()
        {
            _agent.enabled = true;
            _health.Revive();
            _animator.Revive();
            _bodyCollider.enabled = true;
            _agent.BlackboardReference.SetVariableValue("CurrentState", State.Idle);
            _agent.Restart();
        }

        public void Deactivate()
        {
            if (!IsDead)
            {
                _agent.BlackboardReference.SetVariableValue("CurrentState", State.Idle);
            }
            _agent.enabled = false;
        }

        public void Clear()
        {
            _id = -1;
            IsDead = false;
            gameObject.SetActive(false);
        }

        public void StartMove() => _animator.PlayMove();

        // работа с атакой
        public void StartCombatProcess() => _animator.PlayAttack();

        public virtual void CombatProcess() => _combat.Attack();

        public virtual void DeadProcess()
        {
            _bodyCollider.enabled = false;
            _animator.PlayDead();
            _eventBus.RaiseEnemyDied(this);
            _id = -1;
            StartCoroutine(BodyDisablingRoutine());
        }

        private IEnumerator BodyDisablingRoutine()
        {
            yield return new WaitForSeconds(4f);
            IsDead = false;
            gameObject.SetActive(false);
        }

        private void OnDead() => IsDead = true;
        public abstract void Initialize();
    }
}