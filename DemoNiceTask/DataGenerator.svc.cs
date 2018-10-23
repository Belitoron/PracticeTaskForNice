using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace DemoNiceTask
{
	[ServiceContract( Namespace = "" )]
	[AspNetCompatibilityRequirements( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed )]
	public class DataGenerator
	{
		[WebInvoke( Method = "POST",
					RequestFormat = WebMessageFormat.Json,
					UriTemplate = "/AddRandom",
					ResponseFormat = WebMessageFormat.Json,
					BodyStyle = WebMessageBodyStyle.Wrapped )]
		public void Generate()
		{
			Random random = new Random();
			//the easiest way to generate random string
			var randomStr = Guid.NewGuid().ToString( "n" );
			var name = randomStr.Substring( 0, 10 );
			var city = randomStr.Substring( 10, 10 );
			var birthday = DateTime.Now.AddYears( random.Next(-40, -20) );
			var vip = random.NextDouble() < 0.5; //  50/50  probability

			DBworker.InsertCustomer( new Customer( name, city, birthday, vip ) );
		}
		
	}
}
