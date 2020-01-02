// (c) www.click2wait.net

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIWindowLogin : UIWindow
	{
		public UIModalConfirm uiPopup;
	
		public NetworkManagerC2W manager;
		public NetworkAuthenticatorC2W auth;
	
		public GameObject panel;
		public Text statusText;
	
		public InputField accountInput;
		public InputField passwordInput;
	
		public Button loginButton;
		public Button registerButton;
		public Button hostButton;
		public Button dedicatedButton;
		public Button cancelButton;
		public Button quitButton;
		
		void Start()
		{
			if (PlayerPrefs.HasKey("AccountName"))
				accountInput.text = PlayerPrefs.GetString("AccountName", "");
			if (PlayerPrefs.HasKey("AccountPass"))
				passwordInput.text = PlayerPrefs.GetString("AccountPass", "");
		}

		void OnDestroy()
		{
			PlayerPrefs.SetString("AccountName", accountInput.text);
			PlayerPrefs.SetString("AccountPass", passwordInput.text);
		}

		void Update()
		{
			
			if (manager.state != NetworkState.Game)
			{
				panel.SetActive(true);

				if (manager.IsConnecting())
					statusText.text = "Connecting...";
				else if (!Tools.IsAllowedName(accountInput.text) || !Tools.IsAllowedPassword(passwordInput.text))
					statusText.text = "Check Name/Password";
				else
					statusText.text = "";
				
				accountInput.readOnly = manager.CanInput();
				passwordInput.readOnly = manager.CanInput();
				
				registerButton.interactable = manager.CanRegisterAccount(accountInput.text, passwordInput.text);
				registerButton.onClick.SetListener(() => { manager.TryRegisterAccount(accountInput.text, passwordInput.text); });
			
				loginButton.interactable = manager.CanLoginAccount(accountInput.text, passwordInput.text);
				loginButton.onClick.SetListener(() => { manager.TryLoginAccount(accountInput.text, passwordInput.text); });
			
				hostButton.interactable = manager.CanHostAndPlay(accountInput.text, passwordInput.text);
				hostButton.onClick.SetListener(() => { manager.TryHostAndPlay(accountInput.text, passwordInput.text); });
			
				cancelButton.gameObject.SetActive(manager.CanCancel());
				cancelButton.onClick.SetListener(() => { manager.TryCancel(); });
			
				dedicatedButton.interactable = manager.CanDedicatedServer();
				dedicatedButton.onClick.SetListener(() => { manager.TryDedicatedServer(); });
			
				quitButton.onClick.SetListener(() => { NetworkManagerC2W.Quit(); });

				auth.username = accountInput.text;
				auth.password = passwordInput.text;

			}
			else panel.SetActive(false);
		}
	}

}