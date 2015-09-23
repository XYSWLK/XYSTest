using System;
using System.Collections;
using System.IO;
using XYS.Lis.Utility;
namespace XYS.Lis.Section
{
   public class Fish
    {
       private static readonly Hashtable itemNormalImage = new Hashtable();
       private static readonly Hashtable itemNormalName = new Hashtable();
       public static byte[] GetNormalImage(int itemNo)
       {
           if (itemNormalImage.Count ==0)
           {
               InitNormalImage();
           }
           return (byte[])itemNormalImage[itemNo];
       }
       private static void InitNormalImage()
       {
           if (itemNormalName.Count == 0)
           {
               InitNormalName();
           }
           foreach (DictionaryEntry de in itemNormalName)
           {
               byte[] result = IMGTools.ReadImageFile(Path.Combine(System.Environment.CurrentDirectory, "normal", de.Value.ToString()));
               itemNormalImage.Add((int)de.Key, result);
           }
       }
       private static void InitNormalName()
       {
           itemNormalName.Add(50006570, "AML1-ETO.jpg");
           itemNormalName.Add(90009363, "ATM-CEP11.jpg");
           itemNormalName.Add(90008499, "BCL2.jpg");
           itemNormalName.Add(90008738, "BCL6.jpg");
           itemNormalName.Add(50006569, "BCR-ABL.jpg");
           itemNormalName.Add(90009026, "BCR-ABL-ASS1.jpg");
           itemNormalName.Add(50006576, "CBFB.jpg");
           itemNormalName.Add(50006583, "CCND1-IgH.jpg");
           itemNormalName.Add(90009367, "CDKN2A.jpg");
           itemNormalName.Add(90008735, "CEP12.jpg");
           itemNormalName.Add(50006573, "CEPX-CEPY.jpg");
           itemNormalName.Add(90008729, "CKS1B-CDKN2C.jpg");
           itemNormalName.Add(90009373, "CRLF2.jpg");
           itemNormalName.Add(90009370, "D13S319.jpg");
           itemNormalName.Add(90008497, "D20S108.jpg");
           itemNormalName.Add(90008495, "D7S486-CEP7.jpg");
           itemNormalName.Add(90008494, "EGR1-D5S721.jpg");
           itemNormalName.Add(90008741, "EVI1.jpg");
           itemNormalName.Add(90008730, "FGFR1-D8Z2.jpg");
           itemNormalName.Add(50006582, "FGFR3-IgH.jpg");
           itemNormalName.Add(50006574, "IgH.jpg");
           itemNormalName.Add(90009041, "IGH-BCL2.jpg");
           itemNormalName.Add(90009364, "IGH-MAFB.jpg");
           itemNormalName.Add(50006581, "MAF-IgH.jpg");
           itemNormalName.Add(90009038, "IGH-MYC.jpg");
           itemNormalName.Add(50006575, "MLL.jpg");
           itemNormalName.Add(50006579, "MYC.jpg");
           itemNormalName.Add(90009376, "P2RY8.jpg");
           itemNormalName.Add(90009362, "P53-CEP17.jpg");
           itemNormalName.Add(90009032, "PDGFRA.jpg");
           itemNormalName.Add(90009035, "PDGFRB.jpg");
           itemNormalName.Add(50006571, "PML-RARA.jpg");
           itemNormalName.Add(90009029, "RARA.jpg");
           itemNormalName.Add(50006578, "RB-1.jpg");
           itemNormalName.Add(50006577, "SANTI8.jpg");
           itemNormalName.Add(90008496, "TCF3-PBX1.jpg");
           itemNormalName.Add(50006572, "TEL-AML1.jpg");
       }
    }
}
