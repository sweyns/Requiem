using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeWhenClose : MonoBehaviour
{
	[SerializeField]
	GameObject player;
	[SerializeField]
	float radius = 2;

    private bool canFade;
    private bool faded;
    private bool outOfRange;
    private bool canCount;

    private Vector3 targetPos;
    private IEnumerator routine; 

    MeshRenderer renderer;
    Material material;
    Shader transparent;
    Texture mainTex;

    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    GameObject lights;
    [SerializeField]
    GameObject people;
    [SerializeField]
    GameObject frame;




    // Start is called before the first frame update
    void Start()
    {
        gameManager.playerLight.range = PlayerPrefs.GetFloat("lightRange");

        //print(playerLight.range);
        if (gameManager.playerLight.range < 5) {
            gameManager.playerLight.range = 5;
        }
    	int state = 0;
    	if (gameManager != null){
    		state = PlayerPrefs.GetInt("level" + gameManager.currentLevel);
            if (PlayerPrefs.GetInt("level0") == 2 && PlayerPrefs.GetInt("level1") == 2 && PlayerPrefs.GetInt("level2") == 2 && PlayerPrefs.GetInt("level3") == 2) {
                state = 3;
            }
    		     
    	}
    	outOfRange = true;
		canCount = true;
		player = GameObject.FindWithTag("Player");
        targetPos = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        renderer = gameObject.GetComponent<MeshRenderer>();
        material = renderer.material;
        mainTex = renderer.materials[0].mainTexture;


        
    	
    	if (state == 0) {
    		canFade = true;
        	faded = false;
        	renderer.materials[0].mainTexture = null;
    	} else if (state == 1) {
    		faded = true;
    		//renderer.materials[0].color = new Color(renderer.materials[0].color.r, renderer.materials[0].color.g, renderer.materials[0].color.b, 0);
    		renderer.materials[0].mainTexture = null;
    		SetMaterialTransparent();

            iTween.FadeTo(gameObject, 0, 0.001f);
    	} else if (state == 2) {
    		faded = false;
    	} else if (state == 3) {
            lights.SetActive(true);
            people.SetActive(false);
            frame.SetActive(true);
            print("this happened");
        }
        
    }

    IEnumerator DoAThingOverTime(Color start, Color end, float duration) {
        for (float t = 0f; t < duration; t += Time.deltaTime) {
            float normalizedTime = t / duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            material.color = Color.Lerp(start, end, normalizedTime);
            yield return null;
        }
        material.color = end; //without this, the value will end at something like 0.9992367
    }

    void Fade() {
        for (float f = 0f; f >= 0f; f -= 0.05f) {
            material.color = new Color(material.color.r, material.color.g, material.color.b, f);
        }
    }

    private void SetMaterialTransparent()
    {
        foreach (Material m in GetComponent<Renderer>().materials)
        {
            m.SetFloat("_Mode", 2);

            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);

            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            m.SetInt("_ZWrite", 1);

            m.DisableKeyword("_ALPHATEST_ON");

            m.EnableKeyword("_ALPHABLEND_ON");

            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");

            m.renderQueue = 3000;
        }
    }

    private void SetMaterialOpaque()
    {
        foreach (Material m in GetComponent<Renderer>().materials)
        {
            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);

            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);

            m.SetInt("_ZWrite", 1);

            m.DisableKeyword("_ALPHATEST_ON");

            m.DisableKeyword("_ALPHABLEND_ON");

            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");

            m.renderQueue = -1;
        }
        if (gameManager != null) PlayerPrefs.SetInt("level" + gameManager.currentLevel, 2);

    }

    IEnumerator WaitToIncrease(float startRange, float endRange, float duration) {
        for (float t = 0f; t < duration; t += Time.deltaTime) {
            float normalizedTime = t / duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            gameManager.playerLight.range = Mathf.Lerp(startRange, endRange, normalizedTime);
            yield return null;
        }
        gameManager.playerLight.range = endRange; //without this, the value will end at something like 0.9992367
        PlayerPrefs.SetFloat("lightRange", gameManager.playerLight.range);

    }
    IEnumerator CountTime(float waitTime) {
    	

    	yield return new WaitForSeconds(waitTime);
    	
    	GetComponent<Renderer>().materials[0].mainTexture = mainTex;
    	iTween.FadeTo(gameObject, 1, 5f);
    	Invoke("SetMaterialOpaque", 5.0f);
    	faded = false;
        StartCoroutine(WaitToIncrease(gameManager.playerLight.range, gameManager.playerLight.range + 5, 5));
        
    	//SetMaterialOpaque();

    	
    }

    IEnumerator SetFadedTrueAfterSeconds(float seconds) { 
    	yield return new WaitForSeconds(seconds);
    	if (gameManager != null) PlayerPrefs.SetInt("level" + gameManager.currentLevel, 1);
    	faded = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(targetPos, player.transform.position) < radius) 
        {
            if (canFade) {
                canFade = false;
                //renderer.material.shader = transparent;
                
                //StartCoroutine(DoAThingOverTime(material.color, new Color(material.color.r, material.color.g, material.color.b, 0f), 3f));
                SetMaterialTransparent();

                iTween.FadeTo(gameObject, 0, 3.5f);
                StartCoroutine(SetFadedTrueAfterSeconds(3.5f));
            }
            if (faded && canCount) {
            	routine = CountTime(5f);
            	StartCoroutine(routine);
            	canCount = false;
            }
            //when this thing finishes you can count again 

        	//print(Vector3.Distance(targetPos, player.transform.position));
        } else {
        	if (routine != null) {
        		StopCoroutine(routine);
        		canCount = true;
        		routine = null;
        	}
        }
    }
    
}
