using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using UnityEngine.UI;
using System;
using Firebase.Extensions;
using System.Threading.Tasks;
using static UnityEditor.Timeline.TimelinePlaybackControls;


public class AuthManager : MonoBehaviour
{
    FirebaseAuth auth;

    public InputField emailInput;
    public InputField passwordInput;
    public Button loginButton;
    public Button signupButton;
    public string scene;
    public ErrorDialog errorDialog;
    private string logText = "";
    const int kMaxLogSize = 16382;
    private Vector2 scrollViewVector = Vector2.zero;


    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                if (auth.CurrentUser != null)
                {
                    auth.SignOut();
                }
                loginButton.onClick.AddListener(SigninWithEmailAsync);
                signupButton.onClick.AddListener(Signup);
                auth.StateChanged += AuthStateChanged;
                AuthStateChanged(this, null);
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });  
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != null)
        {
            // Kullanýcý oturum açtý, sahneyi deðiþtir
            SceneManager.LoadScene(scene);
        }
    }


 

    public void SigninWithEmailAsync()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        auth.SignInWithEmailAndPasswordAsync(email + "@gmail.com", password)
          .ContinueWithOnMainThread(HandleSignInWithAuthResult);
       
    }

    void HandleSignInWithAuthResult(Task<Firebase.Auth.AuthResult> task)
    {
     
        if (LogTaskCompletion(task, "Sign-in"))
        {
            if (task.Result.User != null && task.Result.User.IsValid())
            {
                
                DebugLog(String.Format("{0} signed in", task.Result.User.DisplayName));
            }
            else
            {
                DebugLog("Signed in but User is either null or invalid");
            }
        }
    }

   
    protected bool LogTaskCompletion(Task task, string operation)
    {
        bool complete = false;
        if (task.IsCanceled)
        {
            DebugLog(operation + " canceled.");
        }
        else if (task.IsFaulted)
        {
            DebugLog(operation + " encounted an error.");
            foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
            {
                string authErrorCode = "";
                Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                if (firebaseEx != null)
                {
                    authErrorCode = String.Format("AuthError.{0}: ",
                      ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
                }
                errorDialog.ShowErrorMessage("Giriþ baþarýsýz. Lütfen kullanýcý adýnýzý, þifrenizi ve internet baðlantýnýzý kontrol edin.");
                DebugLog(authErrorCode + exception.ToString());
            }
        }
        else if (task.IsCompleted)
        {
            DebugLog(operation + " completed");
            complete = true;
        }
        return complete;
    }

    void Signup()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        auth.CreateUserWithEmailAndPasswordAsync(email + "@gmail.com", password).ContinueWith(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Kayýt iþlemi baþarýsýz: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.Log("Yeni kullanýcý oluþturuldu! Kullanýcý: " + newUser.DisplayName + " (" + newUser.Email + ")");
        });
    }
    public void DebugLog(string s)
    {
        Debug.Log(s);
        logText += s + "\n";

        while (logText.Length > kMaxLogSize)
        {
            int index = logText.IndexOf("\n");
            logText = logText.Substring(index + 1);
        }
        scrollViewVector.y = int.MaxValue;
    }
   

}
