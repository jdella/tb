using System;
using TaskBuddi.DL.SQLite;

namespace TaskBuddi.BL.Contracts
{
	/// <summary>
	/// Business entity base class. Provides the ID property.
	/// </summary>
	public abstract class BusinessEntityBase : IBusinessEntity
	{
		public BusinessEntityBase()
		{
		}

		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
	}
}

