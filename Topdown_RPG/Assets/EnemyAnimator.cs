using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(EnemySM))]
public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private bool _usesRigidbodyToSetParameters = true;
    [SerializeField] private string _xDirectionParameterName = "xDir";
    [SerializeField] private string _yDirectionParameterName = "yDir";
    [SerializeField] private string _speedParameterName = "Speed";

    [SerializeField] private bool _flipSpriteBasedOnXDirection = true;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Animator _anim;
    private EnemySM _enemySM;

    private Rigidbody2D _rb;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _enemySM = GetComponent<EnemySM>();

        if (_usesRigidbodyToSetParameters)
            _rb = GetComponent<Rigidbody2D>();
    
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void UpdateParametersFromRigidbody()
    {
        var vel = _rb.velocity;

        if (_flipSpriteBasedOnXDirection)
            _spriteRenderer.flipX = vel.normalized.x < 0.0f;

        _anim.SetFloat(_xDirectionParameterName, vel.normalized.x);
        _anim.SetFloat(_yDirectionParameterName, vel.normalized.y);
        // TODO need to normalize speed here to maximum enemy velocity magnitude
        _anim.SetFloat(_speedParameterName, vel.magnitude);
    }

    void Update()
    {
        if (_usesRigidbodyToSetParameters && _rb != null)
            UpdateParametersFromRigidbody();
    }
}
