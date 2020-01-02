// (c) www.click2wait.net

using System;
using System.Text;
using System.Linq;
using UnityEngine;
using Mirror;
using click2wait;

namespace click2wait {

	[System.Serializable]
	public abstract partial class PropertyManager : BaseManager
	{
		
		
		protected abstract int GetFreeSlot();
		
		protected abstract int GetIndexBySlot(int _slot);
		
		[Server]
		public abstract void CreateDefaultData();
		
		[Server]
		public abstract void InsertDummyData(int startIndex, int endIndex);
		
		[Command]
		public abstract void CmdSellEntry(int _index, long _amount);
		
		[Server]
		protected abstract void SellEntry(int _index, long _amount);
		
		/*
		
		[Command]
		public abstract void CmdBuyEntry();
		
		[Server]
		protected abstract void BuyEntry();
		*/
		
	}

}