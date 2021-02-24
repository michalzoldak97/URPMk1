using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;

namespace U1
{
    public class WebSignUp : MonoBehaviour
    {
        [SerializeField] private string signUpPHPurl;
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private Button submitButton;
        [SerializeField] private TMP_Text validationInfoText;
        [SerializeField] private GameObject infoImage;
        [SerializeField] private int minNameLength, minPasswordLength;
        private bool isSignUpInProgress;
        private MenuManager menuManager;

        private void Start()
        {
            menuManager = GetComponent<MenuManager>();
            VerifyInputs();
        }

        public void CallSignUp()
        {
            if(!menuManager.GetSceneManager().isLoggedIn && !isSignUpInProgress && menuManager.GetSceneManager().signUpAttempts <= 3)
                StartCoroutine(SignUp());
            else
            {
                StartCoroutine(InformCantAttempt("Can not allow sign up, please restart an application"));
            }
        }
        private IEnumerator InformCantAttempt(string infoText)
        {
            infoImage.SetActive(true);
            infoImage.GetComponent<Image>().color = Color.red;
            infoImage.GetComponentInChildren<TMP_Text>().text = infoText;
            yield return new WaitForSeconds(2);
            infoImage.SetActive(false);
        }

        private IEnumerator SignUp()
        {
            isSignUpInProgress = true;
            menuManager.GetSceneManager().signUpAttempts++;
            WWWForm wFrom = new WWWForm();
            wFrom.AddField("username", nameInputField.text);
            wFrom.AddField("password", passwordInputField.text);
            using (UnityWebRequest webRequest = UnityWebRequest.Post(signUpPHPurl, wFrom)) 
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log(": Error: " + webRequest.error);
                }
                if (webRequest.downloadHandler.text == "1")
                {
                    StartCoroutine(SignUpSucces());
                    Debug.Log("User register SUCESS");
                }
                else
                {
                    StartCoroutine(InformCantAttempt("Error: " + webRequest.error + " " + webRequest.downloadHandler.text));
                }
            }
            isSignUpInProgress = false;
        }
        private IEnumerator SignUpSucces()
        {
            infoImage.SetActive(true);
            infoImage.GetComponent<Image>().color = Color.green;
            infoImage.GetComponentInChildren<TMP_Text>().text = "Player account created successfully";
            yield return new WaitForSeconds(2);
            infoImage.SetActive(false);
            yield return new WaitForSeconds(1);
            nameInputField.text = "";
            passwordInputField.text = "";
            menuManager.DeactivateSignUpPanel();
        }

        public void VerifyInputs()
        {
            submitButton.interactable = (CheckInputFields());
        }
        private bool CheckInputFields()
        {
            if (nameInputField.text.Length < minNameLength || passwordInputField.text.Length < minPasswordLength)
            {
                validationInfoText.text = "Name should be at least " + minNameLength + " characters long and password should be at least " + minPasswordLength + " characters long";
                return false;
            }
            else if (!Regex.Match(nameInputField.text, "^[A-Za-z0-9]*$").Success || !Regex.Match(passwordInputField.text, "^[A-Za-z0-9_]*$").Success)
            {
                validationInfoText.text = "Name and password should contain only: lowercase and uppercase letters and/or numbers and/or dashes";
                return false;
            }
            else if (nameInputField.text.Length > minPasswordLength*2 || passwordInputField.text.Length > minPasswordLength*2)
            {
                validationInfoText.text = "Name should be no longer than " + minPasswordLength*2 + " and password should be no longer than " + minPasswordLength*2 + " characters";
                return false;
            }
            else
            {
                validationInfoText.text = "";
                return true;
            }
        }
    }
}
