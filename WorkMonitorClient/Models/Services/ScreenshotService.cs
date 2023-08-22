using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management;
using WorkMonitorTypes.Requests;

namespace WorkMonitorClient.Models.Services
{
    public class ScreenshotService
    { 
        private int width;
        private int height;
        public ScreenshotService()
        {
            ManagementScope scope = new();
            scope.Connect();
            ObjectQuery query = new ("SELECT * FROM Win32_VideoController");
            using ManagementObjectSearcher searcher = new(scope, query);
            ManagementObject result = searcher.Get().Cast<ManagementObject>().First();            
            width = Convert.ToInt32(result.GetPropertyValue("CurrentHorizontalResolution"));
            height = Convert.ToInt32(result.GetPropertyValue("CurrentVerticalResolution"));                 
        }
        public byte[] GetScreenshot()
        {
            using Bitmap bitmap = new(width, height);
            using Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(Point.Empty, Point.Empty, bitmap.Size, CopyPixelOperation.SourceCopy);
            using MemoryStream ms = new();
            bitmap.Save(ms, ImageFormat.Jpeg);
            return ms.GetBuffer();
        }
      
    }   
}
