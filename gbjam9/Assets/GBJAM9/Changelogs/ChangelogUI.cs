using System;
using System.Collections.Generic;
using System.Linq;
using GBJAM.Commons;
using UnityEngine;
using UnityEngine.UI;

namespace GBJAM9.Changelogs
{
    public class ChangelogUI : MonoBehaviour
    {
        public Text changelogText;

        public GameObject nextPageObject;

        public TextAsset changelogFile;
        
        private List<string> changelogPages = new List<string>();

        private int currentPage = 0;

        public GameboyButtonKeyMapAsset controls;
        
        private void Start()
        {
            // We assume first page is blank
            var lines = changelogFile.text.Split(new string[]
            {
                "\r\n", "\r", "\n"
            }, StringSplitOptions.None);

            var removedCommentLines = lines.Where(l => !l.TrimStart().StartsWith("//")).ToList();
            var newFile = string.Join("\n", removedCommentLines);

            changelogPages = newFile.Split('#').ToList();
            // changelogPages = changelogFile.text.Split('#').ToList();
            changelogPages.RemoveAt(0);
            
            changelogText.text = changelogPages[currentPage];
        }

        private void Update()
        {
            nextPageObject.SetActive(currentPage + 1 < changelogPages.Count);
            
            if (controls.button1Pressed && nextPageObject.activeSelf)
            {
                currentPage++;
                changelogText.text = changelogPages[currentPage];
            }

            if (controls.button2Pressed)
            {
                // CloseWindow();
            }
        }
    }
}
