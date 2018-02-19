using UnityEngine;
using System.Collections;
using GameUtil;

/// <summary>
/// Simple script that lets you localize a UIWidget based on DOAM I18n.
/// </summary>
/// 
	
public class DUILocalize : MonoBehaviour
{

	/// <summary>
	/// Localization key.
	/// </summary>

	public string key;
	bool mStarted = false;
	private bool translated = false;

	/// <summary>
	/// This function is called by the Localization manager via a broadcast SendMessage.
	/// </summary>

	void OnLocalize (bool force=false)
	{
		if (!DI18n.IsInitialized ()) {
			return ;
		}
#if NGUI
        //if(key == "dialogs.title.dragongear")
        //Debug.Log("localize key: "+ key);
        if (!translated || force) {
			UIWidget w = GetComponent<UIWidget> ();
			UILabel lbl = w as UILabel;

			if (string.IsNullOrEmpty (key)) {
//				throw new UnassignedReferenceException ("The I18n key can not be emptied: " + w.name);
				Logger.LogError("The I18n key can not be emptied: " + w.name);
			} 
				
			string val = DI18n.T(key);
			
			if(Capitalize)
				val = val.ToUpper();

			if (lbl != null) {				
				lbl.text = val;
			} 

		}
		translated = true;
#endif
	}

	/// <summary>
	/// Localize the widget on enable, but only if it has been started already.
	/// </summary>

	void OnEnable ()
	{
		if (mStarted)
			OnLocalize (false);
	}

	/// <summary>
	/// Localize the widget on start.
	/// </summary>

	void Start()
	{
		if( EnableOnAwake )
			return;
		mStarted = true;
		OnLocalize (true);
	}
	
	void Awake()
	{
		if( !EnableOnAwake )
			return;
		mStarted = true;
		OnLocalize (true);
	}
	
	
	public bool Capitalize = false;
	
	public bool EnableOnAwake = false;
}
