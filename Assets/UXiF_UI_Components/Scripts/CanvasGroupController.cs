using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEditor;


public class CanvasGroupController : MonoBehaviour, ICanvasRaycastFilter//, IMaterialModifier
{
    protected List<Image> _imagesInChildren;
	public List<Image> imagesInChildren
	{
		get
		{
			return _imagesInChildren;
		}
	}
	protected Dictionary<TextMeshProUGUI, Color> _colorMappedTextsInChildren;
	public Dictionary<TextMeshProUGUI, Color> colorMappedTextsInChildren
	{
		get
		{
			return _colorMappedTextsInChildren;
		}
		set
		{
			_colorMappedTextsInChildren = value;
		}
	}

    protected List<TextMeshProUGUI> _textsInChildren;
	public List<TextMeshProUGUI> textsInChildren
	{
		get
		{
			return _textsInChildren;
		}
	}
	protected List<TextMeshPro> _3DTextsInChildren;
	public List<TextMeshPro> threeDTextsInChildren
	{
		get
		{
			return _3DTextsInChildren;
		}
	}
    protected List<Material> _imageMatsInChildren;
	public List<Material> imageMatsInChildren
	{
		get
		{
			return _imageMatsInChildren;
		}
	}

	protected List<CanvasGroupController> _canvasControllersInChildren;
	public List<CanvasGroupController> canvasControllersInChildren
	{
		get
		{
			return _canvasControllersInChildren;
		}
	}


    protected Renderer[] _renderersInChildren;

    private MaterialPropertyBlock _propBlock;    
    
	private Shader customUIShader;

    //[HideInInspector]
    //public Shader imageShader;

	Shader shaderToUse;

    


    public Color tint = new Color(1, 1, 1, 0);

    [Range(0, 1)]
    public float _saturation = 1f;


    [Range(0, 1)]
    public float alpha = 1f;

    // If this bool is false on any BetterCanvasGroup that is a parent of a button,
    // the button will fail to get raycasts because Unity's Raycast checks components on parents
    // and the function toward the end of this file: ICanvasRaycastFilter.IsRaycastLocationValid()
    // will get called on each of them. IgnoreParentGroups will not prevent this.
    public bool blocksRaycasts = true;
    public bool ignoreParentGroups = false;


    CanvasGroupController m_parentCache = null;
    bool m_hasGottenParent = false;

    Data m_lastData;

    Dictionary<Material, Material> m_materials; // maps original materials to the version created by this group

    //public Color desaturatedColor = defaultDesatColor;
    //public float desaturatedSaturationValue = 0.1f;   

    private void Awake()
    {
		customUIShader = (Shader)Resources.Load<Shader> ("Shaders/CustomUIShader.shader");
		SetSharedMaterial(customUIShader);

		_imagesInChildren = GetOwnedImages(_imagesInChildren);

        
        _textsInChildren = GetOwnedTexts(_textsInChildren);
		_3DTextsInChildren = GetOwned3DTexts(_3DTextsInChildren);

        _imageMatsInChildren = new List<Material>();
        m_materials = new Dictionary<Material, Material>();

		foreach (Image i in _imagesInChildren)
		{
			if(i.material != null)
			{
				Material m = Instantiate(i.material);
				m_materials.Add(m, SharedMaterial);
				i.material = m;
				_imageMatsInChildren.Add(i.material);
			}
		}
    }
    public List<Image> GetOwnedImages(List<Image> images)
    {
		Image i = null;
		images = new List<Image>();
		CanvasGroupController parentController = null;
		_canvasControllersInChildren = new List<CanvasGroupController>();

		//CanvasGroupController[] allChildControllers = GetComponentsInChildren<CanvasGroupController>();
		//Image[] allChildImages = GetComponentsInChildren<Image>();
		Transform[] children = GetComponentsInChildren<Transform>();

		foreach(Transform child in children)
		{
			Debug.Log(child.name);

			if(child != transform && child.GetComponent<CanvasGroupController>() != null)
			{
				_canvasControllersInChildren.Add(child.GetComponent<CanvasGroupController>());
				continue;
			}

			i = child.GetComponent<Image>() != null ? child.GetComponent<Image>() : null;
			if(i != null)
			{
				parentController = child.parent.GetComponent<CanvasGroupController>() != null ? child.parent.GetComponent<CanvasGroupController>() : null;
				if(parentController == null)
				{
					if (i.GetComponent<CanvasGroupController>() == null || !i.GetComponent<CanvasGroupController>().ignoreParentGroups)
					{
						images.Add(i);
					}
				}				
				else if(!_canvasControllersInChildren.Contains(parentController))
				{
					if (i.GetComponent<CanvasGroupController>() == null || !i.GetComponent<CanvasGroupController>().ignoreParentGroups)
					{
						images.Add(i);
					}
				}
			}
		}
        //Image[] allChildImages = GetComponentsInChildren<Image>();
        //
        //if (GetComponent<Image>() != null)
        //{
        //    images.Add(GetComponent<Image>());
        //}
        //foreach (Image i in allChildImages)
        //{
        //    if (i.GetComponent<CanvasGroupController>() == null || !i.GetComponent<CanvasGroupController>().ignoreParentGroups)
        //    {
        //        images.Add(i);
        //    }
        //}

        return images;
    }

