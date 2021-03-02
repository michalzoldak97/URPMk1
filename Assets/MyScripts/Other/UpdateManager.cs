using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace U1
{
    public class UpdateManager : MonoBehaviour
    {
        private List<UpdateBehaviour> toUpdate = new List<UpdateBehaviour>();
        
        public bool AddToList(UpdateBehaviour toAdd)
        {
            if (!toUpdate.Contains(toAdd))
            {
                toUpdate.Add(toAdd);
                return true;
            }
            else
                return false;
        }
        public bool RemoveFromList(UpdateBehaviour toGet)
        {
            return toUpdate.Remove(toGet);
        }
        private void UpdateList()
        {
            int count = toUpdate.Count;
            for (int i = 0; i < count; i++)
            {
                toUpdate[i].GetUpdate();
            }
        }
        private void Update()
        {
            UpdateList();
        }
    }
}
