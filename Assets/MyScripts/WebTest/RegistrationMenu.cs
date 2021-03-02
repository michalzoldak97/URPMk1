using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

namespace WEB1
{
    public class RegistrationMenu : MonoBehaviour
    {
        [SerializeField] private string registerPHPurl;
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private Button submitButton;

        public void CallRegister()
        {
            StartCoroutine(Register());
        }

        private IEnumerator Register()
        {
            WWWForm wFrom = new WWWForm();
            wFrom.AddField("username", nameInputField.text);
            wFrom.AddField("password", passwordInputField.text);
            WWW www = new WWW(registerPHPurl, wFrom);
            yield return www;
            if (www.text == "1")
            {
                Debug.Log("User register SUCESS");
                SceneManager.LoadScene(5);
            }
            else
            {
                Debug.Log("User register Fail: " + www.text);
            }
        }

        public void VerifyInputs()
        {
            submitButton.interactable = (nameInputField.text.Length >= 8 && passwordInputField.text.Length >= 8);
        }
    }
}
