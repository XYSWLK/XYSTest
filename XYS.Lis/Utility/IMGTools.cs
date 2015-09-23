using System;
using System.IO;

namespace XYS.Lis.Utility
{
    public class IMGTools
    {
        public static byte[] ReadImageFile(string filePath)
        {
            FileStream fs = File.OpenRead(filePath); //OpenRead
            int filelength = 0;
            filelength = (int)fs.Length; //获得文件长度 
            Byte[] image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            return image;
        }
    }
}
