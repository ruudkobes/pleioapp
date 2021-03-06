﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Pleioapp
{
	public partial class EventPage : ContentPage
	{
		ObservableCollection<Event> events = new ObservableCollection<Event>();
		App app = (App) App.Current;
		Group Group;

		public EventPage ()
		{
			InitializeComponent ();
			EventListView.ItemsSource = events;

			EventListView.ItemSelected += (sender, e) => {
				var selectedEvent = e.SelectedItem as Event;
				if (selectedEvent.url != null) {
					app.ssoService.OpenUrl(selectedEvent.url);
				}
			};

			CouldNotLoad.GestureRecognizers.Add (new TapGestureRecognizer {
				Command = new Command (() => {
					Reload();
				}),
				NumberOfTapsRequired = 1
			});

			MessagingCenter.Subscribe<Xamarin.Forms.Application> (App.Current, "select_group", async(sender) => {
				var app = (App)App.Current;
				setGroup(app.currentGroup);
			});
		}

		public async void Reload()
		{
			CouldNotLoad.IsVisible = false;
			NoItems.IsVisible = false;
			events.Clear ();

			var app = (App)App.Current;
			var service = app.webService;

			try {
				var webEvents = await service.GetEvents (Group);

				foreach (Event e in webEvents) {
					events.Add (e);
				}

				if (events.Count == 0) {
					NoItems.IsVisible = true;
				}
			} catch (Exception e) {
				CouldNotLoad.IsVisible = true;
				System.Diagnostics.Debug.WriteLine ("Catched exception " + e);
			}
		}

		public async void setGroup(Group group)
		{
			Group = group;
			if (group == null) {
				events.Clear ();
				return;
			}

			Reload ();
		}

	}
}

