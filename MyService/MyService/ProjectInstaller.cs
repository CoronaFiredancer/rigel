using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;

namespace MyService
{
	[RunInstaller(true)]
	public partial class ProjectInstaller : Installer
	{
		public ProjectInstaller()
		{
			InitializeComponent();
		}

		protected override void OnBeforeInstall(IDictionary savedState)
		{
			const string parameter = "MySource1\" \"MyLogFile1";
			Context.Parameters["assemblypath"] = "\"" + Context.Parameters["assemblypath"] + "\" \"" + parameter + "\"";

			base.OnBeforeInstall(savedState);
		}
	}
}
