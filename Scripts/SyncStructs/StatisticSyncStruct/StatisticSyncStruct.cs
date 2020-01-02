// (c) www.click2wait.net

using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using click2wait;

namespace click2wait {
	
	[System.Serializable]
	public partial struct StatisticSyncStruct
	{
    	
  		public string 	name;
  		public byte		type;
 		public long		value;
 		
		public StatisticSyncStruct(string _name, byte _type, long _value)
		{
			name 		= _name;
			type		= _type;
			value 		= Math.Max(0,_value);
		}
		
		public bool GetMatch(string _name, byte _type)
		{
			return (name == _name && (type == _type || _type == 0) );
		}
    
    }
	
	public class SyncListStatisticSyncStruct : SyncList<StatisticSyncStruct> { }
	
}