using UnityEngine;

namespace GBJAM14.Editor
{
    [CreateAssetMenu(menuName = "GBJAM/DownloadDatabaseAsset")]
    public class DownloadDatabaseAsset : ScriptableObject
    {
        public string outputPath;
        public string spreadsheetUrl;
    }
}