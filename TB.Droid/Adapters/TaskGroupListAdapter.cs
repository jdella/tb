using System;
using System.Collections.Generic;
using Android.Widget;
using TaskBuddi.BL;
using TaskBuddi.BL.Managers;
using Android.App;
using Android;
using Android.Views;
using Android.Content;
using TaskBuddi.Droid.Screens;
using Android.Graphics;

namespace TaskBuddi.Droid.Adapters
{
	//Adapter to populate Group GridView on Home Screen
	public class TaskGroupListAdapter : BaseAdapter<TaskGroup>
	{
		protected Activity context = null;
		protected IList<TaskGroup> taskGroups = new List<TaskGroup>();
		protected LinearLayout vLayout;

		public TaskGroupListAdapter(Activity context)
			: base()
		{
			//GET TASK GROUPS
			this.context = context;
			this.taskGroups = TaskGroupManager.GetTaskGroups();
		}
		//# Return Views
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			// Inflate layout
			var view = context.LayoutInflater.Inflate(Resource.Layout.TaskGroupListItem, null);
			vLayout = view.FindViewById<LinearLayout>(Resource.Id.vLayout);

			//#Set Group Name and Clicks
			var group = taskGroups[position];
			var vGroupName = view.FindViewById<TextView>(Resource.Id.vGroupName);
			vGroupName.Text = group.Name;
			vGroupName.Click += (sender, e) =>
			{
				var taskGroupDetails = new Intent(context, typeof(TaskGroupDetailsScreen));
				taskGroupDetails.PutExtra("groupId", group.ID);
				context.StartActivity(taskGroupDetails);
			};

			//# List tasks for Group
			View taskItem;
			TextView taskName;
			ImageView taskIcon;
			var tasks = TaskManager.GetTasksByGroup(group.ID);

			foreach (var task in tasks)
			{
				// Inflate layout and bind data
				taskItem = context.LayoutInflater.Inflate(Resource.Layout.TaskListItem, null);
				taskName = taskItem.FindViewById<TextView>(Resource.Id.vName);
				taskIcon = taskItem.FindViewById<ImageView>(Resource.Id.vCheck);

				taskName.Text = task.Name;
				if (task.Done)
					taskIcon.SetImageResource(Resource.Drawable.ic_box_ticked);
				else
					taskIcon.SetImageResource(Resource.Drawable.ic_box_unticked);
				//taskItem.FindViewById<ImageView>(Resource.Id.vCheck).Visibility = task.Done ? 
				//ViewStates.Visible : ViewStates.Gone;

				//#Click Task item -> Task Details
				taskItem.Click += (sender, e) =>
				{
					var showDetails = new Intent(context, typeof(TaskDetailsScreen));
					showDetails.PutExtra("id", task.ID);
					context.StartActivity(showDetails);
				};
				//#Click Task item -> Task Details
				taskIcon.Click += (sender, e) =>
				{
					task.Done = true;
					TaskManager.SaveTask(task);
					taskIcon.SetImageResource(Resource.Drawable.ic_box_ticked);
				};
				vLayout.AddView(taskItem);
			}
			//todo refactor candidate...
			taskItem = context.LayoutInflater.Inflate(Resource.Layout.TaskListItem, null);
			taskItem.FindViewById<TextView>(Resource.Id.vName).Text = "add new task";
			taskItem.FindViewById<TextView>(Resource.Id.vName).SetTextColor(new Color(Resource.Color.shadeygrey));
			taskItem.FindViewById<ImageView>(Resource.Id.vCheck).SetImageResource(Resource.Drawable.ic_plus_thick);
			//taskItem.SetBackgroundColor(Color.White);
			//#Click Task item -> Task Details
			taskItem.Click += (sender, e) =>
			{
				var showDetails = new Intent(context, typeof(TaskDetailsScreen));
				showDetails.PutExtra("groupId", group.ID);
				context.StartActivity(showDetails);
			};
			vLayout.AddView(taskItem); 

			return view;
		}

		/* ADAPTER OVERRIDE METHODS */
		//#get group
		public override TaskGroup this [int position]
		{
			get { return taskGroups[position]; }
		}
		//#get id
		public override long GetItemId(int position)
		{
			return position;
		}
		//#get count
		public override int Count
		{
			get { return taskGroups.Count; }
		}

	}
}