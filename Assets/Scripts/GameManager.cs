using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance = null;

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                // FindObjectOfType() returns the first AManager object in the scene.
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }

            // If it is still null, create a new instance
            if (_instance == null)
            {
                var obj = new GameObject("AManager");
                _instance = obj.AddComponent<GameManager>();
            }

            return _instance;
        }
    }


    // Ensure that the instance is destroyed when the game is stopped in the editor.
    void OnApplicationQuit()
    {
        _instance = null;
    }
    #endregion

    public GameObject deathPanel;
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI messageText;

    public AudioSource coinSource;
    public AudioClip coinAudioClip;

    public float oilAmount;
    public float oilDecay;

    public GameObject[] coinObjects;
    private Animator _deathPanelAnim;
    private Animator _deathTextAnim;
    private Animator _messageTextAnim;

    private void Start()
    {
        _deathPanelAnim = deathPanel.GetComponent<Animator>();
        _deathTextAnim = deathText.GetComponent<Animator>();
        _messageTextAnim = messageText.GetComponent<Animator>();
        OnResetPanel();
    }

    private void Update()
    {
        oilAmount -= Time.deltaTime * oilDecay;

        if (oilAmount <= 0)
        {
            //Death by darkness
            messageText.text = "The Light is gone";
            StartCoroutine("OnPlayerDeath");
        }
    }

    public void OnResetPanel()
    {
        _deathPanelAnim.Play("Panel_FadeIn");
    }

    IEnumerator OnPlayerDeath()
    {
        _deathPanelAnim.Play("Panel_FadeOut");

        yield return new WaitForSeconds(0.5f);

        deathText.text = "Death";
        _deathTextAnim.Play("message_show");

        yield return new WaitForSeconds(1f);

        _messageTextAnim.Play("message_show");

        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(0);
    }

    IEnumerator OnPlayerWin()
    {
        _deathPanelAnim.Play("Panel_FadeOut");

        yield return new WaitForSeconds(0.5f);
        deathText.text = "Hope";
        _deathTextAnim.Play("message_show");

        yield return new WaitForSeconds(1f);

        messageText.text = "You are safe, for now...";
        _messageTextAnim.Play("message_show");

        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(0);
    }

    public void OnPlayerReturn(int coins)
    {
        coinSource.PlayOneShot(coinAudioClip);
        Debug.Log("Player returned with " + coins + " coins.");
        oilAmount += coins / 3.0f;
        oilAmount = Mathf.Clamp(oilAmount, 0f, 1f);
        for (int i = 0; i < coins; i++)
        {
            coinObjects[i].GetComponent<Animator>().Play("coin_show");
        }
        Invoke("InvDisableCoins", 3.0f);
    }

    public void InvDisableCoins()
    {
        foreach (var c in coinObjects)
        {
            if (c.transform.GetChild(0).gameObject.activeInHierarchy)
                c.GetComponent<Animator>().Play("coin_hide");
        }
    }
}
