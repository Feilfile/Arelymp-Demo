using System.Globalization;
using System.Net;
using System.Text;

namespace Frontend.Utils
{
    public static class ByteConverter
    {
        public static byte[] IpToBin(string ip)
        {
            return IPAddress.Parse(ip).GetAddressBytes();
        }

        public static string HexToIp(string ip)
        {
            return new IPAddress(long.Parse(ip, NumberStyles.HexNumber)).ToString();
        }

        public static byte[] StringToBytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string BytesToString(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
