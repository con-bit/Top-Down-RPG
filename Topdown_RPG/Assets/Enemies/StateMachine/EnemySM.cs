using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.GraphicsBuffer;
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

//This is the statemachine class, of which all the states should be children.
public class EnemySM : MonoBehaviour, IDamageable, IKnockbackable {

    public event EventHandler OnDeath;
    public static event EventHandler OnAnyDeath;

    [SerializeField] private float _defaultMovementSpeed;
    public float DefaultMovementSpeed {
        get {
            return _defaultMovementSpeed;
        }
    }
    [SerializeField] private float _defaultMovementForce;
    public float DefaultMovementForce {
        get {
            return _defaultMovementForce;
        }
    }

    private Rigidbody2D _rb;
    public Rigidbody2D RB {
        get {
            return _rb;
        }
    }
    private CircleCollider2D _collider;
    public CircleCollider2D Collider {
        get {
            return _collider;
        }
    }

    //This is what the movement function actually uses to figure out where to go and how fast
    private EnemyMovementParameters _movementParameters;
    public EnemyMovementParameters MovementParameters {
        get {
            return _movementParameters;
        }
        set {
            _movementParameters = value;
        }
    }
    private Vector2 _target;
    public Vector2 Target {
        get {
            return _target;
        }
        set {
            _target = value;
        }
    }

    private void Awake() {
        _currentHealth = _maxHealth;
        isDead = false;
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
        ResetMovementParameters();
    }
    [SerializeField] private EnemyState _defaultState;
    private EnemyState _currentState;
    public EnemyState CurrentState {
        get {
            return _currentState;
        }
    }

    private void Start() {
        SwitchState(GetDefaultState());
    }

    /// <summary>
    /// Gets the default state, logs warnings if it's not set in the inspector and errors if there are no states at all
    /// </summary>
    /// <returns>the default state or null</returns>
    private EnemyState GetDefaultState() {
        if(_defaultState != null) {
            return _defaultState;
        }
        Debug.LogWarning($"Problem with {gameObject.name}: No default state assigned in inspector!");
        EnemyState[] states = GetComponentsInChildren<EnemyState>(true);
        if (states.Length == 0) {
            Debug.LogError($"Fatal problem with {gameObject.name}: No states found in children at all. The state machine needs at least one EnemyState in its children to operate.");
            return null;
        }
        foreach (EnemyState state in states) {
            if (state.transform.parent == this) {
                _defaultState = state;
                return _defaultState;
            }
        }
        Debug.LogError($"Fatal problem with {gameObject.name}: Make sure at least one EnemyState is in a direct child of the state machine.");
        return null;
    }


    /// <summary>
    /// Disables the siblings of the current state, then enables the new state
    /// </summary>
    /// <param name="newState">the new state to enable</param>
    /// <param name="caller">the EnemyState that called the function</param>
    /// <returns>void</returns>
    public void SwitchState(EnemyState newState) {
        Transform parent = newState.transform.parent;

        EnemyState[] states = parent.gameObject.GetComponentsInChildren<EnemyState>(true);
        foreach (EnemyState state in states) {
            if (state.transform != parent) {
                state.gameObject.SetActive(false);
            }

        }
        ResetMovementParameters();
        newState.gameObject.SetActive(true);
        _currentState = newState;
    }

    public void ExitStateImmediate() {
        _currentState.ExitStateImmediate();
    }

    /// <summary>
    /// Moves the character around with physics
    /// </summary>
    /// <returns>void</returns>
    private void HandleMovement() {
        Vector2 pos = transform.position;
        Vector2 movementDirection = (_movementParameters.Target - pos).normalized;

        Vector2 desiredV = movementDirection * _movementParameters.Speed;
        Vector2 differenceV = desiredV - _rb.velocity;


        float differenceFactor = differenceV.magnitude / Mathf.Max(_movementParameters.Speed, 1f);

        _rb.AddForce(differenceV.normalized * differenceFactor * _movementParameters.Force);
    }

    private void FixedUpdate() {
        HandleMovement();
    }

    //Sets the speed and force to default for the character, and the goal destionation to the current position (stop moving)
    public void ResetMovementParameters() {
        _movementParameters = new EnemyMovementParameters();
        _movementParameters.Target = transform.position;
        _movementParameters.Speed = _defaultMovementSpeed;
        _movementParameters.Force = _defaultMovementForce;
    }


    [SerializeField] private float _maxHealth;
    private float _currentHealth;
    public void Damage(float amount)
    {
        _currentHealth -= amount;
        if(_currentHealth < 0){
            Die();
        }


    }
    private bool isDead = false;
    private void Die() {
        if (!isDead) {
            isDead = true;
            OnDeath?.Invoke(this, EventArgs.Empty);
            OnAnyDeath?.Invoke(this, EventArgs.Empty);
        }


        Destroy(gameObject);
    }

    public void Knockback(Vector2 force) {
        _rb.AddForce(force, ForceMode2D.Impulse);
    }

}
public struct EnemyMovementParameters {
    public EnemyMovementParameters(Vector2 target, float speed, float force) {
        Target = target;
        Speed = speed;
        Force = force;
    }
    public Vector2 Target;
    public float Speed;
    public float Force;
}