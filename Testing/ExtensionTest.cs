using Silmoon.Extension;
using System.Linq.Expressions;
using System.Text;

namespace Testing
{
    public class ExtensionTest
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

    }
}
