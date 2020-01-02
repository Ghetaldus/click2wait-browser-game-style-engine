// (c) www.click2wait.net

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using click2wait;

namespace click2wait {
	
	[CreateAssetMenu(fileName = "New Technology", menuName = "Templates/New Technology", order = 999)]
	public partial class TechnologyTemplate : BaseTemplate
	{
    	
    	[Header("Costs")]
		public LevelCurrencyCost[] buyCost;
		public LevelCurrencyCost[] sellCost;
		
    	[Header("Currency Production")]
    	public CurrencyProductionModifier[] autoCurrencyProductionModifier;
    	
    	// -------------------------------------------------------------------------------
    	
		[Header("Folder Name")]
    	public string _folderName;
    	
    	public static string folderName = "";
    	
		private static Dictionary<int, TechnologyTemplate> _cache;

		public static Dictionary<int, TechnologyTemplate> dict
    	{
			get
			{
			
				if (_cache == null)
				{
				
					TechnologyTemplate[] templates = Resources.LoadAll<TechnologyTemplate>(TechnologyTemplate.folderName);
					
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