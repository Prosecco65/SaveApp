﻿using App.ViewModels;
using App.ViewModels.DataViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		private readonly HomeViewModel _viewModel = new HomeViewModel();

		private readonly HomeGraphViewModel _graphViewModel = new HomeGraphViewModel();

		public HomePage()
		{
			InitializeComponent();
			this.BindingContext = _viewModel;
			this.SizeChanged += HomePage_SizeChanged;
			_viewModel.Stats.PropertyChanged += UpdateGraph;
		}

		private void HomePage_SizeChanged(object _, EventArgs e)
		{
			double pageWidth = this.Width > 480 ? 450.0d : this.Width - 30.0d;

			MovementsCanvas.HeightRequest = pageWidth;
			MovementsCanvas.WidthRequest = pageWidth;
			_graphViewModel.UpdateGraphSize((float)pageWidth);
			
			UpdateGraph("");
		}

		private void MovementsCanvas_PaintSurface(object _, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
		{
			var netWorth = _viewModel.Expenses + _viewModel.Income;
			_graphViewModel.Draw(_viewModel.Expenses, netWorth, e);
		}

		private async void OpenAdd_Clicked(object _, EventArgs e)
			=> await Navigation.PushAsync(new AddPage());

		private void RefreshView_Refreshing(object _, EventArgs e)
			=> UpdateGraph("");

		private void UpdateGraph(string name)
		{
			_viewModel.UpdateData();
			MovementsCanvas.InvalidateSurface();
            _viewModel.IsRefreshing = false;
        }
	}
}