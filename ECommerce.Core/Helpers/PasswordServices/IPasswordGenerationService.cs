namespace ECommerce.Core.Helpers.PasswordServices
{
    public interface IPasswordGenerationService
    {
        bool VerifyPassword(string storedSalt, string storedHashedPassword, string inputPassword);
        byte[] GenerateSalt();
        byte[] Combine(byte[] passwordBytes, byte[] salt);
        byte[] HashBytes(byte[] inputBytes);
    }
}
