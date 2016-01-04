//
// FAExt.cs
//
// Author:
//       Luc Dvchosal <luc@2113.ch>
//       Ideas of Neil Kennedy <neil.p.kennedy@outlook.com>
//
// Copyright (c) 2015 lduchosal
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the 'Software'), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED 'AS IS, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using UIKit;
using CoreGraphics;
using FontAwesome;
using Foundation;
using System.Text;
using System.Collections.Generic;

namespace FontAwesome
{
	public static class FAExt
	{
		public static List<FA> Values(this FA value) {

			var values = Enum.GetValues (typeof(FA));
			var result = new List<FA> ();
			foreach(FA item in values) {
				result.Add(item);
			}
			return result;

		}

		public static string String(this FA value) {

			byte b1 = (byte)value;
			byte b2 = (byte)((int)value >> 8);
			byte[] bs = new byte[] { b1, b2 };

			string result = Encoding.Unicode.GetString (bs);
			return result;
		}


		public static FAButton FAButton(this FA fa, UIColor color, float size) {
			var button = new FAButton (fa, color, size);
			return button;
		}

		public static UILabel Label(this FA fa, UIColor color, float size) {
			var label = new UILabel ();
			label.Font = fa.Font (size);
			label.TextColor = color;
			label.Text = fa.String ();
			label.TextAlignment = UITextAlignment.Center;

			return label;
		}

		/// Get a FontAwesome image with the given icon name, text color, size and an optional background color.
		///
		/// - parameter name: The preferred icon name.
		/// - parameter textColor: The text color.
		/// - parameter size: The image size.
		/// - parameter backgroundColor: The background color (optional).
		/// - returns: A string that will appear as icon with FontAwesome

		public static UIImage UIImage(this FA fa, CGSize size) {
			return fa.UIImage (size, UIColor.Black, UIColor.Clear);
		}

		public static UIImage UIImage(this FA fa, CGSize size, UIColor foreground, UIColor background) {
			var paragraph = new NSMutableParagraphStyle ();
			paragraph.Alignment = UITextAlignment.Center;

			// Taken from FontAwesome.io's Fixed Width Icon CSS
			nfloat fontAspectRatio = 1.28571429f;

			nfloat fontSize = (nfloat)Math.Min(size.Width / fontAspectRatio, size.Height);
			var attributes = new UIStringAttributes () {
				Font = fa.Font(fontSize),
				BackgroundColor = background,
				ForegroundColor = foreground,
				ParagraphStyle = paragraph
			};

			var attributedString = new NSMutableAttributedString(fa.String(), attributes);
			var point = new CGRect (0, (size.Height - fontSize) / 2, size.Width, fontSize);
			UIGraphics.BeginImageContextWithOptions(size, false, 0f);
			attributedString.DrawString (point);

			UIImage image = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();

			return image;

		}

		/// Get a FontAwesome image with the given icon name, text color, size and an optional background color.
		///
		/// - parameter name: The preferred icon name.
		/// - parameter textColor: The text color.
		/// - parameter size: The image size.
		/// - parameter backgroundColor: The background color (optional).
		/// - returns: A string that will appear as icon with FontAwesome
		public static UIImage TabBarIcon(this FA icon) {
			var iconsize = new CGSize (35, 35);
			return icon.UIImage (iconsize);
		}

		public static UIFont Font (this FA icon, nfloat size) {
			return UIFont.FromName ("FontAwesome", size);
		}
	}
}

