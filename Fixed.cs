using System;
using System.Globalization;
using System.Windows.Controls;

namespace VisiMatrix
{
    public static class Fixed
    {
        public static Single Float(UInt32 data, int floatingBits)
        {
            var multiplier = 1 << floatingBits;
            var value = ((Single) (data & (multiplier - 1)))/multiplier;
            var intBits = (int)data >> floatingBits;
            if ((intBits & (multiplier >> 1)) > 0)//means that intBits < 0
            {
                intBits |= ~ (multiplier - 1);
            }
            return value + intBits;
        }

        public static UInt32 Int(Single data, int floatingBits)
        {
            var high = ((uint) data) << floatingBits;
            var low = (uint) ((data - (int) data)*(1 << floatingBits));
            return high | low;
        }

        public static double Parse(TextBox box)
        {
            Double x;
            return Double.TryParse(box.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out x)
                ? x
                : Double.Parse(box.Text);
        }
    }
}
