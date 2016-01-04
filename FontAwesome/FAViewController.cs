//
// FAViewController.cs
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
using Foundation;
using System.Linq;
using System.Collections.Generic;
using FontAwesome;
using System.Threading;
using CoreGraphics;

namespace FontAwesome
{
	public class FAViewController : UICollectionViewController
	{

		private readonly FAViewSize _fasize;
		private readonly UISearchBar _searchbar;
		private readonly UIPinchGestureRecognizer _pinchrecognizer;
		private List<FA> _searchresult;
		private readonly List<FA> _datasource;

		public FAViewController (UICollectionViewFlowLayout layout, FAViewSize size) 
			: base (layout)
		{

			_fasize = size;

			_datasource = FA.Empty.Values();
			_searchresult = _datasource;
			_searchbar = new UISearchBar ();
			_searchbar.SearchBarStyle = UISearchBarStyle.Prominent;
			_pinchrecognizer = new UIPinchGestureRecognizer (PinchDetected);

			var searchdelegate = new UISearchFADelegate(this, _searchbar);
			_searchbar.Delegate = searchdelegate;


		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			CollectionView.RegisterClassForCell (typeof (FAViewCell), FAViewCell.Key);
			CollectionView.BackgroundColor = UIColor.White;
			CollectionView.AddGestureRecognizer (_pinchrecognizer);

			_searchbar.SizeToFit ();

			_searchbar.SearchBarStyle = UISearchBarStyle.Minimal;
			_searchbar.Placeholder = @"search here";

			NavigationItem.TitleView =  _searchbar;


		}

		internal void UpdateDatasource (string searchText)
		{
			_searchresult = _datasource
				.Where(v => v.ToString().Contains(searchText))
				.ToList()
				;
			
			CollectionView.ReloadData ();
		}

		public override nint NumberOfSections (UICollectionView collectionView)
		{
			return 1;
		}

		public override nint GetItemsCount (UICollectionView collectionView, nint section)
		{
			return _searchresult.Count;
		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = collectionView.DequeueReusableCell (FAViewCell.Key, indexPath) as FAViewCell;
			var fa = _searchresult [indexPath.Row];
			var width = _fasize.Size ().Width - 50;
			cell.Init (fa, width);

			return cell;

		}

		public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath)
		{

			_fasize.MinMax ();
			Zoom (indexPath);

		}

		void Zoom(NSIndexPath path) {

			((UICollectionViewFlowLayout)Layout).ItemSize = _fasize.Size(); 
			CollectionView.SetCollectionViewLayout (Layout, true);
			CollectionView.ScrollToItem (path, UICollectionViewScrollPosition.CenteredVertically, false); 
			CollectionView.ReloadData ();
		}

