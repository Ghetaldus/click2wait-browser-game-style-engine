// (c) www.click2wait.net

using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using click2wait;

namespace click2wait {
	
	[System.Serializable]
	public partial struct AchievementSyncStruct : ISyncableStruct<AchievementTemplate>
	{
	
    	public int		slot;
  		public int 		hash;
		public long 	value;

		// -------------------------------------------------------------------------------
		public AchievementSyncStruct(int _slot, AchievementTemplate template, long _value)
		{
			slot	= _slot;
			hash 	= (template == null) ? 0 : template.name.GetDeterministicHashCode();
			value	= Math.Max(0,_value);
		}
		
		// -------------------------------------------------------------------------------
		public AchievementTemplate template
		{
			get
			{
				if (hash == 0) return null;
				
				if (!AchievementTemplate.dict.ContainsKey(hash))
					throw new KeyNotFoundException("[Missing] AchievementTemplate not found in Resources: " + hash);
				return AchievementTemplate.dict[hash];
			}
		}  
		
    	// -------------------------------------------------------------------------------
    	public void Update(GameObject player)
    	{
    	
    	
    	}
    	
    	// -------------------------------------------------------------------------------
    	public bool Active
    	{
    		get { return hash != 0; }
    	}
    	
    	// -------------------------------------------------------------------------------
		public bool CanSell
		{
			get { return false; }
		}
    	
    	// -------------------------------------------------------------------------------
		public void Remove(long _amount=1)
		{
			// do nothing - we can never remove an achievement
		}
		
		// -------------------------------------------------------------------------------
		public void Reset()
		{
			// do nothing - we can never reset an achievement
		}
    	
	}
	
	public class SyncListAchievementSyncStruct : SyncList<AchievementSyncStruct> { }
	
}