   	public List<TextMeshProUGUI> GetOwnedTexts(List<TextMeshProUGUI> texts)
    {
		_colorMappedTextsInChildren = new Dictionary<TextMeshProUGUI, Color>();


        TextMeshProUGUI[] allChildTexts = GetComponentsInChildren<TextMeshProUGUI>();
        texts = new List<TextMeshProUGUI>();
		TextMeshProUGUI myText = GetComponent<TextMeshProUGUI>();
		if (myText != null)
        {
			texts.Add(myText);
			_colorMappedTextsInChildren.Add(myText, myText.color);
        }
		foreach (TextMeshProUGUI text in allChildTexts)
        {
			if (text.GetComponent<CanvasGroupController>() == null || !text.GetComponent<CanvasGroupController>().ignoreParentGroups)
            {
                texts.Add(text);
				_colorMappedTextsInChildren.Add(text, text.color);
            }
        }
        return texts;
    }

	public List<TextMeshPro> GetOwned3DTexts(List<TextMeshPro> texts)
	{
		TextMeshPro[] allChildTexts = GetComponentsInChildren<TextMeshPro>();
		texts = new List<TextMeshPro>();
		if (GetComponent<TextMeshPro>() != null)
		{
			texts.Add(GetComponent<TextMeshPro>());
		}
		foreach (TextMeshPro text in allChildTexts)
		{
			if (text.GetComponent<CanvasGroupController>() == null || !text.GetComponent<CanvasGroupController>().ignoreParentGroups)
			{
				texts.Add(text);
			}
		}
		return texts;
	}

    public struct Data
    {
        public Color tint;
        public float saturation;
        public float alpha;
        public bool blocksRaycasts;

        public Data(Color tint, float saturation, float alpha, bool blocksRaycasts)
        {
            this.tint = tint;
            this.saturation = saturation;
            this.alpha = alpha;
            this.blocksRaycasts = blocksRaycasts;
        }

        public bool NeedsUpdating(Data oldData)
        {
            if (Mathf.Approximately(saturation, oldData.saturation) &&
                Mathf.Approximately(alpha, oldData.alpha) &&
                blocksRaycasts == oldData.blocksRaycasts &&
                Mathf.Approximately(tint.a, oldData.tint.a) &&
                Mathf.Approximately(tint.r, oldData.tint.r) &&
                Mathf.Approximately(tint.b, oldData.tint.b) &&
                Mathf.Approximately(tint.g, oldData.tint.g))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Stacks another data on top of this one
        /// </summary>
        public void Stack(Data top)
        {
            float totalTintAlpha = tint.a + top.tint.a;
            if (totalTintAlpha < .001f)
            {
                // avoid div by 0 when we have very low alpha
                tint = Color.Lerp(tint, top.tint, 0.5f);
                tint.a = 0;
            }
            else
            {
                // blend colors together and take the maximum alpha
                float newTintAlpha = Mathf.Max(tint.a, top.tint.a);
                tint = Color.Lerp(tint, top.tint, top.tint.a / totalTintAlpha);
                tint.a = newTintAlpha;
            }

            saturation = saturation * top.saturation;
            alpha = alpha * top.alpha;
            blocksRaycasts = blocksRaycasts | top.blocksRaycasts;
        }
    }

    /// <summary>
    /// Updates material properties based on the control parameters.
    /// This is called automatically if parameters are changed via the inspector or through animations.
    /// </summary>
    public void UpdateMeshes()
    {
        UpdateMesh(FinalData);
    }

    private void UpdateMesh(Data data)
    {
        if (data.NeedsUpdating(m_lastData))
        {
            m_lastData = data;
            //UpdateChildren();
        }
    }


    public Data LocalData
    {
        get
        {
            return new Data(tint, _saturation, alpha, blocksRaycasts);
        }
    }

    public Data FinalData
    {
        get
        {
            Data data = LocalData;
            if (!ignoreParentGroups)
            {
                CanvasGroupController parent = Parent;
                if (parent != null)
                {
                    data.Stack(parent.FinalData);
                }
            }
            return data;
        }
    }

