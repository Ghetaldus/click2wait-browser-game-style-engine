// (c) www.click2wait.net

// A simple interface shared by all SyncStructs

using UnityEngine;
using click2wait;

namespace click2wait
{
	
	public interface ISyncableStruct<T>
	{
		void Update(GameObject player);
		bool Active { get; }
		bool CanSell { get; }
		void Remove(long _amount=1);
		void Reset();
		T template { get; }
	}
	
}