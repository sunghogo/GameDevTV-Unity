using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.LowLevel;

public class Movement : MonoBehaviour
{
    // PARAMTERS - for tuning, typically set in the editor;

    // CACHE - e.g. references for readability or speed;

    // STATE - privvate instance (member) variables
    [SerializeField] private float _mainThrust;
    [SerializeField] private float _rotationThrust;
    [SerializeField] private AudioClip _mainEngineAudio;

    [SerializeField] private ParticleSystem _mainEngineParticles;
    [SerializeField] private ParticleSystem _rightEngineParticles;
    [SerializeField] private ParticleSystem _leftEngineParticles;


    private Rigidbody _rigidBody;
    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _rigidBody.drag = 0.25f;
        _mainThrust = 1000f;
        _rotationThrust = 100f;

        // Constraints
        _rigidBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput() {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust() {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrust();
        }
        else
        {
            StopThrust();
        }
    }

    private void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            StartRotateLeft();
        }
        else StopRotateLeft();

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            StartRotateRight();
        }
        else StopRotateRight();
    }


    private void RotateLeft() {
        ApplyRotation(_rotationThrust);
    }

    private void RotateRight() {
        ApplyRotation(-_rotationThrust);
    }

    private void StartRotateRight()
    {
        RotateRight();
        if (!_leftEngineParticles.isPlaying) _leftEngineParticles.Play();
    }

    private void StopRotateRight()
    {
        _leftEngineParticles.Stop();
    }


    private void StartRotateLeft()
    {
        RotateLeft();
        if (!_rightEngineParticles.isPlaying) _rightEngineParticles.Play();
    }

    private void StopRotateLeft()
    {
        _rightEngineParticles.Stop();
    }

    private void ApplyRotation(float rotationThrust)
    {
        _rigidBody.freezeRotation = true;
        transform.Rotate(UnityEngine.Vector3.forward * rotationThrust * Time.deltaTime);
        _rigidBody.freezeRotation = false;
    }

    private void ApplyThrust() {
        _rigidBody.AddRelativeForce(UnityEngine.Vector3.up * _mainThrust * Time.deltaTime);

    }
    
    private void StopThrust()
    {
        _audioSource.Stop();
        _mainEngineParticles.Stop();
    }

    private void StartThrust()
    {
        ApplyThrust();
        if (!_audioSource.isPlaying) _audioSource.PlayOneShot(_mainEngineAudio);
        if (!_mainEngineParticles.isPlaying) _mainEngineParticles.Play();
    }

    public void StopScript() {
        this.enabled = false;
    }
}
