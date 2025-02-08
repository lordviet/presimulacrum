using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    AudioSource audioSource;

    private void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        StartCoroutine(FadeAndLoadScene("Level 1"));
    }

    public void QuitGame()
    {
        StartCoroutine(FadeAndQuit());
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeAndQuit()
    {
        yield return StartCoroutine(FadeOut());
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void FadeOutAudio()
    {
        StartCoroutine(FadeOutAudioCoroutine());
    }

    private IEnumerator FadeOutAudioCoroutine()
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newVolume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            audioSource.volume = newVolume;
            yield return null;
        }

        // Ensure the volume is set to 0 and stop the audio
        audioSource.volume = 0f;
        audioSource.Stop();
    }

    private IEnumerator FadeOut()
    {
        this.FadeOutAudio();
        fadeImage.gameObject.SetActive(true);
        float elapsedTime = 0f;
        Color startColor = new Color(0, 0, 0, 0);
        Color endColor = new Color(0, 0, 0, 1);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }
    }
}