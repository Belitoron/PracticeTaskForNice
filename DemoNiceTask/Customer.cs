using System;
using System.Runtime.Serialization;

namespace DemoNiceTask
{
	[DataContract]
	public class Customer
	{
		[DataMember]
		public Guid CustomerID;
		[DataMember]
		public string Name;
		[DataMember]
		public string City;
		[DataMember]
		public DateTime BirthDate;
		[DataMember]
		public bool IsVip;

		public Customer( string name, string city, DateTime birthday, bool vip)
		{
			Name = name;
			City = city;
			BirthDate = birthday;
			IsVip = vip;
		}

		public Customer(Guid id, string name, string city, DateTime birthday, bool vip )
			:this(name, city, birthday, vip)
		{
			CustomerID = id;
		}
	}
}