using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : Interactable
{
    [SerializeField] Image DialogBG;
    [SerializeField] Text DialogText;
    [SerializeField] float TransitionSpeed;
    [SerializeField] string[] dialogs;
    int currentDialogIndex = 0;
    Color DialogTextColor;
    Color DialogBGColor;
    float Opacity;
    Coroutine TransistionCoroutine;
    // Start is called before the first frame update


    void GoToNextDialog()
    {
        if( dialogs.Length == 0)
        {
            return;
        }
        currentDialogIndex = (currentDialogIndex + 1) % dialogs.Length;
        DialogText.text = dialogs[currentDialogIndex];
    }



    void Start()
    {
        DialogTextColor = DialogText.color;
        DialogBGColor = DialogBG.color;
        SetOpacity(0);
        if(dialogs.Length != 0)
        {
            DialogText.text = dialogs[0];
        }
        else
        {
            DialogText.text = "";
        }
    }


    void SetOpacity(float opacity)
    {
        opacity = Mathf.Clamp(opacity, 0, 1);
        Color ColorMult = new Color(1f, 1f, 1f, opacity);
        DialogText.color =DialogTextColor * ColorMult;
        DialogBG.color = DialogBGColor * ColorMult;
        Opacity = opacity;
    }

    IEnumerator TranstionOpacityTo(float newOpacity)
    {
        float Dir = newOpacity - Opacity > 0 ? 1 : -1;
        while(Opacity != newOpacity)
        {
            SetOpacity(Opacity + Dir * TransitionSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        SetOpacity(newOpacity);
    }
    private void OnTriggerEnter(Collider other)
    {
        currentDialogIndex = 0;
        DialogText.text = dialogs[currentDialogIndex];
        InteractComponent interactComp = other.GetComponent<InteractComponent>();
        if(interactComp != null)
        {
            if(TransistionCoroutine != null)
            {
                StopCoroutine(TransistionCoroutine);
                TransistionCoroutine = null;
            }
            TransistionCoroutine = StartCoroutine(TranstionOpacityTo(1f));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractComponent interactComp = other.GetComponent<InteractComponent>();
        if (interactComp != null)
        {
            StartCoroutine(TranstionOpacityTo(0f));
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        if (DialogText.text == dialogs[2])
        {
            StartCoroutine(TranstionOpacityTo(0));
            return;
        }
        GoToNextDialog();
    }
}
