// (c) www.click2wait.net

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using click2wait;

namespace click2wait {
	
	[CreateAssetMenu(fileName = "New Statistic", menuName = "Templates/New Statistic", order = 999)]
	public partial class StatisticTemplate : BaseTemplate
	{
	
		[Header("Folder Name")]
    	public string _folderName;
    	
    	public static string folderName = "";
    	
		private static Dictionary<int, StatisticTemplate> _cache;

		public static Dictionary<int, StatisticTemplate> dict
    	{
			get
			{
			
				if (_cache == null)
				{
				
					StatisticTemplate[] templates = Resources.LoadAll<StatisticTemplate>(StatisticTemplate.folderName);
					
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