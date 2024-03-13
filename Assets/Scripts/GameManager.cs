/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: For the GameManager prefab. Performs a variety of tasks, such as updating UI elements, performing player respawns, and the GameOver sequence.
-----------------------------------------*/

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] SceneField gameOverScene;
    [SerializeField] GameObject menuCanvas;
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject[] hearts;
    Image[] emptyHearts;
    Image[] fullHearts;

    void Awake()
    {
        //---------- Make this script a singleton ----------//
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        //--------------------------------------------------//
    }

    void Start()
    {
        UpdatePlayerHearts();
    }

    void Update()
    {
        if (!PlayerController.instance.gameObject.activeSelf && (menuCanvas.activeSelf || mainCanvas.activeSelf))
        {
            menuCanvas.SetActive(false);
            mainCanvas.SetActive(false);
        }
        else if (PlayerController.instance.gameObject.activeSelf)
        {
            menuCanvas.SetActive(true);
            mainCanvas.SetActive(true);
        }
        UpdatePlayerHearts();
    }

    void InitializePlayerHearts()
    {
        emptyHearts = new Image[10];
        fullHearts = new Image[10];
        for (int i = 0; i < hearts.Length; i++)
        {
            foreach (Image img in hearts[i].GetComponentsInChildren<Image>())
            {
                if (img.type == Image.Type.Filled)
                    fullHearts[i] = img;
                else
                    emptyHearts[i] = img;
            }
        }
    }

    public void UpdatePlayerHearts()
    {
        if (emptyHearts == null)
            InitializePlayerHearts();

        int maxHeartsCount = PlayerController.instance.maxHealth / 4;
        Debug.Log(PlayerController.instance.maxHealth);
        int fullHeartsCount = PlayerController.instance.health / 4;
        Debug.Log(PlayerController.instance.health);
        float partialHeart = (float)(PlayerController.instance.health % 4) / 4;
        //Debug.Log(partialHeart);
        for (int i = 0; i < hearts.Length; i++)
        {
            emptyHearts[i].color = maxHeartsCount < i + 1 ? Color.clear : Color.white;
            fullHearts[i].color = fullHeartsCount < i ? Color.clear : Color.white;
            if (i == fullHeartsCount)
                fullHearts[i].fillAmount = partialHeart;
            else
                fullHearts[i].fillAmount = 1f;
        }
    }

    public void RespawnAtCheckpoint(int damageToInflict = 0)
    {
        PlayerController.instance.DecreaseHealth(damageToInflict);
        PlayerController.instance.transform.position = CheckpointScript.instance.transform.position;
    }

    public void RespawnAtRespawnPoint()
    {
        SceneManagerScript.SwapScene(RespawnPointScript.instance.respawnSceneIndex);
        PlayerController.instance.gameObject.SetActive(true);
        PlayerController.instance.HealthToMax();
        UpdatePlayerHearts();
        PlayerController.instance.transform.position = RespawnPointScript.instance.transform.position;
    }

    public void GameOverSequence()
    {
        PlayerController.instance.pInput.Disable();
        PlayerController.instance.EndPlayerCoroutines();
        RespawnPointScript.instance.respawnSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManagerScript.SwapScene(gameOverScene);
    }
}
