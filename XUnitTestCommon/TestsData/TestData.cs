using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsCore.TestsData
{
    public class TestData
    {
        private static Dictionary<string, string> Names()
        {
            Dictionary<string, string> Names = new Dictionary<string, string>();
            Names.Add("Han", "Solo");
            Names.Add("Lyke", "Skywalker");
            Names.Add("Anakin", "Skywalker");
            Names.Add("Coleman ", "Trebor");
            return Names;
        }

        public static KeyValuePair<string, string> FirstLastName()
        {
            Random r = new Random();
            return Names().ElementAt(r.Next(4));
        }

        public static string FullName()
        {
            Random r = new Random();
            int random = r.Next(4);
            return $"{Names().ElementAt(random).Key} {Names().ElementAt(random).Value}";
        }

        public static string GenerateString(int length) => Guid.NewGuid().ToString("n").Substring(0, length);

        public static string GenerateLetterString(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GenerateEmail() => $"lykke_autotest_{GenerateString(10)}@lykke.com";

        public static string GeneratePhone(int length = 12) => "+" + GenerateNumbers(length);

        public static string GenerateNumbers(int length = 12)
        {
            Random random = new Random();

            const string chars = "0123456789";

            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string AVATAR { get { return TestContext.CurrentContext.WorkDirectory.Remove(TestContext.CurrentContext.WorkDirectory.IndexOf("bin")) + "../TestsCore/TestsData/Images/lykke_avatar.png"; } }

        public static string DOCUMENT_PDF { get { return TestContext.CurrentContext.WorkDirectory.Remove(TestContext.CurrentContext.WorkDirectory.IndexOf("bin")) + "../TestsCore/TestsData/Images/lykke_document.pdf"; } }
    }
}
