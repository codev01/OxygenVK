using OxygenVK.AppSource;
using OxygenVK.AppSource.Authorization;
using OxygenVK.AppSource.LocalSettings.Attachments;
using OxygenVK.AppSource.Views;
using OxygenVK.AppSource.Views.Settings;

using VkNet;

using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace OxygenVK.AppSource.Authorization.DialogBoxes
{
	public sealed partial class InWhichWindowDialog : ContentDialog
	{
		public Frame Frame { get; set; }
		public string Token;

		private readonly DispatcherTimer DispatcherTimer = new DispatcherTimer
		{
			Interval = new System.TimeSpan(0, 0, 0, 2)
		};

		public InWhichWindowDialog()
		{
			InitializeComponent();
		}

		private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
		{
			saveIsPass.IsChecked = true;
			RequestedTheme = ThemeHelper.RootTheme;
			DispatcherTimer.Tick += InWhichWindowDialog_Tick;
			DispatcherTimer.Start();

			ToolTipService.SetToolTip(saveIsPass, new ToolTip
			{
				Content = "Используя ПИН-код, \nвы можете обеспечить защиту личных данных \nдополнительным шифрованием"
			});
		}

		private void InWhichWindowDialog_Tick(object sender, object e)
		{
			UpdateChckCapsLock();
		}

		private void ContentDialog_KeyUp(object sender, KeyRoutedEventArgs e)
		{
			UpdateChckCapsLock();
		}

		private void UpdateChckCapsLock()
		{
			CoreVirtualKeyStates keyStates = Window.Current.CoreWindow.GetKeyState(VirtualKey.CapitalLock);
			if (keyStates == CoreVirtualKeyStates.None)
			{
				warningCaps.Visibility = Visibility.Collapsed;
			}
			else
			{
				warningCaps.Visibility = Visibility.Visible;
			}
		}

		private Parameter GetParameter()
		{
			VkApi vkApi = Authorize.AuthorizeAsync(Token);
			
			if (isSave.IsChecked.Value)
			{
				if (saveIsPass.IsChecked.Value)
				{
					new WorkWithUserData().ReceivingDataAndSave(vkApi, pass1.Password);
				}
				else
				{
					new WorkWithUserData().ReceivingDataAndSave(vkApi);
				}
			}

			return new Parameter
			{
				VkApi = vkApi,
				ApplicationSettings = new ApplicationSettingsAttachments
				{
					ElementTheme = ThemeHelper.RootTheme,
					ElementSoundPlayerState = ElementSoundPlayer.State,
					ElementSpatialAudioMode = ElementSoundPlayer.SpatialAudioMode
				}
			};
		}

		private void PrimaryButton_Tapped(object sender, TappedRoutedEventArgs e)
		{
			Next(true);
		}

		private void SecondaryButton_Tapped(object sender, TappedRoutedEventArgs e)
		{
			Next(false);
		}

		private void Next(bool isPrimary)
		{
			if ((pass1.Password.Length == 0 || pass2.Password.Length == 0) && isSave.IsChecked.Value && saveIsPass.IsChecked.Value)
			{
				warningFields.Visibility = Visibility.Visible;
				warningFields.Text = "Есть незаполненное поле!";
			}
			else if ((pass1.Password.Length < 4 || pass2.Password.Length < 4) && isSave.IsChecked.Value && saveIsPass.IsChecked.Value)
			{
				warningFields.Visibility = Visibility.Visible;
				warningFields.Text = "ПИН-код должен содержать не менее 4 символов";
			}
			else if ((pass1.Password != pass2.Password) && isSave.IsChecked.Value && saveIsPass.IsChecked.Value)
			{
				warningFields.Visibility = Visibility.Visible;
				warningFields.Text = "Содержимое полей не совпадает!";
			}
			else
			{
				if (isPrimary)
				{
					new RootFrameNavigation(Frame, typeof(MainPage), GetParameter());
				}
				else
				{
					new WindowGenerator(GetParameter(), typeof(MainPage));
				}
				DispatcherTimer.Stop();
				DispatcherTimer.Tick -= InWhichWindowDialog_Tick;
				Hide();
			}
		}

		private void SaveIsPass_Unchecked(object sender, RoutedEventArgs e)
		{
			if (saveIsPass.IsChecked.Value == saveNotPass.IsChecked.Value)
			{
				saveIsPass.IsChecked = true;
				saveNotPass.IsChecked = false;
			}
		}
	}
}
