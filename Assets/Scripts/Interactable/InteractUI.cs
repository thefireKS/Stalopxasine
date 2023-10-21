using System;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactable
{
    public class InteractUI : MonoBehaviour
    {
        [SerializeField] private GameObject Sprite;
        public void Show()
        {
            Sprite.SetActive(true);
        }

        public void Hide()
        {
            Sprite.SetActive(false);
        }
    }
}
