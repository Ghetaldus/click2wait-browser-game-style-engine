using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public abstract class UIWindow : MonoBehaviour, IWindow
	{

		[SerializeField] protected UIWindowBackground background;
		[SerializeField] protected Animator animator;
		[SerializeField] protected string showTriggerName = "show";
		[SerializeField] protected string closeTriggerName = "close";
	
		private void Start()
		{
			SetWindowActive(false);
		}

		public void Show()
		{
			SetWindowActive(true);
			animator.SetTrigger(showTriggerName);
			background.FadeIn();
		}

		public void Hide()
		{
			animator.SetTrigger(closeTriggerName);
			background.FadeOut();
			StartCoroutine(Tools.GetMethodName(DeactivateWindow));
		}

		private IEnumerator DeactivateWindow()
		{
			yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
			SetWindowActive(false);
		}

		private void SetWindowActive(bool active)
		{
			gameObject.SetActive(active);
			background.gameObject.SetActive(active);
		}

	}

}