using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private Canvas endingCanvas;
    [SerializeField] private float quoteDelay = 3f;
    [SerializeField] private float quoteDisplayDuration = 5f;

    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip successAudio;
    [SerializeField] AudioClip successDistortedAudio;
    [SerializeField] AudioClip crashAudio;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem darkParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;

    bool isControllable = true;
    bool isCollidable = true;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    private void Update()
    {
        // NOTE: Uncomment this line to switch debug mode
        // this.RespondToDebugKeys();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isControllable || !isCollidable)
        {
            return;
        }

        switch (other.gameObject.tag)
        {
            case "Finish":
                this.StartLoadNextScene();
                break;

            case "Friendly":
                Debug.Log("Level begin!");
                break;

            default:
                this.StartCrashSequence();
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isControllable || !isCollidable)
        {
            return;
        }

        switch (other.gameObject.tag)
        {
            case "Finish":
                this.StartLoadEnding();
                break;
        }
    }

    private void StartLoadEnding()
    {
        isControllable = false;

        this.audioSource.Stop();
        this.audioSource.PlayOneShot(this.successDistortedAudio);

        this.darkParticles.Play();

        this.GetComponent<Movement>().enabled = false;
        this.GetComponent<Oscillator>().enabled = true;
        this.GetComponent<Rigidbody>().useGravity = false;

        StartCoroutine(ShowEndingQuoteAndLoadMainMenu());
    }

    private IEnumerator ShowEndingQuoteAndLoadMainMenu()
    {
        // Wait for the initial delay
        yield return new WaitForSeconds(quoteDelay);

        // Show the ending canvas and set the quote text
        endingCanvas.gameObject.SetActive(true);

        // Wait for the quote display duration
        yield return new WaitForSeconds(quoteDisplayDuration);

        // Load the main menu
        SceneManager.LoadScene("Main Menu");
    }

    private void StartLoadNextScene()
    {
        isControllable = false;

        this.audioSource.Stop();
        this.audioSource.PlayOneShot(this.successAudio);

        this.successParticles.Play();

        this.GetComponent<Movement>().enabled = false;

        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void StartCrashSequence()
    {
        isControllable = false;

        this.audioSource.Stop();
        this.audioSource.PlayOneShot(this.crashAudio);

        this.crashParticles.Play();

        this.GetComponent<Movement>().enabled = false;
        Invoke("ReloadScene", levelLoadDelay);
    }

    private void ReloadScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    private void LoadNextScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        // If we are on the last scene and begin a new game loop restart at the first level
        int nextScene = currentScene == (SceneManager.sceneCountInBuildSettings - 1) ? 1 : currentScene + 1;

        SceneManager.LoadScene(nextScene);
    }

    private void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            this.LoadNextScene();
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            this.isCollidable = !this.isCollidable;

            Debug.Log($"Collision enabled: {this.isCollidable}");
        }
    }
}
