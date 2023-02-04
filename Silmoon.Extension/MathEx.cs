using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension
{
    public class MathEx
    {
        // 定义Pow方法，入参x表示底数，y表示指数
        public static decimal Pow(decimal x, int y)
        {
            // 定义结果变量result初始值为1
            decimal result = 1;

            // 当y大于0时执行循环
            while (y > 0)
            {
                // 如果y & 1 == 1，则说明y为奇数，需要乘以一次x
                if ((y & 1) == 1)
                    result *= x;

                // x每次循环都要乘以x
                x *= x;

                // y每次循环都要除以2
                y >>= 1;
            }

            // 返回结果
            return result;
        }
    }
}
