using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace SPGW.Infra.Helpers
{
    public static class Utilty
    {
        public static DateTime ToPersianDate(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);
            int hour = pc.GetHour(dt);
            int min = pc.GetMinute(dt);
            int sec = pc.GetSecond(dt);

            return new DateTime(year, month, day, hour, min, sec);
            //return new DateTime(year, month, day, 0, 0, 0,0);
            //return new DateTime(day , month , year , hour, min,0);
        }
        public static string ToPersianDateTimeString(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);
            int hour = pc.GetHour(dt);
            int min = pc.GetMinute(dt);
            int sec = pc.GetSecond(dt);

            //return new DateTime(year, month, day, hour, min, 0);
            return year.ToString() + "/" + month.ToString() + "/" + day.ToString() + " - " + hour.ToString() + ":" + min.ToString() + ":" + sec.ToString();
            //return new DateTime(year, month, day, 0, 0, 0,0);
            //return new DateTime(day , month , year , hour, min,0);
        }

        public static string ToPersianDateTimeString_PPAymentDate(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);

            return year.ToString().PadLeft(4, '0') + month.ToString().PadLeft(2, '0') + day.ToString().PadLeft(2, '0');
        }

        public static string ToPersianDateTimeString_PPAymentTime(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);
            int hour = pc.GetHour(dt);
            int min = pc.GetMinute(dt);
            int sec = pc.GetSecond(dt);

            //return new DateTime(year, month, day, hour, min, 0);
            return /*year.ToString() + "/" + month.ToString() + "/" + day.ToString() + " - " +*/ hour.ToString() + ":" + min.ToString() + ":" + sec.ToString();
            //return new DateTime(year, month, day, 0, 0, 0,0);
            //return new DateTime(day , month , year , hour, min,0);
        }
        public static string ToPersianDateTimeString_Time(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);
            int hour = pc.GetHour(dt);
            int min = pc.GetMinute(dt);
            int sec = pc.GetSecond(dt);

            //return new DateTime(year, month, day, hour, min, 0);
            return /*year.ToString() + "/" + month.ToString() + "/" + day.ToString() + " - " +*/ hour.ToString() + ":" + min.ToString() + ":" + sec.ToString();
            //return new DateTime(year, month, day, 0, 0, 0,0);
            //return new DateTime(day , month , year , hour, min,0);
        }
        public static string ToPersianDateTimeString_Date(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);
            int hour = pc.GetHour(dt);
            int min = pc.GetMinute(dt);
            int sec = pc.GetSecond(dt);

            //return new DateTime(year, month, day, hour, min, 0);
            return year.ToString() + "/" + month.ToString() + "/" + day.ToString();// + " - " + hour.ToString() + ":" + min.ToString() + ":" + sec.ToString();
            //return new DateTime(year, month, day, 0, 0, 0,0);
            //return new DateTime(day , month , year , hour, min,0);
        }
        public static string ToPersianDateString(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);
            int hour = pc.GetHour(dt);
            int min = pc.GetMinute(dt);


            //return new DateTime(year, month, day, hour, min, 0);
            return year.ToString() + "/" + month.ToString() + "/" + day.ToString();//+ "/" + hour.ToString() + "/" + min.ToString();
                                                                                   //return new DateTime(year, month, day, 0, 0, 0,0);
                                                                                   //return new DateTime(day , month , year , hour, min,0);
        }
        public static string BPM_ToDateString(this DateTime dt)
        {
            return dt.Year.ToString().PadLeft(4) + dt.Month.ToString().PadLeft(2) + dt.Day.ToString().PadLeft(2);
        }

        public static string BPM_ToTimeString(this DateTime dt)
        {
            return dt.Hour.ToString().PadLeft(2) + dt.Minute.ToString().PadLeft(2) + dt.Second.ToString().PadLeft(2);
        }



        public static DateTime ToMiladiDate(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.ToDateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, 0);
        }

        public static byte[] SHA1Generate(string data)
        {
            byte[] dataBaytes = Encoding.UTF8.GetBytes(data);
            byte[] result;

            SHA1 sha = new SHA1CryptoServiceProvider();
            // This is one implementation of the abstract class SHA1.
            result = sha.ComputeHash(dataBaytes);
            return result;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        public enum ImageComperssion
        {
            Maximum = 50,
            Good = 60,
            Normal = 70,
            Fast = 80,
            Minimum = 90,
        }

        public static void ResizeImage(this Stream inputStream, int width, int height, string savePath, ImageComperssion ic = ImageComperssion.Normal)
        {
            System.Drawing.Image img = new Bitmap(inputStream);
            System.Drawing.Image result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawImage(img, 0, 0, width, height);
            }
            result.CompressImage(savePath, ic);
        }

        public static void ResizeImage(this System.Drawing.Image img, int width, int height, string savePath, ImageComperssion ic = ImageComperssion.Normal)
        {
            System.Drawing.Image result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawImage(img, 0, 0, width, height);
            }
            result.CompressImage(savePath, ic);
        }

        public static void ResizeImageByWidth(this Stream inputStream, int width, string savePath, ImageComperssion ic = ImageComperssion.Normal)
        {
            System.Drawing.Image img = new Bitmap(inputStream);
            int height = img.Height * width / img.Width;
            System.Drawing.Image result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawImage(img, 0, 0, width, height);
            }
            result.CompressImage(savePath, ic);
        }

        public static void ResizeImageByWidth(this System.Drawing.Image img, int width, string savePath, ImageComperssion ic = ImageComperssion.Normal)
        {
            int height = img.Height * width / img.Width;
            System.Drawing.Image result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawImage(img, 0, 0, width, height);
            }
            result.CompressImage(savePath, ic);
        }

        public static void ResizeImageByHeight(this Stream inputStream, int height, string savePath, ImageComperssion ic = ImageComperssion.Normal)
        {
            System.Drawing.Image img = new Bitmap(inputStream);
            int width = img.Width * height / img.Height;
            System.Drawing.Image result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawImage(img, 0, 0, width, height);
            }
            result.CompressImage(savePath, ic);
        }

        public static void ResizeImageByHeight(this System.Drawing.Image img, int height, string savePath, ImageComperssion ic = ImageComperssion.Normal)
        {
            int width = img.Width * height / img.Height;
            System.Drawing.Image result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawImage(img, 0, 0, width, height);
            }
            result.CompressImage(savePath, ic);
        }

        public static void CompressImage(this System.Drawing.Image img, string path, ImageComperssion ic)
        {
            System.Drawing.Imaging.EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Convert.ToInt32(ic));
            ImageFormat format = img.RawFormat;
            ImageCodecInfo codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == format.Guid);
            string mimeType = codec == null ? "image/jpeg" : codec.MimeType;
            ImageCodecInfo jpegCodec = null;
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < codecs.Length; i++)
            {
                if (codecs[i].MimeType == mimeType)
                {
                    jpegCodec = codecs[i];
                    break;
                }
            }
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            img.Save(path, jpegCodec, encoderParams);
        }

        public static DateTime StartOfDay(this DateTime theDate)
        {
            return theDate.Date;
        }

        public static DateTime EndOfDay(this DateTime theDate)
        {
            return theDate.Date.AddDays(1).AddTicks(-1);
        }
    }
}
