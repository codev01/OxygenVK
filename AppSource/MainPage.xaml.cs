using System;
using System.Numerics;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using MUXC = Microsoft.UI.Xaml.Controls;

namespace OxygenVK.AppSource
{
    public sealed partial class MainPage : Page
    {
        public Enum displayMode;
        public bool paneIsOpen;
        public static double ContainerAdapterWidth { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.SetTitleBar(AppTitleBar);
        }
        private void Navigation_Loaded(object sender, RoutedEventArgs e)
        {
            //contentFrame.Navigate(typeof(NewsPage), null, new DrillInNavigationTransitionInfo());
            headerShadow.Opacity = 0;
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            //if (scrollViewer.VerticalOffset == 0)
            //{
            //    headerShadow.Opacity = 0;
            //}
            //else
            //{
            //    headerShadow.Opacity = 1;
            //}
            //SetInitialVisuals();
        }

        //public void SetInitialVisuals()
        //{
        //    _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

        //    UpdateSeeAlsoPanelVerticalTranslationAnimation();
        //}

        //private void UpdateSeeAlsoPanelVerticalTranslationAnimation()
        //{
        //    ElementCompositionPreview.SetIsTranslationEnabled(add_onsPanelGrid, true);

        //    var targetPanelVisual = ElementCompositionPreview.GetElementVisual(add_onsPanelGrid);
        //    targetPanelVisual.Properties.InsertVector3("Translation", Vector3.Zero);

        //    var scrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(scrollViewer);

        //    var expression = _compositor.CreateExpressionAnimation("ScrollManipulation.Translation.Y * -1");
        //    expression.SetReferenceParameter("ScrollManipulation", scrollProperties);
        //    expression.Target = "Translation.Y";
        //    targetPanelVisual.StartAnimation(expression.Target, expression);
        //}

        //private void ScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    if (e.NewSize.Width <= 1035)
        //    {
        //        if (frameTransform.TranslateX == 0)
        //        {
        //            contentFrame.Margin = new Thickness(0);
        //            add_onsPanelGrid.Visibility = Visibility.Collapsed;
        //            showStoryboard.Begin();
        //        }
        //    }
        //    else
        //    {
        //        if (frameTransform.TranslateX == 310)
        //        {
        //            contentFrame.Margin = new Thickness(0, 0, 300, 0);
        //            add_onsPanelGrid.Visibility = Visibility.Visible;
        //            hideStoryboard.Begin();
        //        }
        //    }
        //}

        private void contentFrame_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                header.Width = ContainerAdapterWidth + 16;
            }
            catch
            {
                //
            }
        }

        private void Navigation_SelectionChanged(MUXC.NavigationView sender, MUXC.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {

            }
            switch (args.SelectedItemContainer.Tag.ToString())
            {
                case "news":
                    //contentFrame.Navigate(typeof(NewsPage), null, new DrillInNavigationTransitionInfo());
                    break;
                case "test":
                    break;
            }
        }

        private void Navigation_DisplayModeChanged(MUXC.NavigationView sender, MUXC.NavigationViewDisplayModeChangedEventArgs args)
        {
            displayMode = args.DisplayMode;
            AppNameTextBlock_Margin(displayMode, paneIsOpen);
        }

        private void Navigation_PaneClosing(MUXC.NavigationView sender, MUXC.NavigationViewPaneClosingEventArgs args)
        {
            accaunts.Visibility = Visibility.Collapsed;
            accaunts.Opacity = 0;
            paneIsOpen = false;
            AppNameTextBlock_Margin(displayMode, paneIsOpen);
        }

        private void Navigation_PaneOpening(MUXC.NavigationView sender, object args)
        {
            accaunts.Visibility = Visibility.Visible;
            accaunts.Opacity = 1;
            paneIsOpen = true;
            AppNameTextBlock_Margin(displayMode, paneIsOpen);
        }

        public void AppNameTextBlock_Margin(Enum displayMode, bool paneIsOpen)
        {
            switch (displayMode)
            {
                case MUXC.NavigationViewDisplayMode.Minimal:
                    if (paneIsOpen)
                    {
                        AppTitleBar.Margin = new Thickness(320, 0, 0, 0);
                        AppNameTextBlock.Visibility = Visibility.Collapsed;
                        AppNameTextBlock.Opacity = 0;
                        accaunts.Margin = new Thickness(0, 4, 10, -4);
                        accauntBorder.Width = 196;
                    }
                    else
                    {
                        AppTitleBar.Margin = new Thickness(80, 0, 0, 0);
                        AppNameTextBlock.Visibility = Visibility.Visible;
                        AppNameTextBlock.Opacity = 1;
                        AppNameTextBlock.Translation = new Vector3(0, 0, 0);
                        accaunts.Margin = new Thickness(0, 0, 10, 0);
                        accauntBorder.Width = 230;
                    }
                    break;
                case MUXC.NavigationViewDisplayMode.Compact:
                    AppTitleBar.Margin = new Thickness(40, 0, 0, 0);
                    if (paneIsOpen)
                    {
                        AppNameTextBlock.Translation = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        AppNameTextBlock.Translation = new Vector3(20, 0, 0);
                    }
                    break;
                case MUXC.NavigationViewDisplayMode.Expanded:

                    AppTitleBar.Margin = new Thickness(40, 0, 0, 0);
                    AppNameTextBlock.Visibility = Visibility.Visible;
                    AppNameTextBlock.Opacity = 1;
                    if (paneIsOpen)
                    {
                        AppNameTextBlock.Translation = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        AppNameTextBlock.Translation = new Vector3(20, 0, 0);
                    }
                    break;
            }
        }
    }
}
