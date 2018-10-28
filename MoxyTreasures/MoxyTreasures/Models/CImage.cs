using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoxyTreasures.Models
{
	public class CImage
	{
        public int ImageID { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string FileExtension { get; set; }
        public byte[] FileBytes { get; set; }

		public bool IsImageFile()
		{
			try
			{
				if (this.FileExtension.ToLower() == ".jpg" || this.FileExtension.ToLower() == ".bmp" || this.FileExtension.ToLower() == ".gif" || this.FileExtension.ToLower() == ".png")
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		public string BytesBase64
		{
			get
			{
				if (FileBytes.Length > 0) { return Convert.ToBase64String(FileBytes); }
				return string.Empty;
			}
		}

		public string GetShortExtension
		{
			get {
				if (FileExtension == null) { return string.Empty; }
				return FileExtension.Replace(".", string.Empty);
			}
		}
	}
	
}