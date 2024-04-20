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

        // Giriþ baþarýlý olduðunda tetiklenecek olayý dinle
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != null)
        {
            // Kullanýcý oturum açtý, sahneyi deðiþtir
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
                Debug.LogError("Giriþ iþlemi baþarýsýz: " + task.Exception);
                return;
            }

            FirebaseUser user = task.Result.User;
            Debug.Log("Giriþ baþarýlý! Kullanýcý: " + user.DisplayName + " (" + user.Email + ")");
        });
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
}
