/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: For the GameManager prefab. Performs a variety of tasks, such as updating UI elements, performing player respawns, and the GameOver sequence.
-----------------------------------------*/

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] public SceneField gameOverScene;
    [SerializeField] GameObject menuCanvas;
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject healthPanel;
    [SerializeField] GameObject moneyCounter;
    [SerializeField] GameObject[] hearts;
    Image[] emptyHearts;
    Image[] fullHearts;
    public bool heartContainerCollected = false;

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
        CheckPlayerBehindUI();
    }

    public void EnablePlayerInput()
    {
        PlayerController.instance.pInput.Enable();
        InventoryMenuScript.instance.iInput.Enable();
    }

    public void DisablePlayerInput()
    {
        PlayerController.instance.pInput.Disable();
        InventoryMenuScript.instance.iInput.Disable();
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
        int fullHeartsCount = PlayerController.instance.health / 4;
        float partialHeart = (float)(PlayerController.instance.health % 4) / 4;
        for (int i = 0; i < hearts.Length; i++)
        {
            emptyHearts[i].color = maxHeartsCount < i + 1 ? Color.clear : Color.white;
            fullHearts[i].color = fullHeartsCount < i ? Color.clear : Color.white;
            if (i == fullHeartsCount)
                fullHearts[i].fillAmount = partialHeart;
            else
                fullHearts[i].fillAmount = 1f;
        }

        if (maxHeartsCount <= 5)
        {
            moneyCounter.GetComponent<HorizontalLayoutGroup>().padding.top = -30;
        }
        else
            moneyCounter.GetComponent<HorizontalLayoutGroup>().padding.top = 0;
    }

    public void UpdatePlayerMoney()
    {
        moneyCounter.GetComponentInChildren<TextMeshProUGUI>().SetText(PlayerController.instance.money.ToString("000"));
    }

    void CheckPlayerBehindUI()
    {
        if (IsPointInRectTransform(PlayerController.instance.transform.position, healthPanel.GetComponent<RectTransform>(), Camera.main))
        {
            foreach (Image i in healthPanel.GetComponentsInChildren<Image>())
            {
                if (i.color.a > 0f) i.color = new Color(i.color.r, i.color.g, i.color.b, 0.5f);
            }
            foreach (TextMeshProUGUI t in healthPanel.GetComponentsInChildren<TextMeshProUGUI>())
            {
                if (t.color.a > 0f) t.color = new Color(t.color.r, t.color.g, t.color.b, 0.5f);
            }
            //Debug.Log("helloo");
        }
        else
        {
            foreach (Image i in healthPanel.GetComponentsInChildren<Image>())
            {
                if (i.color.a > 0f) i.color = new Color(i.color.r, i.color.g, i.color.b, 1f);
            }
            foreach (TextMeshProUGUI t in healthPanel.GetComponentsInChildren<TextMeshProUGUI>())
            {
                if (t.color.a > 0f) t.color = new Color(t.color.r, t.color.g, t.color.b, 1f);
            }
        }
    }

    bool IsPointInRectTransform(Vector2 point, RectTransform rt, Camera cam)
    {
        int screenWidth = cam.scaledPixelWidth;
        int screenHeight = cam.scaledPixelHeight;

        float leftSide = cam.ScreenToWorldPoint(rt.anchorMin * screenWidth).x;
        float rightSide = cam.ScreenToWorldPoint(rt.anchorMax * screenWidth).x;
        float topSide = cam.ScreenToWorldPoint(rt.anchorMax * screenHeight).y;
        float bottomSide = cam.ScreenToWorldPoint(rt.anchorMin * screenHeight).y;

        //Debug.Log(leftSide + ", " + rightSide + ", " + topSide + ", " + bottomSide);

        // Check to see if the point is in the calculated bounds
        if (point.x >= leftSide &&
            point.x <= rightSide &&
            point.y >= bottomSide &&
            point.y <= topSide)
        {
            return true;
        }
        return false;
    }

    public void RespawnAtCheckpoint(int damageToInflict = 0)
    {
        PlayerController.instance.DecreaseHealth(damageToInflict);
        PlayerController.instance.transform.position = CheckpointScript.instance.transform.position;
        PlayerController.instance.GetComponent<Collider2D>().enabled = true;
    }

    public void RespawnAtRespawnPoint()
    {
        SceneManagerScript.SwapScene(RespawnPointScript.instance.respawnSceneIndex);
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        while (SceneFadeManager.instance.isFadingOut)
        {
            yield return null;
        }
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
        PlayerController.instance.animator.SetBool("IsDying", true);
        //SceneManagerScript.SwapScene(gameOverScene);
    }
}
