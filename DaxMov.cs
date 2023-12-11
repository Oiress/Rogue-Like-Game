using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DaxMov : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Rigidbody2D _rigidbody;
    private Vector2 _movementInput;
    private bool isDashing;
    private bool canDash = true;
    private SpriteRenderer _spriteRenderer;

    [Header("Dash")]
    [SerializeField] float _dashSpeedMultiplier;
    [SerializeField] float _dashTime;
    [SerializeField] float _dashCooldown;

    [Header("Animacion")]
    private Animator _animator;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            _rigidbody.velocity = _movementInput;

            // Escalado del sprite
            if (_movementInput.x < 0)
            {
                _spriteRenderer.flipX = true; // Voltea el sprite a la izquierda
            }
            else if (_movementInput.x > 0)
            {
                _spriteRenderer.flipX = false; // Restaura el sprite a su escala original
            }
        }
    }

    private void OnMove(InputValue inputValue)
    {
        if (!isDashing)
        {
            _movementInput = inputValue.Get<Vector2>() * _speed;
        }
        if (_movementInput.magnitude > 0)
        {
            _animator.SetBool("isWalking", true);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }
    }

    private IEnumerator OnDash()
    {
        if (canDash)
        {
            isDashing = true;
            canDash = false;

            Vector2 dashDirection = _movementInput.normalized * _speed * _dashSpeedMultiplier;
            _rigidbody.velocity = dashDirection;

            _animator.SetBool("isDashing", true);

            

            yield return new WaitForSeconds(_dashTime);

            _animator.SetBool("isDashing", false);
            isDashing = false;

            yield return new WaitForSeconds(_dashCooldown);
            canDash = true;
        }
    }
}