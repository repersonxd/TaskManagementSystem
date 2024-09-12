public class CustomValidation
{
    public static bool IsStrongPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            return false;

        bool hasUpperChar = password.Any(char.IsUpper);
        bool hasLowerChar = password.Any(char.IsLower);
        bool hasMiniMaxChars = password.Length >= 8;
        bool hasNumber = password.Any(char.IsDigit);
        bool hasSymbols = password.Any(ch => !char.IsLetterOrDigit(ch));

        return hasUpperChar && hasLowerChar && hasMiniMaxChars && hasNumber && hasSymbols;
    }
}
