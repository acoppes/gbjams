namespace GBJAM14.Services
{
    public interface IStorageService
    {
        void SaveTextToFile(string path, string contents);

        string LoadFileAsText(string path);

        void DeleteFile(string path);
    }
}