// (c) www.click2wait.net

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using click2wait;

namespace click2wait {

	public abstract partial class ScriptableTemplate : ScriptableObject
	{
	
		[Header("Basic Info")]
		public string 		title;
		public string 		category;
		[TextArea(1, 20)]
		public string 		description;
				
		[Header("Display")]
		public Sprite		backgroundIcon;
		public Sprite 		smallIcon;
		[Tooltip("Display priority (ascending)")]
		public int 			priority;
		
		
		
		protected string 	_name;
	
		public new string name
		{
			get {
				if (string.IsNullOrWhiteSpace(_name))
					_name = base.name;
				return _name;
			}
		}
	
		public virtual void OnValidate()
		{
	
			if (String.IsNullOrWhiteSpace(title))
				title = name;
			
		}

	}

}