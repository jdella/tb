﻿using System;
using Android.Locations;
using Android.Content;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskBuddi.Droid
{
	using Android.App;
	using Android.Gms.Maps;
	using Android.Gms.Maps.Model;
	using Android.OS;
	using Android.Widget;

	[Activity(Label = "Task Map")]
	public class MapScreen : Activity, ILocationListener
	{
		protected GoogleMap _map;
		protected MapFragment _mapFragment;
		MarkerOptions marker;
		
		protected TextView vDebug;
		
		protected Location _currentLocation;
		protected LocationManager _locationManager;
		protected string _locationProvider;

		//** TODO Bind to Background Location Service implmentation**//
		//protected LocationService _locationService;
		//protected IServiceConnection serviceConnection = new IServiceConnection();
		//bool _isBound;

		protected override void OnCreate(Bundle bundle)
		{
			// Init vieww
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.MapLayout);
			vDebug = FindViewById<TextView>(Resource.Id.vDebug);

			InitMapFragment();
			InitLocationManager();

			//** TODO Bind to Background Location Service implmentation**//
			//_locationManager = GetSystemService(Context.LocationService) as LocationManager;
			//StartService(new Intent(this, typeof(LocationService)));
			//var intent = new Intent(this, typeof(LocationService));
			//BindService(intent, _locationService, Bind.AutoCreate);
		}

		protected override void OnResume()
		{
			base.OnResume();
			//start location listeners
			_locationManager.RequestLocationUpdates(_locationProvider, 6000, 50, this);
			SetupMapIfNeeded();
		}

		protected void InitLocationManager()
		{
			_locationManager = (LocationManager)GetSystemService(LocationService);

			// set accuracy for location requests
			Criteria criteria = new Criteria();
			criteria.Accuracy = Accuracy.Fine;

			// Get suitable provider from OS
			IList<string> acceptableLocationProviders = _locationManager
                .GetProviders(criteria, true);
			if (acceptableLocationProviders.Any())
				_locationProvider = acceptableLocationProviders.First();
			else
				_locationProvider = string.Empty; //none found
		}

		private void InitMapFragment()
		{
			// get fragment
			_mapFragment = FragmentManager.FindFragmentByTag("map") as MapFragment;
			if (_mapFragment == null)
			{
				GoogleMapOptions mapOptions = new GoogleMapOptions()
                    .InvokeMapType(GoogleMap.MapTypeSatellite)
					.InvokeZoomControlsEnabled(true)
					.InvokeCompassEnabled(true);
                
				// get map
				FragmentTransaction fragTx = FragmentManager.BeginTransaction();
				_mapFragment = MapFragment.NewInstance(mapOptions);
				fragTx.Add(Resource.Id.vMap, _mapFragment, "map");
				fragTx.Commit();
			}
		}

		private void SetupMapIfNeeded()
		{
			if (_map == null)
			{
				_map = _mapFragment.Map;
				UpdateMarker();
			}
		}

		private void UpdateMarker()
		{
			if (_map != null && _currentLocation != null)
			{
				marker = new MarkerOptions();
				var pos = new LatLng(_currentLocation.Latitude, _currentLocation.Longitude);
				marker.SetPosition(pos);
				marker.SetTitle("Current");
				marker.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueBlue));
				_map.AddMarker(marker);

				// We create an instance of CameraUpdate, and move the map to it.
				CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(pos, 15);
				_map.MoveCamera(cameraUpdate);
				GetDeviceLocation();
			}
		}
		//LOCATION METHODS
		async void GetDeviceLocation()
		{
			// debug result string
			var log = "No location";
			if (_currentLocation != null)
			{
				// Google returns a list of addresses
				Geocoder geocoder = new Geocoder(this);
				IList<Address> addressList = await geocoder
                    .GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10); //max results
				// Get first
				Address address = addressList.FirstOrDefault();
				if (address != null)
				{
					//build address string
					StringBuilder deviceAddress = new StringBuilder();
					for (int i = 0; i < address.MaxAddressLineIndex; i++)
					{
						deviceAddress.Append(address.GetAddressLine(i))
                            .AppendLine(",");
					}
					log = deviceAddress.ToString();
				}
				else
					log = "Unable to determine the address.";
			}
			// log result to screen
			vDebug.Text = System.DateTime.Now.TimeOfDay + " : " + log;
		}

		public void OnLocationChanged(Location location)
		{
			_currentLocation = location;
			if (_currentLocation != null)
			{
				UpdateMarker();
			}
		}

		public void OnProviderDisabled(string provider)
		{
			
		}

		public void OnProviderEnabled(string provider)
		{
			
		}

		public void OnStatusChanged(string provider, Availability status, Bundle extras)
		{
            
		}
	}
}
