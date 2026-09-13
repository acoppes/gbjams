using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        public List<CharacterData> characters;

        public CharacterData GetCharacterData(string characterId)
        {
            return characters.FirstOrDefault(c =>
                c.id.Equals(characterId, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}