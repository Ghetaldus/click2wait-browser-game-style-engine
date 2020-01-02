// (c) www.click2wait.net

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using click2wait;

namespace click2wait {

	[CreateAssetMenu(fileName = "New Unit", menuName = "Templates/New Unit", order = 999)]
	public partial class UnitTemplate : EntityTemplate
	{
    	
    	[Header("Currency Production")]
    	public CurrencyProductionModifier[] autoCurrencyProductionModifier;
    	
    	// -------------------------------------------------------------------------------
    	
		[Header("Folder Name")]
    	public string _folderName;
    	
    	public static string folderName = "";
    	
		private static Dictionary<int, UnitTemplate> _cache;

		public static Dictionary<int, UnitTemplate> dict
    	{
			get
			{
			
				if (_cache == null)
				{
				
					UnitTemplate[] templates = Resources.LoadAll<UnitTemplate>(UnitTemplate.folderName);
					
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