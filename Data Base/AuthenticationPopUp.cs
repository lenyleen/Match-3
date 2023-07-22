using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationPopUp : MonoBehaviour
{
     [SerializeField] private TextMeshProUGUI StateInfo;
     [SerializeField] private TMP_InputField loginField;
     [SerializeField] private TMP_InputField passwordField;
     [SerializeField] private TMP_InputField imageURLField;
     [SerializeField] private Button signInButton;
     [SerializeField] private Button registrationButton;
     [SerializeField] private Toggle showPassword;
     [SerializeField] private Sprite passwordShown;
     [SerializeField]private Sprite passwordHided;
     [SerializeField] private Image avatar;
     private FirebaseAuth auth;
     
     private void Start()
     {
          auth = FirebaseAuth.DefaultInstance;
          SwitchToSignInState();
          passwordField.inputType = TMP_InputField.InputType.Password;
          showPassword.onValueChanged.AddListener(ShowPassword);
     }

     private void ShowPassword(bool show)
     {
          if (show)
          {
               passwordField.inputType = TMP_InputField.InputType.Standard;
               showPassword.image.sprite = passwordShown;
               return;
          }
          passwordField.inputType = TMP_InputField.InputType.Password;
          showPassword.image.sprite = passwordHided;
     }
     

     private void SwitchToSignInState()
     {
          imageURLField.gameObject.SetActive(false);
          StateInfo.text = "SIGNING IN";
          signInButton.gameObject.SetActive(true);
          signInButton.onClick?.RemoveListener(SwitchToSignInState);
          signInButton.onClick.AddListener(LogIn);
          registrationButton.onClick.AddListener(SwitchToRegistrationState);
     }
     private void SwitchToRegistrationState()
     {
          loginField.gameObject.SetActive(true);
          imageURLField.gameObject.SetActive(true);
          StateInfo.text = "REGISTRATION";
          signInButton.onClick?.RemoveListener(LogIn);
          signInButton.onClick.AddListener(SwitchToSignInState);
          registrationButton.onClick.RemoveListener(SwitchToRegistrationState);
          registrationButton.onClick.AddListener(RegisterNewPlayer);
     }
     private void LogIn()
     {
          var email = loginField.text + "@gm.com";
          auth.SignInWithEmailAndPasswordAsync(email, passwordField.text).ContinueWith(task => {
               if (task.IsCanceled) {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
               }
               if (task.IsFaulted) {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
               }

               Firebase.Auth.FirebaseUser newUser = task.Result;
               Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
          });
     }
     private void RegisterNewPlayer()
     {
          var email = loginField.text + "@gm.com";
          auth.CreateUserWithEmailAndPasswordAsync(email, passwordField.text).ContinueWith(task =>
          {
               if (task.IsCanceled)
               {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
               }

               if (task.IsFaulted)
               {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
               }
               Firebase.Auth.FirebaseUser newUser= task.Result;
               
          });
          

     }
}
