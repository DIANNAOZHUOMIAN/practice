using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Demo.Helpers
{
    public static class ImageHelper
    {
        /// <summary>
        /// 把一个图片转换成字节数组
        /// </summary>
        /// <param name="imgPath">图片的路径</param>
        /// <returns>字节数组</returns>
        public static byte[] ImgPathToByte(string imgPath)
        {
            FileStream fs = new FileStream(imgPath, FileMode.Open);
            byte[] byteData = new byte[fs.Length];
            fs.Read(byteData, 0, byteData.Length);
            fs.Close();
            return byteData;
        }

        /// <summary>
        /// 把字节数组转换成BitmapImage对象
        /// </summary>
        /// <param name="streamByte">字节数组</param>
        /// <returns>BitmapImage对象</returns>
        public static BitmapImage ByteToImg(byte[] streamByte)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(streamByte); 
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}
