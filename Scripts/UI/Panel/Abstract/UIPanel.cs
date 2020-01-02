// (c) www.click2wait.net

using UnityEngine;
using click2wait;

namespace click2wait
{

	public abstract partial class UIPanel : MonoBehaviour
	{
	
		[Header("UI Panel")]
        public GameObject windowPanel;
		public Transform contentGroup;
		[Range(0.01f, 3f)]
		public float updateInterval = 0.25f;
		
		protected GameObject localPlayer;
		protected string category;
		protected float fInterval;
		
		// -------------------------------------------------------------------------------
		void Update()
		{
			if (Time.time > fInterval || fInterval <= 0)
			{
			
				if (localPlayer == null)
		    		localPlayer = AccountManager.localPlayer;
		    	
		    	if (localPlayer == null)
		    		return;
				
				ThrottledUpdate();
				
				fInterval = Time.time + updateInterval;
			
			}
		}
		
		// -------------------------------------------------------------------------------
		protected virtual void ThrottledUpdate() {}
        
        // -------------------------------------------------------------------------------
        public virtual void Show(string _text)
        {
        	category = _text;
        	windowPanel.SetActive(!windowPanel.activeInHierarchy);
        }
        
        // -------------------------------------------------------------------------------
        public void Hide()
        {
        	windowPanel.SetActive(false);
        	category = "";
        }
        
        // -------------------------------------------------------------------------------
        
	}

}