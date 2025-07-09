using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene
{
    FirstMenu,
    Menu,
    Arcade,
    Survival,
}

public class MySceneManager : MonoBehaviour
{
    [Header("Mode Settings")]
    public Scene currentScene;

    [Header("Fade Settings")]
    [Tooltip("Assign the Renderer of the quad used for fading.")]
    public Renderer fadeQuad;
    [Tooltip("How long the fade in/out takes.")]
    public float fadeDuration = 1f;
    public bool autoSceneChange = false;

    private Material fadeMaterial;
    private Color fadeColor;

    public bool isStarted = false;

    // Mace Vars
    private GameObject maceGO;
    //private StickyGrabInteractable maceStickyGrab;
    public List<Vector3> macePositionList = new List<Vector3>();

    // Player Rig Vars
    private GameObject playerRigGO;
    public List<Vector3> playerPositionList = new List<Vector3>();

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

        maceGO = GameObject.FindWithTag("Mace");
        if (maceGO != null)
        {
            //maceStickyGrab = maceGO.GetComponent<StickyGrabInteractable>();
        }
        playerRigGO = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        currentScene = Scene.FirstMenu;
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
        if (currentScene != Scene.Arcade)
        {
            currentScene = Scene.Arcade;

            //Change Scene to arcade
            FadeToScene((int)currentScene);

            // Release Mace Grab and Change the spawn positions for the new Scene
            //maceStickyGrab.ReleaseStickyGrab();
            maceGO.transform.position = macePositionList[(int)currentScene];

            playerRigGO.transform.position = playerPositionList[(int)currentScene];
        }
    }
    public void ChangeToSurvival()
    {
        if (currentScene != Scene.Survival)
        {
            currentScene = Scene.Survival;

            //Change Scene to Survival
            FadeToScene((int)currentScene);

            // Release Mace Grab and Change the spawn positions for the new Scene
            //maceStickyGrab.ReleaseStickyGrab();
            maceGO.transform.position = macePositionList[(int)currentScene];

            playerRigGO.transform.position = playerPositionList[(int)currentScene];
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


