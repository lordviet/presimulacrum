using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;

    [SerializeField] float thrustStrength = 100f;
    [SerializeField] float rotationStrength = 100f;

    [SerializeField] AudioClip mainBoosterAudio;

    [SerializeField] ParticleSystem mainBoosterParticles;
    [SerializeField] ParticleSystem leftBoosterParticles;
    [SerializeField] ParticleSystem rightBoosterParticles;

    Rigidbody rb;
    AudioSource audioSource;

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void OnDisable()
    {
        if (mainBoosterParticles.isPlaying)
        {
            mainBoosterParticles.Stop();
        }

        if (leftBoosterParticles.isPlaying)
        {
            leftBoosterParticles.Stop();
        }

        if (rightBoosterParticles.isPlaying)
        {
            rightBoosterParticles.Stop();
        }
    }

    private void Start()
    {
        this.rb = this.GetComponent<Rigidbody>();
        this.audioSource = this.GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        this.ProcessThrust();
        this.ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            this.StartThrusting();
            return;
        }

        this.StopThrusting();
    }

    private void StartThrusting()
    {
        Vector3 force = Vector3.up * thrustStrength * Time.fixedDeltaTime;

        if (!audioSource.isPlaying)
        {
            this.audioSource.PlayOneShot(this.mainBoosterAudio);
        }

        if (!mainBoosterParticles.isPlaying)
        {
            this.mainBoosterParticles.Play();
        }

        this.rb.AddRelativeForce(force);
    }

    private void StopThrusting()
    {
        this.audioSource.Stop();
        this.mainBoosterParticles.Stop();
    }

    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();

        if (rotationInput != 0)
        {
            this.StartRotating(rotationInput);
            return;
        }

        this.StopRotating();
    }

    private void StartRotating(float rotationInput)
    {
        this.HandleRotationParticles(rotationInput);

        Vector3 force = Vector3.forward * rotationStrength * -rotationInput * Time.fixedDeltaTime;

        rb.freezeRotation = true;

        transform.Rotate(force);

        rb.freezeRotation = false;

        return;
    }

    private void StopRotating()
    {
        this.leftBoosterParticles.Stop();
        this.rightBoosterParticles.Stop();
    }

    private void HandleRotationParticles(float rotationInput)
    {
        if (rotationInput == 0)
        {
            return;
        }
        else if (rotationInput > 0)
        {
            if (!this.leftBoosterParticles.isPlaying)
            {
                this.leftBoosterParticles.Play();
            }
        }
        else
        {
            if (!this.rightBoosterParticles.isPlaying)
            {
                this.rightBoosterParticles.Play();
            }
        }
    }
}
