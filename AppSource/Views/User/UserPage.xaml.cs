using System.Collections.Generic;

using OxygenVK.AppSource.Views.Controls;

using Windows.UI.Xaml.Controls;

namespace OxygenVK.AppSource.Views.User
{
	public sealed partial class UserPage : Page
	{
		public UserPage()
		{
			InitializeComponent();
			NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

			for (int i = 0; i < 10; i++)
			{


				VKMessgae msg = new VKMessgae();

				List<ThumbAttachment> a = new List<ThumbAttachment>();

				for (int j = 0; j < i + 2; j++)
				{
					a.Add(new ThumbAttachment { Height = 100, Width = 160 });
				}

				msg.text = @"Если вaм пoвeзлo и вы мoжeтe нaблюдaть нoчaми звeзднoe нeбo, тo вы мoжeтe увидeть мeтeoритный дoждь, кoтoрый будeт дo кoнцa aпрeля, a oжидaeмый пик сeгoдня и зaвтрa (22.04 - 23.04).<LineBreak />
							Кaждый гoд в aпрeлe Зeмля прoхoдит сквoзь шлeйф пыли, кoтoрый oстaвляeт пoслe сeбя кoмeтa Тэтчeр. Прoхoдя чeрeз aтмoсфeру Зeмли, мeтeoры сгoрaют и oстaвляют длинныe вспышки.
							Присылaйтe свoи фoтoгрaфии, eсли зaфиксируeтe чтo - тo стрaннoe";
				msg.attachments = a;

				_lv.Items.Add(msg);
			}
		}

		public class VKMessgae
		{
			public string text { get; set; }
			public List<ThumbAttachment> attachments { get; set; }
		}
	}
}
