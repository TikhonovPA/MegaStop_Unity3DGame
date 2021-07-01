using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
    public Sprite btn, btnpress, musicOn, musicOff;
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();

        if (gameObject.name == "Music Button")
        {
            if (PlayerPrefs.GetString("music") == "No")
                transform.GetChild(0).GetComponent<Image>().sprite = musicOff;

        }
    }

    public void PlayGame()
    {
        PlayButtonSound();
        StartCoroutine(LoadScene("Game"));
    }

    public void MusicButton()
    {
        PlayButtonSound();
        if (PlayerPrefs.GetString("music") == "No")
        {
            PlayerPrefs.SetString("music", "Yes");
            transform.GetChild(0).GetComponent<Image>().sprite = musicOn;
        }
        else
        {
            PlayerPrefs.SetString("music", "No");
            transform.GetChild(0).GetComponent<Image>().sprite = musicOff;
        }
    }

    private void PlayButtonSound()
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();
    }

    public void ShopScene()
    {
        PlayButtonSound();
        StartCoroutine(LoadScene("Shop"));
    }

    public void ExitShopScene()
    {
        PlayButtonSound();
        StartCoroutine(LoadScene("Main"));
    }

    public void OpenInsta()
    {
        PlayButtonSound();
        Application.OpenURL("https://www.youtube.com/");
    }

    public void SetPressedButton()
    {
        _image.sprite = btnpress;
        transform.GetChild(0).localPosition -= new Vector3(0, 7f, 0);
    }

    public void SetDefaultButton()
    {
        _image.sprite = btn;
        transform.GetChild(0).localPosition += new Vector3(0, 7f, 0);
    }

    IEnumerator LoadScene(string name)
    {
        float fadeTime = Camera.main.GetComponent<Fading>().Fade(1f);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(name);
    }
}
