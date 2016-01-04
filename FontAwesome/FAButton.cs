//
// FAButton.cs
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

namespace FontAwesome
{
	public class FAButton : UIButton
	{

		private FA _icon;
		public FA Icon {
			get {
				return _icon;
			}
			set {
				_icon = value;
				SetTitle (_icon.String(), UIControlState.Normal);
			}
		}

		public nfloat IconSize{
			get{ return this.Font.PointSize; }
			set{
				this.Font = FA.Adjust.Font (value);
			}
		}

		public FAButton (FA icon, UIColor fontColor, float iconSize = 20) : base()
		{
			Icon = icon;
			IconSize = iconSize;
			SetTitleColor (fontColor, UIControlState.Normal);
			SetTitleColor (fontColor.ColorWithAlpha(100), UIControlState.Highlighted);
		}
	}
}
