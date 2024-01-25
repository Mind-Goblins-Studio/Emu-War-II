using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGameScreenManager : MonoBehaviour
{
    [SerializeField] private Fading button;
    [SerializeField] private GameObject uteIcon;
    
    // Start is called before the first frame update
    void Start()
    {
        // Uncover Button & Ute after 3 sec
        Invoke("UncoverButton", 1.2f);
        Invoke("UncoverUte", 1.3f);
    }

    
    private void UncoverButton()
    {
        button.FadeIn();
    }
    
    private void UncoverUte()
    {
        uteIcon.GetComponent<Fading>().FadeIn();
    }
}
