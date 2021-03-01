using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class MainSceneMenuControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public AudioClip clickSound;
    public Sprite menuDowned;
    public Sprite menuUpped;
    public CommonDataSettings commonData;

    private AudioSource playerAudio;

    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        playerAudio.PlayOneShot(clickSound, 1.0f);
        GetComponent<Image>().sprite = menuDowned;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = menuUpped;
        commonData.toMenu = true;
    }
}
