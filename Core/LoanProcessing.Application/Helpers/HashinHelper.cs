using System.Security.Cryptography;
using System.Text;

namespace LoanProcessing.Application.Helpers;

public static class HashingHelper
{
    public static string ComputeSha256Hash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}

