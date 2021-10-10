using UnityEngine;
using UnityEngine.UI;

namespace GBJAM9.Changelogs
{
    public class ChangelogUI : MonoBehaviour
    {
        public Text changelogText;

        public GameObject nextPageObject;

        public GameObject closeObject;

        public TextAsset changelogFile;
        
        // TODO: gb controls
        
        private void Start()
        {
            changelogText.text = changelogFile.text;
        }
    }
}
