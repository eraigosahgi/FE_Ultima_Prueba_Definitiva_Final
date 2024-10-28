﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUtilidadAzure.Objetos
{
	public class TableNameAttribute : Attribute
	{
		public string TableName { get; set; }

		public TableNameAttribute(string tablename)
		{
			this.TableName = tablename;
		}
	}
}