using UnityEngine;

public static class PlatformUtilities
{	
	private static bool _isMobile = Application.isMobilePlatform;

	private static bool _forceDebugMobile = false;

	public static bool IsMobileUI()
	{
		return _isMobile ? _isMobile : _forceDebugMobile;
	}
}

