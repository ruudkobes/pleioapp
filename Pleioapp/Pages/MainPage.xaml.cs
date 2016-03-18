﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Pleioapp
{
	public partial class MainPage : MasterDetailPage
	{
		public GroupPage groupPage = new GroupPage ();
		public LeftMenu leftMenu = new LeftMenu();

		public MainPage ()
		{
			InitializeComponent ();

			leftMenu.Menu.ItemSelected += (sender, e) =>  {
				ViewGroup(e.SelectedItem as Group);
			};

			Master = leftMenu;
			leftMenu.IsVisible = true;
			Detail = new NavigationPage(groupPage);

			MessagingCenter.Subscribe<Xamarin.Forms.Application> (App.Current, "logout", async(sender) => {
				groupPage.setGroup(null);
			});
		}

		void ViewGroup (Group group)
		{
			groupPage.setGroup (group);
		}
	}
}

