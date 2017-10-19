using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using BB.DataLayer;
using BB.Implementation.Config;

namespace BB.Implementation
{
    public static class Security
    {
        const int SALT_LENGTH = 50;

        // These constants may be changed without breaking existing hashes.
        public const int SALT_BYTES = 24;
        public const int HASH_BYTES = 18;
        public const int PBKDF2_ITERATIONS = 64000;

        // These constants define the encoding and may not be changed.
        public const int HASH_SECTIONS = 5;
        public const int HASH_ALGORITHM_INDEX = 0;
        public const int ITERATION_INDEX = 1;
        public const int HASH_SIZE_INDEX = 2;
        public const int SALT_INDEX = 3;
        public const int PBKDF2_INDEX = 4;

        internal const string PASSWORD_MIN_LENGTH_MISSING = "Could not obtain minimum password length from config.";
        internal const string PASSWORD_MIN_SYMBOLS_MISSING = "Could not obtain minimum number of symbol characters from config.";
        internal const string PASSWORD_MIN_UPPERCASE_MISSING = "Could not obtain minimum number of uppercase characters from config.";
        internal const string PASSWORD_MIN_LOWERCASE_MISSING = "Could not obtain minimum number of lowercase characters from config.";
        internal const string PASSWORD_MIN_NUMBER_CHARS_MISSING = "Could not obtain minimum number of number characters from config.";
        internal const string PASSWORD_PREVIOUS_TO_CHECK_MISSING = "Could not obtain minimum number of previous passwords to check for repeats from config.";
        /// <summary>
        /// Generates a random 50 character string for use as a salt when checking passwords
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateNewSalt()
        {
            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
            var Salt = new byte[SALT_LENGTH];
            try
            { 
                using (rngCsp)
                {
                    rngCsp.GetBytes(Salt);
                }
            }
            catch (CryptographicException ex)
            {
                throw new CannotPerformOperationException(
                    "Random number generator not available.",
                    ex
                );
            }
            catch (ArgumentNullException ex)
            {
                throw new CannotPerformOperationException(
                    "Invalid argument given to random number generator.",
                    ex);
            }

            if (Salt == null || Salt.Length == 0)
            {
                throw new Exception("Salt Generation failed.");
            }

            return Salt;
        }