    /// <summary>
    /// Gets the BetterCanvasGroup that owns this one, or null if no parent exists.
    /// </summary>
    CanvasGroupController Parent
    {
        get
        {
            if (m_parentCache == null && !m_hasGottenParent)
            {
                m_hasGottenParent = true;
                m_parentCache = GetBetterCanvasGroupInParentExlcudingSelf();
            }
            return m_parentCache;
        }
    }

    public CanvasGroupController GetBetterCanvasGroupInParentExlcudingSelf()
    {
        return GetComponentInParent<CanvasGroupController>().gameObject != gameObject ? GetComponentInParent<CanvasGroupController>() : null;
    }

    public void OnValidate()
    {
        SetColorsOfChildren();
    }

    public void OnDidApplyAnimationProperties()
    {
        SetColorsOfChildren();
    }

    void OnEnable()
    {
        //update materials in the event they were modified while this component or game object was disabled
        SetColorsOfChildren();
    }

    private static Material SharedMaterial { set; get; }
    static void SetSharedMaterial(Shader shader)
    {
        if (SharedMaterial == null)
        {
            SharedMaterial = Instantiate(Canvas.GetDefaultCanvasMaterial());
            SharedMaterial.shader = shader;
            // prevents unity from complaining about materials being leaked into the scene
            SharedMaterial.hideFlags = HideFlags.DontSave;
            SharedMaterial.name = string.Format("BCG ({0})", SharedMaterial.name);
        }
    }

    public Material GetModifiedMaterial(Material baseMaterial)
    {
        return SharedMaterial;
    }

	void OnRender(Material mat)
	{
		mat.SetFloat("_EffectAmount", 1f- _saturation);
		mat.SetColor("_Color", tint);
	}

    protected void SetColorsOfChildren()
    {	
        tint.a = alpha;
		Color textColor;
		float[] hsv = new float[3];


		if(_canvasControllersInChildren != null)
		{
			foreach(CanvasGroupController c in _canvasControllersInChildren)
			{
				if(!c.ignoreParentGroups)
				{
					c._saturation = _saturation;
					c.tint = tint;
					c.alpha = alpha;
					c.blocksRaycasts = blocksRaycasts;
					c.SetColorsOfChildren();
				}
			}
		}

		if(_imagesInChildren != null)
		{
			foreach(Material m in _imageMatsInChildren)
			{
				//Debug.Log("Setting saturation of child to " + (1f - saturation));
				//Debug.Log("Setting tint of child to " + tint);
				m.SetFloat("_EffectAmount", 1f - _saturation);
				m.SetColor("_Color", tint);
			}
		} 

		if(_colorMappedTextsInChildren != null)
		{
			foreach(KeyValuePair<TextMeshProUGUI, Color> text in _colorMappedTextsInChildren)
			{
				textColor = tint * text.Value;
				Color.RGBToHSV(textColor, out hsv[0], out hsv[1], out hsv[2]);
				hsv[1] = hsv[1] * _saturation;
				textColor = Color.HSVToRGB(hsv[0], hsv[1], hsv[2]);
				textColor.a = alpha;

				text.Key.color = textColor;
			}
		}

        //if (_textsInChildren != null)
        //{
        //    foreach (TextMeshProUGUI t in _textsInChildren)
        //    {
		//		//textColor = tint * t.color;
		//		//textColor.a = tint.a;
		//		//t.color = textColor;
		//		t.color = tint;
        //    }
        //}
		
		if(_3DTextsInChildren != null)
		{
			foreach(TextMeshPro t in _3DTextsInChildren)
			{
				t.color = tint;
			}
		}       
    }
   
    /// <summary>
    /// Destroys the object using Destroy() or DestroyImmediate()
    /// </summary>
    void BetterDestroy(UnityEngine.Object obj)
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            DestroyImmediate(obj);
        }
        else
        {
            Destroy(obj);
        }
        obj = null;
    }

    void OnDestroy()
    {
        if (m_materials != null)
        {
            foreach (Material mat in m_materials.Values)
            {
                BetterDestroy(mat);
            }
            m_materials = null;
        }

        //UpdateChildren(); // give all child materials a chance to update because this BetterCanvasGroup is no longer affecting them
    }

    bool ICanvasRaycastFilter.IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        // Same as comment above:
        // If this bool is false on any BetterCanvasGroup that is a parent of a DVButton,
        // the button will fail to get raycasts because Unity's Raycast checks components on parents
        // and this function will get called on each of them. IgnoreParentGroups will not prevent this.
        return blocksRaycasts;
    }
}
