using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounce : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private float _bounceForce;
    [SerializeField] private float _bounceHeight;

    [SerializeField] private SpriteRenderer _paintSplashPrefab;

    [SerializeField] private GameObject _particleSystem;

    [SerializeField] private Animator _anim;

    [SerializeField] public TrailRenderer _trail;
    [SerializeField] public TrailRenderer _trailSuper;

    [SerializeField] private AudioSource _splashAudio;
    [SerializeField] public AudioSource _passingAudio;

    private bool _isNextCollisionAllowed = true;

    public int totalRingsPassed = 0;

    public int perfectPasses = 0;
    public bool isSuperSpeedActive;

    private bool _ballBounced;
    private bool _ballSlowedDown;
    private float _yJumpStartPosition;

    [SerializeField] private float _jumpVelocity = 0.5f;
    private float jumpVelocity;
    [SerializeField] private float _jumpDampening = 0.1f;

    private void Start()
    {
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
            _rb = rb;

        if (TryGetComponent(out Animator anim))
            _anim = anim;

        _trail.material.color = GameManager.Instance.ballColor;


    }

    private void Jump()
    {
        jumpVelocity = _jumpVelocity;
        _rb.useGravity = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isNextCollisionAllowed == false)
            return;

        if (collision.gameObject.TryGetComponent(out DeathPart deathPart))
        {
            if (isSuperSpeedActive == false)
                GameManager.Instance.gameOver = true;
        }

        if (isSuperSpeedActive)
        {
            if (!collision.gameObject.TryGetComponent<Goal>(out Goal goal))
            {
                totalRingsPassed++;
                GameManager.Instance.UpdateSlide();
                Destroy(collision.gameObject.transform.parent.gameObject);
            }
        }
        _ballBounced = true;

        _rb.velocity = Vector3.zero;
        _yJumpStartPosition = transform.position.y;

        Jump();

        //_rb.AddForce(Vector3.up * _bounceForce, ForceMode.Impulse);

        if (isSuperSpeedActive == false)
            SpawnSplash(collision.gameObject);

        if (!GameManager.Instance.mute)
            _splashAudio.Play();

        _anim.Play("Bounce");

        perfectPasses = 0;
        _passingAudio.pitch = 1;

        if (isSuperSpeedActive)
            ToggleSuper();



        _isNextCollisionAllowed = false;
        Invoke(nameof(AllowNextCollision), 0.2f);

    }

    private void FixedUpdate()
    {
        /*

        if (_ballBounced)
        {
            if (!_ballSlowedDown && transform.position.y >= _yJumpStartPosition + _bounceHeight / 2)
            {
                _rb.velocity = _rb.velocity / 2f;
                _ballSlowedDown = true;
            }

            if (transform.position.y >= _yJumpStartPosition + _bounceHeight)
            {
                _rb.velocity = Vector3.zero;
                _rb.AddForce(Vector3.down * _bounceForce * 1.5f, ForceMode.Force);
                _ballBounced = false;
                _ballSlowedDown = false;
            }
        }
        */

        var pos = transform.position;

        if (jumpVelocity != 0)
        {
            pos.y += jumpVelocity;
            jumpVelocity -= _jumpDampening;

            if (jumpVelocity <= 0)
            {
                _rb.useGravity = true;
                jumpVelocity = 0;
            }
        }

        transform.position = pos;

    }

    private void SpawnSplash(GameObject parentPart)
    {
        //–азмер части на которую нужно заспавнить краску
        float parentPartSize = parentPart.GetComponent<Renderer>().bounds.size.y;
        // оордината y на которую нужно заспавнить краску
        float partTop = parentPart.transform.position.y + parentPartSize;



        float paintSize = _paintSplashPrefab.GetComponent<Renderer>().bounds.size.y;

        float ySpawnCoordinate = partTop + 0.00005f;

        Vector3 splashSpawnPosition = new Vector3(transform.position.x, ySpawnCoordinate, transform.position.z);

        var paintSplash = Instantiate(_paintSplashPrefab, splashSpawnPosition, Quaternion.identity);
        paintSplash.transform.SetParent(parentPart.transform, true);

        GameObject particle = Instantiate(_particleSystem, new Vector3(transform.position.x, ySpawnCoordinate, transform.position.z), Quaternion.identity);
        particle.transform.Rotate(-90, 270, 0);
        particle.transform.SetParent(transform);

        particle.GetComponent<ParticleSystem>().startColor = GameManager.Instance.ballColor;

        //_particleSystem.transform.position = new Vector3(transform.position.x, ySpawnCoordinate, transform.position.z);
        //var particle = Instantiate(_particleSystem, new Vector3(transform.position.x, ySpawnCoordinate, transform.position.z), Quaternion.identity);
        //particle.transform.SetParent(parentPart.transform, true);

        //_particleSystem.Play();


    }

    private void AllowNextCollision()
    {
        _isNextCollisionAllowed = true;
    }

    public void ToggleSuper()
    {
        if (isSuperSpeedActive == false)
        {
            _trail.gameObject.SetActive(false);
            _trailSuper.gameObject.SetActive(true);
            isSuperSpeedActive = true;
        }
        else
        {
            _trail.gameObject.SetActive(true);
            _trailSuper.gameObject.SetActive(false);
            isSuperSpeedActive = false;
        }
    }
}
