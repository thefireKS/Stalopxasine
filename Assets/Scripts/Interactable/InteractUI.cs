using System;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactable
{
    public class InteractUI : MonoBehaviour
    {
        [SerializeField] private GameObject Sprite;

        public static InteractUI InteractionMarker;

        private void Awake()
        {
            InteractionMarker = this;
        }
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
