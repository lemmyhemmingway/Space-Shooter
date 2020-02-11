using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;

    private float _speedMulti = 2.0f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private float _fireRate = 0.15f;

    private float _canFire = -0.5f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _manager;
    private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shield;

    private int _score;

    private UI_Manager _uiManager;

    private void Start()
    {
        //player position
        transform.position = new Vector3(0, 0, 0);
        _manager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();

        if (_manager == null)
        {
            Debug.LogError("Mananger NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI MANAGER NULL");
        }
    }

    private void Update()
    {
        calculateMovement();
        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    private void calculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float yLimitMin = -3.5f;
        float yLimitMax = 5.5f;

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        // transform.Translate(_speed * horizontalInput * Vector3.right * Time.deltaTime);
        // transform.Translate(_speed * verticalInput * Vector3.up * Time.deltaTime);

        transform.Translate(direction * _speed * Time.deltaTime);
        transform.position = new Vector3(
            transform.position.x,
            Mathf.Clamp(transform.position.y, yLimitMin, yLimitMax),
            0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shield.SetActive(false);
            return;
        }
        _lives -= 1;

        _uiManager.UpdateLives(_lives);
        if (_lives < 1)
        {
            _manager.PlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedActive()
    {
        _speed *= _speedMulti;
        StartCoroutine(SpeedActiveRoutine());
    }

    private IEnumerator SpeedActiveRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMulti;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
    }

    public void IncreaseScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}