        public static PasswordCheckResponse CheckPassword(string Password, IEnumerable<PreviousPassword> PreviousPasswords)
        {
            //Get the settings from the config file
            var SecuritySettings = (SecuritySection)ConfigurationManager.GetSection("passwordPolicies");
            var sb = new StringBuilder();

            var PwdLengthPolicy = SecuritySettings.PasswordPolicies["Length"];

            int MinPwdLength = 0;
            if (!int.TryParse(PwdLengthPolicy.value, out MinPwdLength))
            {
                throw new Exception(PASSWORD_MIN_LENGTH_MISSING);
            }

            //Check password Length
            if (Password.Length < MinPwdLength)
            {
                sb.Append(PwdLengthPolicy.FormattedMessage);
                sb.Append("\n");
            }

            var PwdSymbolPolicy = SecuritySettings.PasswordPolicies["SymbolChars"];

            int MinSymbolChars = 0;
            if (!int.TryParse(PwdSymbolPolicy.value, out MinSymbolChars))
            {
                throw new Exception(PASSWORD_MIN_SYMBOLS_MISSING);
            }

            //Check number of symbol characters Length
            if (Password.Where(x => "!\"£$%^&*()_-+={}[]:;@'~#<,>.?/¬`¦|\\".Contains(x.ToString())).Count() < MinSymbolChars)
            {
                sb.Append(PwdSymbolPolicy.FormattedMessage);
                sb.Append("\n");
            }

            var PwduppercasePolicy = SecuritySettings.PasswordPolicies["UppercaseChars"];

            int MinUppercaseChars = 0;
            if (!int.TryParse(PwduppercasePolicy.value, out MinUppercaseChars))
            {
                throw new Exception(PASSWORD_MIN_UPPERCASE_MISSING);
            }

            //Check number of uppercase characters
            if (Password.Where(x => "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Contains(x.ToString())).Count() < MinUppercaseChars)
            {
                sb.Append(PwduppercasePolicy.FormattedMessage);
                sb.Append("\n");
            }

            var PwdLowercasePolicy = SecuritySettings.PasswordPolicies["LowercaseChars"];

            int MinLowercaseChars = 0;
            if (!int.TryParse(PwdLowercasePolicy.value, out MinLowercaseChars))
            {
                throw new Exception(PASSWORD_MIN_LOWERCASE_MISSING);
            }

            //Check number of lowercase characters
            if (Password.Where(x => "abcdefghijklmnopqrstuvwxyz".Contains(x.ToString())).Count() < MinLowercaseChars)
            {
                sb.Append(PwdLowercasePolicy.FormattedMessage);
                sb.Append("\n");
            }

            var PwdNumberPolicy = SecuritySettings.PasswordPolicies["NumberChars"];

            int MinNumberChars = 0;
            if (!int.TryParse(PwdNumberPolicy.value, out MinNumberChars))
            {
                throw new Exception(PASSWORD_MIN_NUMBER_CHARS_MISSING);
            }

            //Check number of number characters
            if (Password.Where(x => "01234567890".Contains(x.ToString())).Count() < MinNumberChars)
            {
                sb.Append(PwdNumberPolicy.FormattedMessage);
                sb.Append("\n");
            }

            var PrevPwdsPolicy = SecuritySettings.PasswordPolicies["PreviousPwdsToCheck"];

            int NumberPrevPwdsToCheck = 0;
            if (!int.TryParse(PrevPwdsPolicy.value, out NumberPrevPwdsToCheck))
            {
                throw new Exception(PASSWORD_PREVIOUS_TO_CHECK_MISSING);
            }

            //Check against previous passwords (if necessary)
            bool PwdFound = false;
            if (PreviousPasswords != null && PreviousPasswords.Count() > 0)
            {
                foreach (var PrevPwd in PreviousPasswords.OrderByDescending(x => x.CreationDate).Take(NumberPrevPwdsToCheck))
                {
                    //Get the submitted password hashed with the previous passwords salt
                    string currentPasswordHash = GetPasswordHash(Encoding.ASCII.GetBytes(PrevPwd.Salt), Password);
                    if (currentPasswordHash == PrevPwd.PasswordHash)
                    {
                        PwdFound = true;
                        break;
                    }
                }

                if (PwdFound)
                {
                    sb.Append(PrevPwdsPolicy.FormattedMessage);
                    sb.Append("\n");
                }
            }

            if (string.IsNullOrEmpty(sb.ToString()))
            {
                return new PasswordCheckResponse { PasswordOK = true };
            }
            else
            {
                return new PasswordCheckResponse { PasswordOK = false, Message = sb.ToString().Substring(0,sb.ToString().Length-1)};
            }
        }

        public static String GetPasswordHash(byte[] Salt, String Password)
        {

            byte[] hash = PBKDF2(Password, Salt, PBKDF2_ITERATIONS, HASH_BYTES);

            // format: algorithm:iterations:hashSize:salt:hash
            String parts = "sha1:" +
                PBKDF2_ITERATIONS +
                ":" +
                hash.Length +
                ":" +
                Convert.ToBase64String(Salt) +
                ":" +
                Convert.ToBase64String(hash);
            return parts;

        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt))
            {
                pbkdf2.IterationCount = iterations;
                return pbkdf2.GetBytes(outputBytes);
            }
        }

        public static string CreateHash(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[SALT_BYTES];
            try
            {
                using (RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider())
                {
                    csprng.GetBytes(salt);
                }
            }
            catch (CryptographicException ex)
            {
                throw new CannotPerformOperationException(
                    "Random number generator not available.",
                    ex
                );
            }
            catch (ArgumentNullException ex)
            {
                throw new CannotPerformOperationException(
                    "Invalid argument given to random number generator.",
                    ex
                );
            }

            byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTES);

            // format: algorithm:iterations:hashSize:salt:hash
            String parts = "sha1:" +
                PBKDF2_ITERATIONS +
                ":" +
                hash.Length +
                ":" +
                Convert.ToBase64String(salt) +
                ":" +
                Convert.ToBase64String(hash);
            return parts;
        }

    }

    public class PasswordCheckResponse
    {
        public Boolean PasswordOK { get; set; }

        public String Message { get; set; }
    }

    class InvalidHashException : Exception
    {
        public InvalidHashException() { }
        public InvalidHashException(string message)
            : base(message) { }
        public InvalidHashException(string message, Exception inner)
            : base(message, inner) { }
    }

    class CannotPerformOperationException : Exception
    {
        public CannotPerformOperationException() { }
        public CannotPerformOperationException(string message)
            : base(message) { }
        public CannotPerformOperationException(string message, Exception inner)
            : base(message, inner) { }
    }
}
