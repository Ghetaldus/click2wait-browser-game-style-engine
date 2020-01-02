// (c) www.click2wait.net

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using click2wait;

namespace click2wait {
	
	[CreateAssetMenu(fileName = "New Event", menuName = "Templates/New Event", order = 999)]
	public partial class EventTemplate : ScriptableTemplate
	{
	
		[Header("Stats")]
		[Tooltip("Allows to adjust the impact of this Event (mostly for modifiers)")]
		public int level;
		
		[Header("Active Period")]
		public UDateTime startDate;
		public UDateTime endDate;
		
		[Header("Currency Production")]
    	public CurrencyProductionModifier[] autoCurrencyProductionModifier;

		
		
		[Header("Folder Name")]
    	public string _folderName;
    	
    	public static string folderName = "";
    	
		private static Dictionary<int, EventTemplate> _cache;

		public static Dictionary<int, EventTemplate> dict
    	{
			get
			{
			
				if (_cache == null)
				{
				
					EventTemplate[] templates = Resources.LoadAll<EventTemplate>(EventTemplate.folderName);
					
					List<string> duplicates = templates.ToList().FindDuplicates(t => t.name);
					if (duplicates.Count == 0)
					{
						_cache = templates.ToDictionary(t => t.name.GetDeterministicHashCode(), t => t);
					}
					else
					{
						foreach (string duplicate in duplicates)
							Debug.LogError("[Warning] Resources folder contains multiple templates with the name " + duplicate);
					}
				}
				return _cache;
			}
        }
        
        public override void OnValidate()
		{
			base.OnValidate();
			folderName = _folderName;
		}
	
	
	
	
	}

}