using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WEB1
{
    public class MainMenu : MonoBehaviour
    {
        public void GoToRegister()
        {
            SceneManager.LoadScene(6);
        }
    }
}
