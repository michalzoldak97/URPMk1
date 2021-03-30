using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
namespace U1
{
    public class WebLogIn : MonoBehaviour
    {
        [SerializeField] private string logInPHPurl;
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private Button logInButton;
        [SerializeField] private TMP_Text validationInfoText;
        [SerializeField] private GameObject infoImage;
        [SerializeField] private int minNameLength, minPasswordLength;
        private bool isLogInInProgress, isLogInSuccessProgress;
        private MenuManager menuManager;
        private void Start()
        {
            menuManager = GetComponent<MenuManager>();
            VerifyInputs();
        }
        public void VerifyInputs()
        {
            logInButton.interactable = (CheckInputFields());
        }
        private bool CheckInputFields()
        {
            if(isLogInInProgress || isLogInSuccessProgress)
            {
                validationInfoText.text = "";
                return false;
            }
            else if (nameInputField.text.Length < minNameLength || passwordInputField.text.Length < minPasswordLength)
            {
                validationInfoText.text = "Name should be at least " + minNameLength + " characters long and password should be at least " + minPasswordLength + " characters long";
                return false;
            }
            else if (!Regex.Match(nameInputField.text, "^[A-Za-z0-9_]*$").Success || !Regex.Match(passwordInputField.text, "^[A-Za-z0-9_]*$").Success)
            {
                validationInfoText.text = "Name and password should contain only: lowercase and uppercase letters and/or numbers and/or dashes";
                return false;
            }
            else if (nameInputField.text.Length > minPasswordLength * 2 || passwordInputField.text.Length > minPasswordLength * 2)
            {
                validationInfoText.text = "Name should be no longer than " + minPasswordLength * 2 + " and password should be no longer than " + minPasswordLength * 2 + " characters";
                return false;
            }
            else
            {
                validationInfoText.text = "";
                return true;
            }
        }
        public void CallLogInAttempt()
        {
            if (!menuManager.GetSceneManager().isLoggedIn && !isLogInInProgress && menuManager.GetSceneManager().logInAttempts <= 5)
                StartCoroutine(LogIn());
            else if (menuManager.GetSceneManager().logInAttempts > 5)
            {
                StartCoroutine(InformCantAttempt("Can not allow log in, please restart an application"));
            }
            else
            {
                StartCoroutine(InformCantAttempt("Log In error"));
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
        private IEnumerator LogIn()
        {
            isLogInInProgress = true;
            menuManager.GetSceneManager().logInAttempts++;
            WWWForm wFrom = new WWWForm();
            wFrom.AddField("username", nameInputField.text);
            wFrom.AddField("password", passwordInputField.text);
            using (UnityWebRequest webRequest = UnityWebRequest.Post(logInPHPurl, wFrom))
            {
                logInButton.interactable = false;
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    StartCoroutine(InformCantAttempt("Error: " + webRequest.error));
                    //Debug.Log(": Error: " + webRequest.error);
                }
                else if (webRequest.downloadHandler.text == "Incorrect password" || webRequest.downloadHandler.text == "Incorrect username")
                {
                    logInButton.interactable = false;
                    StartCoroutine(InformCantAttempt("Error: " + "Incorrect credentials provided"));
                }
                else if (webRequest.downloadHandler.text == "1")
                {
                    logInButton.interactable = false;
                    StartCoroutine(LogInSuccesInfo());
                    //Debug.Log("User Login SUCESS, dataloader stuff");
                }
                else
                {
                    StartCoroutine(InformCantAttempt("Error: " + webRequest.error + " " + webRequest.downloadHandler.text));
                }
            }
            isLogInInProgress = false;
        }
        private IEnumerator LogInSuccesInfo()
        {
            isLogInSuccessProgress = true;
            menuManager.GetSceneManager().CallEventLoggedIn(nameInputField.text);
            menuManager.GetSceneManager().isLoggedIn = true;
            infoImage.SetActive(true);
            infoImage.GetComponent<Image>().color = Color.green;
            infoImage.GetComponentInChildren<TMP_Text>().text = "Player logged in successfully";
            yield return new WaitForSeconds(1);
            nameInputField.text = "";
            passwordInputField.text = "";
            isLogInInProgress = false;
            infoImage.SetActive(false);
            menuManager.LogInAttempt();
            isLogInSuccessProgress = false;
        }
    }
}
