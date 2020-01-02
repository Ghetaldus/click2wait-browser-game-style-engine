// (c) www.click2wait.net

using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIModalConfirm : UIModal
	{

		public static UIModalConfirm singleton;
		
		protected Action confirmAction;
		
		[SerializeField] protected Text confirmButtonText;
		
		// -------------------------------------------------------------------------------
		void Awake()
		{
			if (singleton == null) singleton = this;
		}
		
		// -------------------------------------------------------------------------------
		public void Setup(string _description, string confirmText, Action confirm)
		{
	
			confirmAction 			= confirm;
			confirmButtonText.text 	= confirmText;
			
			Show(_description);
		
		}
		
		// -------------------------------------------------------------------------------
		public override void onClickConfirm()
		{
			if (confirmAction != null)
				confirmAction();

			Close();
		}
		
	}

}