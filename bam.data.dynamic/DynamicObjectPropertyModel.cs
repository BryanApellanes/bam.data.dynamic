/*
	Copyright © Bryan Apellanes 2015  
*/

using System.Reflection;

namespace Bam.Data.Dynamic
{
	public class DynamicObjectPropertyModel
	{
        public DynamicObjectPropertyModel()
        { }

		public DynamicObjectPropertyModel(PropertyInfo property)
		{
			this.PropertyInfo = property;
			this.PropertyName = property.Name;
			this.PropertyType = property.PropertyType.Name;
		}

		public string PropertyName { get; set; }
		public string PropertyType { get; set; }
		internal PropertyInfo PropertyInfo { get; set; }
	}
}
