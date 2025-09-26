using CommsManager.Views;

namespace CommsManager;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		RegisterRoutes();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}

	private void RegisterRoutes()
	{
		Routing.RegisterRoute("CommissionDetailPage", typeof(CommissionDetailPage));
		Routing.RegisterRoute("CommissionsPage", typeof(CommissionsPage));
		Routing.RegisterRoute("CustomerEditPage", typeof(CustomerEditPage));
		Routing.RegisterRoute("CustomersPage", typeof(CustomersPage));
	}
}