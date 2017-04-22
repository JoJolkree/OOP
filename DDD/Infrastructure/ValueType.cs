using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ddd.Taxi.Domain;

namespace Ddd.Infrastructure
{
	
	/// <summary>
	/// Базовый класс для всех Value типов.
	/// </summary>
	public class ValueType<T>
	{
	    public override string ToString()
	    {
	        var strings = GetType()
	            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
	            .OrderBy(x => x.Name)
	            .Select(x => x.Name.ToString() + ": " + (x.GetValue(this) == null ? "" : x.GetValue(this).ToString()))
                .ToArray();

            var builder = new StringBuilder();
	        builder.Append(GetType().Name + "(");
	        for(var i = 0; i < strings.Length; i++)
	        {
	            builder.Append(strings[i]);
                if(i < strings.Length - 1)
                    builder.Append("; ");
	        }
	        builder.Append(")");
	        return builder.ToString();
	    }

	    public override bool Equals(object obj)
	    {
	        var propertyInfos = GetType()
	            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
	            .OrderBy(x => x.Name);

	        foreach (var info in propertyInfos)
	        {
	            if (obj == null || obj.GetType() != GetType()) return false;
	            if (info.GetValue(obj) != null && info.GetValue(this) != null && 
                    !info.GetValue(obj).Equals(info.GetValue(this)))
	                return false;
	            if (info.GetValue(obj) == null && info.GetValue(this) != null ||
	                info.GetValue(this) == null && info.GetValue(obj) != null)
	                return false;
	        }
	        return true;
	    }

	    public bool Equals(PersonName name)
	    {
	        if (name == null || GetType() != name.GetType()) return false;

	        return name.FirstName == GetType().GetProperty("FirstName").GetValue(this).ToString() &&
	               name.LastName == GetType().GetProperty("LastName").GetValue(this).ToString();
	    }

	    public override int GetHashCode()
	    {
	        var properties = GetType()
	            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
	            .Select(x => x.GetValue(this));

	        unchecked
	        {
	            var result = 0;
	            foreach (var value in properties)
	                result += value.GetHashCode();
	            return result;
	        }
	    }
	}
	
}