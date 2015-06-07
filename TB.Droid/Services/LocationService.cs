﻿using System;
using Android.Locations;
using Android.App;
using Android.Content;
using Android.OS;
using System.Collections.Generic;
using System.Linq;

namespace TaskBuddi.Droid
{
	public class LocationService : Service, ILocationListener
	{
		IBinder binder;
		protected Location _currentLocation;
		protected LocationManager _locationManager;
		protected string _locationProvider;

		public event EventHandler<ConnectedEventArgs> Connected = delegate {};

		public class ConnectedEventArgs : EventArgs
		{
            
		}

		public class LocationBinder : Binder
		{
			public LocationService Service { get { return this.Service; } }

			// Return this instance of LocalService so clients can call public methods
			//			LocationService getService()
			//			{
			//				return LocationService.this;
			//			}
		}

		//@Override
		public override IBinder OnBind(Intent intent)
		{
			return binder;
		}

		//@Override
		//		public override StartCommandResult OnStartCommand(Intent intent, int flags, int startId)
		//		{
		//			if (intent.Action.Equals("startListening"))
		//			{
		//				_locationManager = this.GetSystemService(Context.LocationService);
		//
		//				// set accuracy for location requests
		//				Criteria criteria = new Criteria();
		//				criteria.Accuracy = Accuracy.Fine;
		//
		//				// Get suitable provider from OS
		//				IList<string> acceptableLocationProviders = _locationManager
		//                    .GetProviders(criteria, true);
		//				if (acceptableLocationProviders.Any())
		//					_locationProvider = acceptableLocationProviders.First();
		//				else
		//					_locationProvider = string.Empty; //none found
		//
		//				_locationManager.RequestLocationUpdates(_locationProvider, 10000, 0, this);
		//				//_locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 10000, 0, this);
		//			}
		//			else
		//			{
		//				if (intent.Action.Equals("stopListening"))
		//				{
		//					_locationManager.RemoveUpdates(this);
		//					_locationManager = null;
		//				}
		//			}
		//			return StartCommandResult.Sticky;
		//		}


		public void onLocationChanged(Location location)
		{
			this._currentLocation = location;   
			

		}

		public void onProviderDisabled(string provider)
		{
		}

		public void onProviderEnabled(string provider)
		{
		}

		public void onStatusChanged(string arg0, int arg1, Bundle arg2)
		{
		}

		public void OnLocationChanged(Location location)
		{
			throw new NotImplementedException();
		}

		public void OnProviderDisabled(string provider)
		{
			throw new NotImplementedException();
		}

		public void OnProviderEnabled(string provider)
		{
			throw new NotImplementedException();
		}

		public void OnStatusChanged(string provider, Availability status, Bundle extras)
		{
			throw new NotImplementedException();
		}
	}
}