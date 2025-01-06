using System;
using System.Globalization;
using System.Text;

namespace SmartBillApi.DataTransferObject.Core
{
    public static class StringEntension
    {
        public static string ToTitleCase(this string str)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(str);
        }
        public static string Encrypt(this string param)
        {
            if (string.IsNullOrEmpty(param))
                throw new NullReferenceException(string.Empty);
            try
            {
                byte[] encData_byte = new byte[param.Length];
                encData_byte = Encoding.UTF8.GetBytes(param);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public static string Decrypt(this string param)
        {
            if (string.IsNullOrEmpty(param))
                throw new NullReferenceException(string.Empty);

            try
            {
                UTF8Encoding encoder = new UTF8Encoding();
                Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(param);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new string(decoded_char);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
