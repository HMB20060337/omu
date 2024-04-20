using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    FirebaseAuth auth;

    public InputField emailInput;
    public InputField passwordInput;
    public Button loginButton;
    public Button signupButton;
    public string scene;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.SignOut();
        loginButton.onClick.AddListener(Login);
        signupButton.onClick.AddListener(Signup);

        // Giri� ba�ar�l� oldu�unda tetiklenecek olay� dinle
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != null)
        {
            // Kullan�c� oturum a�t�, sahneyi de�i�tir
            SceneManager.LoadScene(scene);
        }
    }

    void Login()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        auth.SignInWithEmailAndPasswordAsync(email + "@gmail.com", password).ContinueWith(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Giri� i�lemi ba�ar�s�z: " + task.Exception);
                return;
            }

            FirebaseUser user = task.Result.User;
            Debug.Log("Giri� ba�ar�l�! Kullan�c�: " + user.DisplayName + " (" + user.Email + ")");
        });
    }

    void Signup()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        auth.CreateUserWithEmailAndPasswordAsync(email + "@gmail.com", password).ContinueWith(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Kay�t i�lemi ba�ar�s�z: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.Log("Yeni kullan�c� olu�turuldu! Kullan�c�: " + newUser.DisplayName + " (" + newUser.Email + ")");
        });
    }
}
