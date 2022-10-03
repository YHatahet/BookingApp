using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using BookingApp.Models;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;


using MongoDB.Bson;
using MongoDB.Driver;
using BookingApp.Services;
namespace BookingApp.Controllers;

[ApiController, Route("auth")]
public class AuthController : ControllerBase
{

    private IMongoCollection<User> _userCollection;
    private string _token;

    public AuthController(BookingAppService bookingAppService)
    {
        _userCollection = bookingAppService._userCollection;
        _token = bookingAppService.token;
    }

    private string ComputeHash(string plainText, string hashAlgorithm, byte[] saltBytes)
    {
        // If salt is not specified, generate it.
        if (saltBytes == null)
        {
            // Define min and max salt sizes.
            int minSaltSize = 4;
            int maxSaltSize = 8;

            // Generate a random number for the size of the salt.
            Random random = new Random();
            int saltSize = random.Next(minSaltSize, maxSaltSize);

            // Allocate a byte array, which will hold the salt.
            saltBytes = new byte[saltSize];

            // Initialize a random number generator.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            // Fill the salt with cryptographically strong byte values.
            rng.GetNonZeroBytes(saltBytes);
        }

        // Convert plain text into a byte array.
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        // Allocate array, which will hold plain text and salt.
        byte[] plainTextWithSaltBytes =
        new byte[plainTextBytes.Length + saltBytes.Length];

        // Copy plain text bytes into resulting array.
        for (int i = 0; i < plainTextBytes.Length; i++)
            plainTextWithSaltBytes[i] = plainTextBytes[i];

        // Append salt bytes to the resulting array.
        for (int i = 0; i < saltBytes.Length; i++)
            plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

        HashAlgorithm hash;

        // Make sure hashing algorithm name is specified.
        if (hashAlgorithm == null)
            hashAlgorithm = "";

        // Initialize appropriate hashing algorithm class.
        switch (hashAlgorithm.ToUpper())
        {

            case "SHA384":
                hash = new SHA384Managed();
                break;

            case "SHA512":
                hash = new SHA512Managed();
                break;

            default:
                hash = new MD5CryptoServiceProvider();
                break;
        }

        // Compute hash value of our plain text with appended salt.
        byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

        // Create array which will hold hash and original salt bytes.
        byte[] hashWithSaltBytes = new byte[hashBytes.Length +
        saltBytes.Length];

        // Copy hash bytes into resulting array.
        for (int i = 0; i < hashBytes.Length; i++)
            hashWithSaltBytes[i] = hashBytes[i];

        // Append salt bytes to the result.
        for (int i = 0; i < saltBytes.Length; i++)
            hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

        // Convert result into a base64-encoded string.
        string hashValue = Convert.ToBase64String(hashWithSaltBytes);

        // Return the result.
        return hashValue;
    }

    private bool VerifyHash(string plainText, string hashAlgorithm, string hashValue)
    {

        // Convert base64-encoded hash value into a byte array.
        byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

        // We must know size of hash (without salt).
        int hashSizeInBits, hashSizeInBytes;

        // Make sure that hashing algorithm name is specified.
        if (hashAlgorithm == null)
            hashAlgorithm = "";

        // Size of hash is based on the specified algorithm.
        switch (hashAlgorithm.ToUpper())
        {

            case "SHA384":
                hashSizeInBits = 384;
                break;

            case "SHA512":
                hashSizeInBits = 512;
                break;

            default: // Must be MD5
                hashSizeInBits = 128;
                break;
        }

        // Convert size of hash from bits to bytes.
        hashSizeInBytes = hashSizeInBits / 8;

        // Make sure that the specified hash value is long enough.
        if (hashWithSaltBytes.Length < hashSizeInBytes)
            return false;

        // Allocate array to hold original salt bytes retrieved from hash.
        byte[] saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];

        // Copy salt from the end of the hash to the new array.
        for (int i = 0; i < saltBytes.Length; i++)
            saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

        // Compute a new hash string.
        string expectedHashString = ComputeHash(plainText, hashAlgorithm, saltBytes);

        // If the computed hash matches the specified hash,
        // the plain text value must be correct.
        return (hashValue == expectedHashString);
    }

    private string CreateToken(User user)
    {

        List<Claim> claims = new List<Claim>
        {
            new Claim("id", user._id),
            new Claim("userType", user.userType)
        };
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_token));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1), // expire in 1 day
            signingCredentials: creds
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User newUser)
    {
        newUser.password = ComputeHash(newUser.password, "SHA512", null);

        await _userCollection.InsertOneAsync(newUser);
        return CreatedAtAction(nameof(Register), new { id = newUser._id }, newUser);

    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] User loginUser)
    {
        var user = await _userCollection.Find(o => o.username == loginUser.username).FirstOrDefaultAsync();

        if (user == null) return BadRequest("User not found");

        if (!VerifyHash(loginUser.password, "SHA512", user.password)) return BadRequest("Incorrect Password");
        //TODO cookie
        string token = CreateToken(user);
        return Ok(token);
    }
}

