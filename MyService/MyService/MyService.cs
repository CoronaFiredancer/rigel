using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MyService
{
	public partial class MyService : ServiceBase
	{
		public MyService()
		{
			InitializeComponent();

			eventLog1 = new EventLog();

			if (!EventLog.SourceExists("MySource"))
			{
				EventLog.CreateEventSource("MySource", "MyNewLog");
			}

			eventLog1.Source = "MySource";
			eventLog1.Log = "MyNewLog";
		}

		protected override void OnStart(string[] args)
		{
			eventLog1.WriteEntry("In OnStart");
			var timer = new Timer
			{
				Interval = 60000
			};
			timer.Elapsed += OnTimer;
		}

		private void OnTimer(object sender, ElapsedEventArgs e)
		{
			eventLog1.WriteEntry("Monitoring system", EventLogEntryType.Information);
		}

		protected override void OnStop()
		{
			eventLog1.WriteEntry("In onStop.");
		}

		protected override void OnContinue()
		{
			eventLog1.WriteEntry("In OnContinue.");
		}
	}
}
