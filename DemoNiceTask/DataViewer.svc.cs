using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Net;

namespace DemoNiceTask
{
	[ServiceContract( Namespace = "" )]
	[AspNetCompatibilityRequirements( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed )]
	public class DataViewer
	{
		[WebGet( UriTemplate = "/View",
		         ResponseFormat = WebMessageFormat.Json )]
		public List<Customer> ViewAll()
		{
			return DBworker.GetAllCustomers();
		}

		[WebGet( UriTemplate = "/View/{Id}", 
		         ResponseFormat = WebMessageFormat.Json )]
		public Customer FindCustomer(string Id){
			Guid parsedId;
			HttpResponseMessage resp;
			if( Guid.TryParse( Id, out parsedId ) ) {
				var customer = DBworker.GetCustomer( parsedId );
				if( customer != null ) {
					return customer;
				}
				resp = new HttpResponseMessage( HttpStatusCode.NotFound ) {
					Content = new StringContent( string.Format( "No Customer with ID = {0}", Id ) ),
					ReasonPhrase = "Customer ID Not Found"
				};
			}
			else {
				resp = new HttpResponseMessage( HttpStatusCode.BadRequest ) {
					Content = new StringContent( string.Format( "Wrong GUID format '{0}'", Id ) ),
					ReasonPhrase = "Incorrect GUID"
				};
			}
			throw new HttpResponseException( resp );
		}
	}
}
