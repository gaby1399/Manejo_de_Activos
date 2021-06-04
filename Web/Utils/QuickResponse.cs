using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Web.Utils
{
    public class QuickResponse
    { /// <summary>
      /// Método que devuelve un la imagen generada
      /// El primer parámetro es la palabra(s) a convertir
      /// y el segundo parámetro es el nivel. Este parámetro  es muy importante
      /// </summary>
      /// <param name="input">la info que se desea ingresar en el QR</param>
      /// <param name="qrlevel"></param>
      /// <returns></returns>    

        public static byte[] QuickResponseGenerador(string input, int qrlevel)
        {

            string toenc = input;
            MessagingToolkit.QRCode.Codec.QRCodeEncoder qe = new MessagingToolkit.QRCode.Codec.QRCodeEncoder();
            qe.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qe.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L; // - Using LOW for more storage
            qe.QRCodeVersion = qrlevel;
            Image bm = qe.Encode(toenc);
            using (MemoryStream ms = new MemoryStream())
            {
                bm.Save(ms, ImageFormat.Jpeg);

                return ms.ToArray();
            }
        }
    }
}