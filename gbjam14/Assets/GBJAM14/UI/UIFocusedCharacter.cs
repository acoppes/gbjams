using System;
using Game.Screens;
using GBJAM14.Systems;
using Gemserk.Utilities.UI;
using UnityEngine;

namespace GBJAM14.UI
{
    public class UIFocusedCharacter : MonoBehaviour
    {
        public UIWindow window;
        public TextView text;

        private CharacterDB characterDB;
        
        private void Awake()
        {
            characterDB = FindAnyObjectByType<CharacterDB>();
        }

        public void Show(string characterId)
        {
            var characterData = characterDB.GetCharacterData(characterId);
            text.SetText(characterData.dialogName);
            window.Open();
        }

        public void Hide()
        {
            window.Close();
        }
    }
}