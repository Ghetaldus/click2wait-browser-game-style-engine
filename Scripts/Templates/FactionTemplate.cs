﻿// (c) www.click2wait.net

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using click2wait;

namespace click2wait {
	
	[CreateAssetMenu(fileName = "New Faction", menuName = "Templates/New Faction", order = 999)]
	public partial class FactionTemplate : ScriptableTemplate
	{
	
	
		[Header("Folder Name")]
    	public string _folderName;
    	
    	public static string folderName = "";
    	
		private static Dictionary<int, FactionTemplate> _cache;

		public static Dictionary<int, FactionTemplate> dict
    	{
			get
			{
			
				if (_cache == null)
				{
				
					FactionTemplate[] templates = Resources.LoadAll<FactionTemplate>(FactionTemplate.folderName);
					
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