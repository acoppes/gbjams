using System;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;

namespace GBJAM14.Systems
{
    public class CharacterDB : MonoBehaviour
    {
        [Serializable]
        public class CharacterData
        {
            public string id;
            public string dialogName;
            public Sprite portrait;
            public Color textColor = Color.black;
            public MinMaxFloat randomPitch = new MinMaxFloat(1, 1);
        }

        public List<CharacterData> characters;

        public CharacterData GetCharacterData(string characterId)
        {
            return characters.FirstOrDefault(c =>
                c.id.Equals(characterId, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}