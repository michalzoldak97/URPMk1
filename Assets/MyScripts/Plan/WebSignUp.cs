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
        [SerializeField] private TMP_Text infoText;
        [SerializeField] private int minNameLength, minPasswordLength;

        private void Start()
        {
            VerifyInputs();
        }

        public void CallSignUp()
        {
            StartCoroutine(SignUp());
        }

        private IEnumerator SignUp()
        {

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
                    Debug.Log("User register SUCESS");
                else
                    Debug.Log("User register Fail: " + webRequest.downloadHandler.text);
                
            }
        }

        public void VerifyInputs()
        {
            submitButton.interactable = (CheckInputFields());
        }
        private bool CheckInputFields()
        {
            if (nameInputField.text.Length < minNameLength || passwordInputField.text.Length < minPasswordLength)
            {
                infoText.text = "Name should be at least " + minNameLength + " characters long and password should be at least " + minPasswordLength + " characters long";
                return false;
            }
            else if (!Regex.Match(nameInputField.text, "^[A-Za-z0-9]*$").Success || !Regex.Match(passwordInputField.text, "^[A-Za-z0-9_]*$").Success)
            {
                infoText.text = "Name and password should contain only: lowercase and uppercase letters and/or numbers and/or dashes";
                return false;
            }
            else
            {
                infoText.text = "";
                return true;
            }
        }
    }
}
