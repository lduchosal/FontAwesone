//
// FABarButtonItem.cs
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

namespace FontAwesome
{
	public sealed class FABarButtonItem : UIBarButtonItem
	{
		private UILabel _titleLabel = null;
		private UIButton _iconButton = null;

		public override string Title{
			get{
				if (_titleLabel != null) {
					return _titleLabel.Text;
				} 
				return null;
			}
			set{
				if (_titleLabel != null) {
					_titleLabel.Text = value;
				}
			}
		}

		public string Icon {
			get {
				if (_iconButton != null) {
					return _iconButton.Title (UIControlState.Normal);
				}
				return null;

			}
			set {
				if (_iconButton != null) {
					_iconButton.SetTitle (value, UIControlState.Normal);
				}

			}
		}

		public FABarButtonItem (FA icon, UIColor fontColor, EventHandler handler) : base()
		{
			_iconButton = new UIButton (new CGRect (0, 0, 32, 32)) {
				Font = icon.Font (25)
			};
			_iconButton.SetTitleColor (fontColor, UIControlState.Normal);
			_iconButton.TouchUpInside += handler;

			this.Icon = icon.String();

			CustomView = _iconButton;
		}

		public FABarButtonItem (FA icon, string title, UIColor fontColor, EventHandler handler) : base()
		{
			UIView view = new UIView (new CGRect (0, 0, 32, 32));

			_iconButton = new UIButton (new CGRect (0, 0, 32, 21)) {
				Font = icon.Font (20),
			};
			_iconButton.SetTitleColor (fontColor, UIControlState.Normal);
			_iconButton.TouchUpInside += handler;

			_titleLabel = new UILabel (new CGRect (0, 18, 32, 10)) {
				TextColor = fontColor,
				Font = UIFont.SystemFontOfSize(10f),
				TextAlignment = UITextAlignment.Center
			};

			this.Title = title;
			this.Icon = icon.String();

			view.Add (_iconButton);
			view.Add (_titleLabel);

			CustomView = view;
		}


	}
}
