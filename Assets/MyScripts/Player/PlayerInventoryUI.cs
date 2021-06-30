using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace U1
{
    public class PlayerInventoryUI : MonoBehaviour
    {
        [SerializeField] private Transform inventoryContent;
        [SerializeField] private GameObject inventoryButton;
        private PlayerInventoryMaster inventoryMaster;
        private void Start()
        {
            inventoryMaster = GetComponent<PlayerInventoryMaster>();
        }
        private void OnEnable()
        {
            inventoryMaster = GetComponent<PlayerInventoryMaster>();
            inventoryMaster.EventReloadUI += BuildInventoryUI;
        }
        private void OnDisable()
        {
            inventoryMaster.EventReloadUI -= BuildInventoryUI;
        }
        private void BuildInventoryUI(Transform dummy)
        {
            ClearInventoryUI();
            List<Transform> myItems = inventoryMaster.GetItemsOnPlayer();
            for (int i = 0; i < myItems.Count; i++)
            {
                SpawnItemButton(myItems[i]);
            }
        }
        private void SpawnItemButton(Transform toPlace)
        {
            GameObject Ibutton = Instantiate(inventoryButton, inventoryContent);
            Ibutton.GetComponentInChildren<TMP_Text>().text = toPlace.name;
            Ibutton.GetComponent<Button>().onClick.AddListener(delegate { CallEventActivateItem(toPlace); });
        }
        private void CallEventActivateItem(Transform toActivate)
        {
            inventoryMaster.CallEventActivateItem(toActivate);
        }
        private void ClearInventoryUI()
        {
            foreach(Transform uiElement in inventoryContent)
            {
                Destroy(uiElement.gameObject);
            }
        }
    }
}
