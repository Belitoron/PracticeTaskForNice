using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DemoNiceTask
{
	public static class DBworker
	{
		private static string insertQuery = @"INSERT INTO [dbo].[Customers] ([Id], [Name], [City], [BirthDate], [Vip])
		                                      VALUES (NEWID(), '{0}', '{1}', '{2:yyyy-MM-dd}', {3});";

		private static string selectAll = @"SELECT [Id], [Name], [City], [BirthDate], [Vip] FROM [dbo].[Customers]";
		private static string selectById = @"SELECT [Id], [Name], [City], [BirthDate], [Vip] FROM [dbo].[Customers]
		                                     WHERE [Id] = '{0}'";


		private static Customer ReadCustomer( SqlDataReader reader ) {
			var id = (Guid)reader.GetValue( 0 );
			var name = (string)reader.GetValue( 1 );
			var city = (string)reader.GetValue( 2 );
			var birthday = (DateTime)reader.GetValue( 3 );
			var vip = (bool)reader.GetValue( 4 );
			//TODO:  Add logger
			Console.WriteLine( "DEBUG: Read from DB user " + id.ToString() );
			return new Customer( id, name, city, birthday, vip );
		}

		public static void InsertCustomer( Customer customer )
		{
			var connection = new SqlConnection( ConfigurationManager.ConnectionStrings["localDB"].ConnectionString );
			try {
				connection.Open();
				using( var adapter = new SqlDataAdapter() ) {
					var query = string.Format( insertQuery, customer.Name, customer.City, customer.BirthDate, customer.IsVip ? 1 : 0 );
					adapter.InsertCommand = new SqlCommand( query, connection );
					adapter.InsertCommand.ExecuteNonQuery();
				}
				connection.Close();
			}
			catch( Exception exc ) {
				//TODO: Add logger
				Console.WriteLine( "ERROR: " + exc.Message );
			}
		}

		public static List<Customer> GetAllCustomers(){
			var lst = new List<Customer>();
			var connection = new SqlConnection( ConfigurationManager.ConnectionStrings["localDB"].ConnectionString );
			try {
				connection.Open();
				using( var command = new SqlCommand( selectAll, connection ) ){
					var reader = command.ExecuteReader();
					while( reader.Read() ) {
						lst.Add( ReadCustomer( reader ) );
					}
				}
				connection.Close();
			}
			catch( Exception exc ) {
				//TODO:  Add logger
				Console.WriteLine( "ERROR: " + exc.Message );
			}
			return lst;
		}

		public static Customer GetCustomer( Guid customerId )
		{
			Customer cust = null;
			var connection = new SqlConnection( ConfigurationManager.ConnectionStrings["localDB"].ConnectionString );
			try {
				connection.Open();
				var query = string.Format( selectById, customerId );
				using( var command = new SqlCommand( query, connection ) ) {
					var reader = command.ExecuteReader();
					if( reader.Read() ) {
						cust = ReadCustomer( reader );
					}
				}
				connection.Close();
			}
			catch( Exception exc ) {
				//TODO:  Add logger
				Console.WriteLine( "ERROR: " + exc.Message );
			}
			if(cust == null){
				//TODO:  Add logger
				Console.WriteLine( string.Format("WARN: Customer with ID = '{0}' not found", customerId ) );
			}
			return cust;
		}
	}
}