		protected void PinchDetected(UIPinchGestureRecognizer pinch) {

			if (pinch.State != UIGestureRecognizerState.Ended) {
				return;
			}

			var location = pinch.LocationInView (CollectionView);
			var path = CollectionView.IndexPathForItemAtPoint (location);
			if (path == null) {
				var visiblecells = CollectionView.IndexPathsForVisibleItems;
				int center = visiblecells.Length / 2;
				path = visiblecells [center];
			}
			if (pinch.Scale > 1) {
				_fasize.Increment ();
			}	

			if (pinch.Scale < 1) {
				_fasize.Decrement ();
			}

			Zoom (path);
		}

	}

	class UISearchFADelegate : UISearchBarDelegate 
	{

		private readonly UISearchBar _searchbar;
		private readonly FAViewController _searchviewcontroller;
		public UISearchFADelegate(FAViewController searchviewcontroller, UISearchBar searchbar) {
			_searchbar = searchbar;
			_searchviewcontroller = searchviewcontroller;
		}

		public override void TextChanged (UISearchBar searchBar, string searchText)
		{
			_searchviewcontroller.UpdateDatasource (searchText);
		}

		public override void CancelButtonClicked (UISearchBar searchBar)
		{
			_searchbar.Text = null;
			_searchbar.ResignFirstResponder ();
		}

		public override void SearchButtonClicked (UISearchBar searchBar)
		{
			_searchbar.EndEditing (true);
		}

		public override void OnEditingStarted (UISearchBar searchBar)
		{
			_searchbar.SetShowsCancelButton(true, true);
		}

		public override void OnEditingStopped (UISearchBar searchBar)
		{
			_searchbar.SetShowsCancelButton(false, true);
		}

	}

	public class FAViewSize
	{

		private readonly int _headerheigh;
		private readonly int _headerwidth;
		private readonly int _maxcellperwidth;

		private readonly int _spacing;
		private readonly int _inset;
		private readonly float _ratio;

		private readonly int _itemspacing;
		private readonly int _linespacing;

		private readonly nfloat _totalwidth;
		private int _cellperwidth;

		private int CellPerWidth {
			get { return _cellperwidth; }
		}

		private int Spacing { 
			get { return _spacing; }
		}
		private int Inset {
			get { return _inset; }
		}
		private float Ratio { 
			get { return _ratio; }
		}


		private int HeaderHeight { 
			get { return _headerheigh; }
		}
		private int HeaderWidth {
			get { return _headerwidth; }
		}


		private int ItemSpacing { 
			get { return _itemspacing; }
		}
		private int LineSpacing {
			get { return _linespacing; }
		}

		public FAViewSize(nfloat totalwidth) {

			_maxcellperwidth = 20;
			_headerheigh = 0;
			_headerwidth = 10;

			_spacing = 5;
			_inset = 5;
			_ratio = 1;
			_cellperwidth = 5;

			_itemspacing = 5;
			_linespacing = 2;
			_totalwidth = totalwidth;
		}

		public void Increment ()
		{
			if (_cellperwidth <= 1) {
				Interlocked.Exchange (ref _cellperwidth, 1);
				return;
			}
			Interlocked.Decrement (ref _cellperwidth);
		}

		public int MinMax ()
		{
			int result = _cellperwidth > 1 ? 1 : 3;
			Interlocked.Exchange (ref _cellperwidth, result);
			return result;
		}

		public void Decrement ()
		{
			if (_cellperwidth >= _maxcellperwidth) {
				Interlocked.Exchange (ref _cellperwidth, _maxcellperwidth);
				return;
			}
			Interlocked.Increment (ref _cellperwidth);
		}

		public CGSize Size() {
			nfloat width = ((_totalwidth - _cellperwidth -1) / _cellperwidth);
			nfloat height = width * Ratio;
			return new CGSize (width, height);
		}

		public UIEdgeInsets SectionInset {
			get {
				var inset = new UIEdgeInsets (Inset, Inset, Inset, Inset);
				return inset;
			}
		}

		public UICollectionViewFlowLayout FlowLayout ()
		{
			return new UICollectionViewFlowLayout {
				HeaderReferenceSize = new CGSize (HeaderWidth, HeaderHeight),
				ScrollDirection = UICollectionViewScrollDirection.Vertical,
				MinimumInteritemSpacing = ItemSpacing,
				MinimumLineSpacing = LineSpacing,
				ItemSize = Size()
			};	

		}
	}

	sealed class FAViewCell : UICollectionViewCell {

		public static readonly NSString Key = new NSString ("FAViewCell");
		private readonly UILabel _image;
		private readonly UILabel _label ;

		public UIView _root;

		[Export ("initWithFrame:")]
		public FAViewCell (CGRect frame) : base (frame)
		{

			_root = new UIView ();
			_root.TranslatesAutoresizingMaskIntoConstraints = false;

			_image = new UILabel ();
			_image.TranslatesAutoresizingMaskIntoConstraints = false;
			_image.TextAlignment = UITextAlignment.Center;

			_label = new UILabel ();
			_label.TranslatesAutoresizingMaskIntoConstraints = false;
			_label.TextAlignment = UITextAlignment.Center;

			_root.AddSubview (_image);
			_root.AddSubview (_label);

			ContentView.AddSubview (_root);

			UpdateConstraints ();
		}

		public override void UpdateConstraints ()
		{

			if (!NeedsUpdateConstraints ()) {
				return;
			}
			base.UpdateConstraints ();

			var metrics = new object[]{
				"image", _image,
				"rootmargin", 2,
				"imagemargin", 10,
				"labelmargin", 5,
				"label", _label,
				"root", _root,
			};


			var rootH1 = NSLayoutConstraint.FromVisualFormat (
				"H:|-rootmargin-[root]-rootmargin-|",
				NSLayoutFormatOptions.DirectionLeftToRight,
				metrics
			);

			var rootV1 = NSLayoutConstraint.FromVisualFormat (
				"V:|-rootmargin-[root]-rootmargin-|",
				NSLayoutFormatOptions.DirectionLeadingToTrailing,
				metrics
			);


			ContentView.AddConstraints (rootH1);
			ContentView.AddConstraints (rootV1);

			var consH1 = NSLayoutConstraint.FromVisualFormat (
				"H:|-imagemargin-[image]-imagemargin-|",
				NSLayoutFormatOptions.AlignAllCenterX,
				metrics
			);

			var consH2 = NSLayoutConstraint.FromVisualFormat (
				"H:|-labelmargin-[label]-labelmargin-|",
				NSLayoutFormatOptions.AlignAllCenterX,
				metrics
			);


			var consV1 = NSLayoutConstraint.FromVisualFormat (
				"V:|-imagemargin-[image]-labelmargin-[label(20)]-labelmargin-|",
				NSLayoutFormatOptions.DirectionLeadingToTrailing,
				metrics
			);

			_root.AddConstraints (consV1);
			_root.AddConstraints (consH1);
			_root.AddConstraints (consH2);

		}

		public override void PrepareForReuse ()
		{
			base.PrepareForReuse ();
			_image.Text = null;
			_label.Text = null;

		}

		public void Init(FA icon, nfloat width) {
			var text = icon.ToString();
			var image = icon.String();
			_label.Text = text;
			_image.Text = image;

			_image.Font = FA.Empty.Font (width);

		}

	}
}

