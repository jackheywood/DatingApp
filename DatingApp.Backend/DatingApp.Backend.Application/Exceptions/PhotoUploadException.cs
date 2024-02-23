namespace DatingApp.Backend.Application.Exceptions;

public class PhotoUploadException(string message) : Exception("Photo upload failed: " + message);
