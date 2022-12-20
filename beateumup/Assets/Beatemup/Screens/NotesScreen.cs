using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Beatemup.Screens
{
    public class NotesScreen : MonoBehaviour
    {
        public InputAction nextPage;
        
        [FormerlySerializedAs("previousPage")] 
        public InputAction closeAction;
        
        [FormerlySerializedAs("changelogText")] 
        public Text text;

        public GameObject nextPageObject;

        [FormerlySerializedAs("changelogFile")] 
        public TextAsset file;
        
        private List<string> pages = new List<string>();

        private int currentPage = 0;

        public event Action onClose;

        private bool closed;

        public float touchDelay = 1.0f;
        private float touchDelayCurrent;
        
        private void OnEnable()
        {
            nextPage.Enable();
            closeAction.Enable();
        }
        
        private void Start()
        {
            LoadText(file);
            touchDelayCurrent = touchDelay;
        }

        public void LoadText(TextAsset textAsset)
        {
            file = textAsset;
            
            // We assume first page is blank
            var lines = textAsset.text.Split(new string[]
            {
                "\r\n", "\r", "\n"
            }, StringSplitOptions.None);

            var removedCommentLines = lines.Where(l => !l.TrimStart().StartsWith("//")).ToList();
            var newFile = string.Join("\n", removedCommentLines);

            pages = newFile.Split('#').ToList();
            pages.RemoveAt(0);
            
            text.text = pages[currentPage];
        }

        private void Update()
        {
            if (closed)
            {
                return;
            }
            
            nextPageObject.SetActive(currentPage + 1 < pages.Count);
            
            touchDelayCurrent -= Time.deltaTime;
            
            if (touchDelayCurrent > 0)
            {
                return;
            }
            
            if (nextPage.WasReleasedThisFrame() && nextPageObject.activeSelf)
            {
                currentPage++;
                text.text = pages[currentPage];
            }

            if (closeAction.WasReleasedThisFrame() || (nextPage.WasReleasedThisFrame() && !nextPageObject.activeSelf))
            {
                // CloseWindow();
                closed = true;
                onClose?.Invoke();
            }
        }
    }
}
