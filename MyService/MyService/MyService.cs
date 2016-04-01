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
using System.Runtime.InteropServices;

namespace MyService
{
	public partial class MyService : ServiceBase
	{
		public enum ServiceState
		{
			SERVICE_STOPPED = 0x00000001,
			SERVICE_START_PENDING = 0x00000002,
			SERVICE_STOP_PENDING = 0x00000003,
			SERVICE_RUNNING = 0x00000004,
			SERVICE_CONTINUE_PENDING = 0x00000005,
			SERVICE_PAUSE_PENDING = 0x00000006,
			SERVICE_PAUSED = 0x00000007,
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct ServiceStatus
		{
			public long dwServiceType;
			public ServiceState dwCurrentState;
			public long dwControlsAccepted;
			public long dwWin32ExitCode;
			public long dwServiceSpecificExitCode;
			public long dwCheckPoint;
			public long dwWaitHint;
		}

		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

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
			var serviceStatus = new ServiceStatus
			{
				dwCurrentState = ServiceState.SERVICE_START_PENDING,
				dwWaitHint = 100000
			};
			SetServiceStatus(ServiceHandle, ref serviceStatus);

			eventLog1.WriteEntry("In OnStart");
			var timer = new Timer
			{
				Interval = 60000
			};
			timer.Elapsed += OnTimer;

			serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
			SetServiceStatus(ServiceHandle, ref serviceStatus);
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
