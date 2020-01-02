// (c) www.click2wait.net

using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public abstract partial class UIModal : MonoBehaviour
	{

		[SerializeField] protected UIWindowBackground background;
		[SerializeField] protected Animator animator;
		[SerializeField] protected string showTriggerName = "show";
		[SerializeField] protected string closeTriggerName = "close";
		[SerializeField] protected Text description;
		
		// -------------------------------------------------------------------------------
		void Start()
		{
			SetPopupActive(false);
		}
		
		// -------------------------------------------------------------------------------
		public virtual void Show(string _text)
		{
			description.text = _text;
			Setup();
		}
		
		// -------------------------------------------------------------------------------
		public virtual void onClickConfirm()
		{
			Close();
		}
		
		// -------------------------------------------------------------------------------
		public virtual void onClickCancel()
		{
			Close();
		}
		
		// -------------------------------------------------------------------------------
		protected void Setup()
		{
			SetPopupActive(true);
			animator.SetTrigger(showTriggerName);
			background.FadeIn();
		}
		
		// -------------------------------------------------------------------------------
		protected void Close()
		{
			animator.SetTrigger(closeTriggerName);
			background.FadeOut();
			StartCoroutine(Tools.GetMethodName(DeactivateWindow));
		}
		                          
		// -------------------------------------------------------------------------------
		protected IEnumerator DeactivateWindow()
		{
			yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
			SetPopupActive(false);
		}
		
		// -------------------------------------------------------------------------------
		protected void SetPopupActive(bool active)
		{
			gameObject.SetActive(active);
			background.gameObject.SetActive(active);
		}
		
		// -------------------------------------------------------------------------------
		
	}

}