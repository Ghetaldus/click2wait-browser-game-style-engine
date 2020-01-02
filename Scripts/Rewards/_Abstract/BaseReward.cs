// (c) www.click2wait.net

using System;
using System.Text;
using UnityEngine;
using click2wait;

namespace click2wait {
	
	[System.Serializable]
	public abstract partial class BaseReward
	{
		
		[HideInInspector]public string title;
		[HideInInspector]public byte state = Constants.STATE_NONE;
		[HideInInspector]public long timer = 0;
	
	/*
		public virtual void OnValidate()
		{
	
			if (String.IsNullOrWhiteSpace(name))
				title = name;
			
		}
	*/
	
	}

}