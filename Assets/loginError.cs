using UnityEngine;
using UnityEngine.UI;

public class ErrorDialog : MonoBehaviour
{
    public Text errorMessageText;
    public Button retryButton;

    private void Start()
    {
   
        retryButton.onClick.AddListener(Retry);
    }


    public void ShowErrorMessage(string errorMessage)
    {
  
        gameObject.SetActive(true);


        errorMessageText.text = errorMessage;
    }


    private void Retry()
    {

        HideErrorMessage();
    }


    private void HideErrorMessage()
    {
   
        gameObject.SetActive(false);
    }
}
