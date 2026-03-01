using Silmoon.Extensions;
using Silmoon.Secure;
using System.Linq.Expressions;
using System.Text;

namespace Testing
{
    public class GeneralTest
    {
        [Fact]
        public static void StringExtensionTest()
        {
            string chinese = "你好世界";
            string englishAndChinese = "Hello世界";

            var displayWidth = chinese.GetDisplayWidth();
            Console.WriteLine(displayWidth);
            Assert.Equal(8, displayWidth);

            var encodingByteCount = chinese.GetEncodingByteCount(Encoding.UTF8);
            Console.WriteLine(encodingByteCount);
            Assert.Equal(12, encodingByteCount);

            var substringDisplayWidth = englishAndChinese.SubstringDisplayWidth(4, 5);
            Console.WriteLine(substringDisplayWidth);
            Assert.Equal("o世界", substringDisplayWidth);

            Console.WriteLine();


            byte[] bytes = [0, 1, 2, 3, 4, 5];

            var hexString = bytes.ToHexString(true, true);
            Assert.Equal("0x102030405", hexString);
            Console.WriteLine(hexString);

            var result = hexString.HexStringToByteArray();
            Console.WriteLine(string.Join(", ", result.Data));
            Assert.Equal("1, 2, 3, 4, 5", string.Join(", ", result.Data));
            Console.WriteLine();
        }
        [Fact]
        public static void NumberExtensionTest()
        {
            decimal a = -1.03m;
            double b = 2;
            float c = 3;
            decimal d = 4;

            Console.WriteLine(a.Negative());
            Assert.Equal(-1.03m, a.Negative());
            Console.WriteLine(b.Negative());
            Assert.Equal(-2, b.Negative());
            Console.WriteLine(c.Negative());
            Assert.Equal(-3, c.Negative());
            Console.WriteLine(d.Negative());
            Assert.Equal(-4, d.Negative());
        }
        [Fact]
        public static void HashTest()
        {
            string s = "Hello世界";
            var md5Hash = s.GetMD5Hash();
            Console.WriteLine(md5Hash);
            Assert.Equal("0C904623F512630CDEE90AFBD0FB6E3C", md5Hash);
            var sha1Hash = s.GetSHA1Hash();
            Console.WriteLine(sha1Hash);
            Assert.Equal("BF6FC0C837449B1E310CAFEFCC53DF5A30AF11EF", sha1Hash);
            var sha256Hash = s.GetSHA256Hash();
            Console.WriteLine(sha256Hash);
            Assert.Equal("65C5437771864812AB040C07B86FD72432F9F55F7AC780EBAF02DE4CCFD28690".Replace(" ", ""), sha256Hash);
        }
        [Fact]
        public static void EncryptionTest()
        {
            string data = "Hello世界";
            string key = "1234567890123456";


            var encrypted1 = EncryptHelper.AesEncryptStringToBase64String(data, key);
            var decrypted2 = EncryptHelper.AesDecryptBase64StringToString(encrypted1, key);
            Console.WriteLine(encrypted1);
            Console.WriteLine(decrypted2);
            Assert.Equal(data, decrypted2);

            var encrypted2 = EncryptHelper.AesEncryptStringV2(data, key, false);
            var decrypted3 = EncryptHelper.AesDecryptStringV2(encrypted2, key, false);
            Console.WriteLine(encrypted2);
            Console.WriteLine(decrypted3);
            Assert.Equal(data, decrypted3);

            Assert.Equal(encrypted1, encrypted2);
            Assert.Equal(decrypted2, decrypted3);
        }
    }
}
