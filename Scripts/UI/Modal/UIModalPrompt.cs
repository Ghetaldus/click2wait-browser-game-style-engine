// (c) www.click2wait.net

using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIModalPrompt : UIModal
	{
	
		public static UIModalPrompt singleton;
	
		private Action confirmAction;
		private Action cancelAction;

		[SerializeField] protected Text confirmButtonText;
		[SerializeField] protected Text cancelButtonText;
		
		// -------------------------------------------------------------------------------
		void Awake()
		{
			if (singleton == null) singleton = this;
		}
	
		// -------------------------------------------------------------------------------
		public void Setup(string _description, string confirmText, string cancelText, Action confirm, Action cancel, bool hasCancelButton=true)
		{
			confirmAction = confirm;
			cancelAction = cancel;
			confirmButtonText.text = confirmText;
			cancelButtonText.text = cancelText;

			if (!hasCancelButton)
				cancelButtonText.transform.parent.gameObject.SetActive(false);
			else
				cancelButtonText.transform.parent.gameObject.SetActive(true);
	
			Show(_description);
		
		}

		// -------------------------------------------------------------------------------
		public override void onClickConfirm()
		{
			if (confirmAction != null)
				confirmAction();

			Close();
		}
	
		// -------------------------------------------------------------------------------
		public override void onClickCancel()
		{
			if (cancelAction != null)
				cancelAction();

			Close();
		}

		// -------------------------------------------------------------------------------
	
	}

}