// (c) www.click2wait.net

using System;
using System.Text;
using UnityEngine;
using click2wait;

namespace click2wait {
	
	// -----------------------------------------------------------------------------------
	[System.Serializable]
	public partial class Requirements
	{
		
		public ObjectType				objectType;
		public ActionType				actionType;
		
		public BuildingRequirement[] 	buildingRequirements;
		public TechnologyRequirement[] 	technologyRequirements;
		public EnhancementRequirement[] enhancementRequirements;
		public UnitRequirement[] 		unitRequirements;
		public EventRequirement[] 		eventRequirements;
		
	}
	
	// -----------------------------------------------------------------------------------
	[System.Serializable]
	public partial class BuildingRequirement
	{
		public BuildingTemplate template;
		public int level;
	}
	
	// -----------------------------------------------------------------------------------
	[System.Serializable]
	public partial class TechnologyRequirement
	{
		public TechnologyTemplate template;
		public int level;
	}
	
	// -----------------------------------------------------------------------------------
	[System.Serializable]
	public partial class EnhancementRequirement
	{
		public EnhancementTemplate template;
		public int level;
	}
	
	// -----------------------------------------------------------------------------------
	[System.Serializable]
	public partial class UnitRequirement
	{
		public UnitTemplate template;
		public int level;
	}
	
	// -----------------------------------------------------------------------------------
	[System.Serializable]
	public partial class EventRequirement
	{
		public EventTemplate template;
		public int level;
	}
	
	// -----------------------------------------------------------------------------------
	
}