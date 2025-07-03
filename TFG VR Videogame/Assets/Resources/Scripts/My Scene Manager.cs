using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Mode
{
    Menu,
    Arcade,
    Survival,
}

public class MySceneManager : MonoBehaviour
{
    [Header("Mode Settings")]
    public Mode currentMode;

    [Header("Fade Settings")]
    [Tooltip("Assign the Renderer of the quad used for fading.")]
    public Renderer fadeQuad;
    [Tooltip("How long the fade in/out takes.")]
    public float fadeDuration = 1f;
    public bool autoSceneChange = false;

    private Material fadeMaterial;
    private Color fadeColor;

    public bool isStarted = false;

    // Scene auto changer
    private float autoChangeInterval = 10f;
    private float timer = 0f;

    private void Awake()
    {
        fadeMaterial = fadeQuad.material;
        fadeColor = fadeMaterial.color;

        fadeColor.a = 0f;
        fadeMaterial.color = fadeColor;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentMode = Mode.Menu;
    }

    void Update()
    {
        if (autoSceneChange)
        {
            timer += Time.unscaledDeltaTime;
            if (timer >= autoChangeInterval)
            {
                timer = 0f;
                // Toggle between scene index 1 and 2
                int currentIndex = SceneManager.GetActiveScene().buildIndex;
                int nextIndex = (currentIndex == 1) ? 2 : 1;
                FadeToScene(nextIndex);
            }
        }
    }

    public void ChangeToArcade()
    {
        if (currentMode != Mode.Arcade)
        {
            currentMode = Mode.Arcade;
            //Change Scene to arcade
            FadeToScene(1);
        }
    }
    public void ChangeToSurvival()
    {
        if (currentMode != Mode.Survival)
        {
            currentMode = Mode.Survival;
            //Change Scene to Survival
            FadeToScene(2);
        }
    }

    public void FadeToScene(int sceneIndex)
    {
        StartCoroutine(DoSceneFade(sceneIndex));
    }

    private IEnumerator DoSceneFade(int sceneIndex)
    {
        yield return StartCoroutine(Fade(0f, 1f));

        var loadOp = SceneManager.LoadSceneAsync(sceneIndex);
        while (!loadOp.isDone)
            yield return null;

        yield return StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, t);

            fadeColor.a = currentAlpha;
            fadeMaterial.color = fadeColor;

            yield return null;
        }
        fadeColor.a = endAlpha;
        fadeMaterial.color = fadeColor;
    }
}


