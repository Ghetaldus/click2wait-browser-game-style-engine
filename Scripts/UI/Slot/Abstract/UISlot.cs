// (c) www.click2wait.net

using UnityEngine;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{
	
	// -----------------------------------------------------------------------------------
	public abstract partial class UISlot<T> : UIThrottle
	{
	
    	public Button button;
    	
    	[Header("Image")]
		public Image image;
		public Image backgroundImage;
		public Image borderImage;
		
		[Header("Label")]
		public GameObject labelOverlay;
		public Text textLabel;
		
		[Header("Value")]
		public GameObject valueOverlay;
		public Text textValue;
				
		[Header("Timer")]
		public GameObject timerOverlay;
    	public Text timerText;
    	public Image timerCircle;
    	
    	[Header("Type & Action")]
    	public ObjectType objectType;
		public ActionType actionType;
    	
        protected CurrencyManager 	currencyManager;
		protected PermissionManager permissionManager;
		protected GameObject		localPlayer;
		protected T					entry;
		
		// -------------------------------------------------------------------------------
		public virtual void Init(GameObject _localPlayer, ref T _entry)
		{
			localPlayer 			= _localPlayer;
			entry 					= _entry;
			currencyManager 		= localPlayer.GetComponent<CurrencyManager>();
			permissionManager		= localPlayer.GetComponent<PermissionManager>();
		}
		
		// -------------------------------------------------------------------------------
		public virtual void Reset()
		{
			image.sprite = null;
			valueOverlay.SetActive(false);
			timerOverlay.SetActive(false);
			button.onClick.RemoveAllListeners();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}