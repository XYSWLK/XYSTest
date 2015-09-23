using System;
using System.Collections.Generic;
using System.IO;
using XYS.Lis.Utility;
namespace XYS.Lis.Section
{
   public class XingTai
    {
       public static byte[] GetImage(string imagePath)
       {
           string fullPath = Path.Combine(LisConfig.GetGSRemotePath(), imagePath);
           return IMGTools.ReadImageFile(fullPath);
       }
    }
}
