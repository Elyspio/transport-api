namespace Core.Exceptions;

public class FileNotFoundException : Exception
{
    public FileNotFoundException(string username, string fileId) : base(
        $"Could not found the file {fileId} for the user {username}")
    {
    }
}