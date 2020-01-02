// (c) www.click2wait.net

using UnityEngine;
using click2wait;

namespace click2wait
{

	public abstract partial class UIThrottle : MonoBehaviour
	{
	
		[Header("UI Throttle")]
		[Range(0.01f, 3f)]
		public float updateInterval = 0.25f;
		
		protected float fInterval;
		
		// -------------------------------------------------------------------------------
		void Update()
		{
			if (Time.time > fInterval || fInterval <= 0)
			{
			
				ThrottledUpdate();
				
				fInterval = Time.time + updateInterval;
			
			}
		}
		
		// -------------------------------------------------------------------------------
		protected virtual void ThrottledUpdate() {}
        
        // -------------------------------------------------------------------------------
        
	}

}