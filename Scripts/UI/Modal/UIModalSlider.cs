// (c) www.click2wait.net

using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIModalSlider : UIModal
	{
	
		public static UIModalSlider singleton;
	
		private Action<long> confirmAction;
	
		[SerializeField] protected Text sliderValueText;
		[SerializeField] protected Slider slider;
		[SerializeField] protected Text confirmButtonText;
		
		// -------------------------------------------------------------------------------
		void Awake()
		{
			if (singleton == null) singleton = this;
		}
		
		// -------------------------------------------------------------------------------
		public void Setup(string _description, string confirmText, long _maxValue, Action<long> confirm)
		{
	
			confirmAction 			= confirm;
			confirmButtonText.text 	= confirmText;
			slider.value 			= 0;
			slider.maxValue 		= _maxValue;
		
			onChange();
		
			Show(_description);
		
		}
		
		// -------------------------------------------------------------------------------
		public void onClickMinus()
		{
			slider.value--;
			onChange();
		}
		
		// -------------------------------------------------------------------------------
		public void onClickPlus()
		{
			slider.value++;
			onChange();
		}
		
		// -------------------------------------------------------------------------------
		public void onChange()
		{
			sliderValueText.text = slider.value.ToString() + "/" + slider.maxValue.ToString();
		}
		
		// -------------------------------------------------------------------------------
		public override void onClickConfirm()
		{
			if (confirmAction != null)
				confirmAction(Convert.ToInt32(slider.value));

			Close();
		}

		// -------------------------------------------------------------------------------
		
	}

}