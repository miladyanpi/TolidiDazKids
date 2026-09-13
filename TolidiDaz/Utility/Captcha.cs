//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.Drawing.Text;
//using System.IO;
//using System.Linq;
//using System.Web;



//namespace Utility
//{
//    public class Captcha
//    {
//        public Bitmap CaptchaImage { get; set; }
//        public byte[] CaptchaBytes { get; set; }
//        public string CaptchaResult { get; set; }

//        public Captcha()
//        {
//            CaptchaImage = new Bitmap(85, 20, PixelFormat.Format32bppArgb);
//            Graphics MyGraphics = Graphics.FromImage(CaptchaImage);
//            Rectangle MyRectangle = new Rectangle(0, 0, 85, 20);
//            SolidBrush MySolidBrush = new SolidBrush(Color.White);
//            MyGraphics.FillRectangle(MySolidBrush, MyRectangle);

//            // ایجاد کپچا
//            Random Rand = new Random();
//            int A, B, i;
//            A = Rand.Next(10, 99);
//            B = Rand.Next(0, 9);
//            string StrCaptcha = string.Format("{0} + {1} = ?", A, B);

//            CaptchaResult = (A + B).ToString();

//            Pen MyPen = new Pen(ColorTranslator.FromHtml("#d4d4d5"));
//            // رسم خط های عمودی زمینه
//            for (i = 0; i <= 85; i += 5)
//                MyGraphics.DrawLine(MyPen, i, 0, i, 20);
//            // رسم خط های افقی زمینه
//            for (i = 0; i <= 85; i += 5)
//                MyGraphics.DrawLine(MyPen, 0, i, 85, i);
//            // نوع رندر حروف
//            MyGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
//            // چاپ کپچا روی تصویر
//            MyGraphics.DrawString(StrCaptcha, new Font("Arial", 12, FontStyle.Italic), Brushes.Black, 2, 2);

//            using (MemoryStream stream = new MemoryStream())
//            {
//                CaptchaImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
//                CaptchaBytes = stream.ToArray();
//            }

//            MyPen.Dispose();
//            MySolidBrush.Dispose();
//            MyGraphics.Dispose();
//            CaptchaImage.Dispose();
//        }
//    }
//}