using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyApplier : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public Transform[] obstructions;
    public LayerMask layers;
    
    public float fadeDuration = 0.5f;


    private int oldHitsNumber;
    

    void Start()
    {
        oldHitsNumber = 0;
    }
    
    private void LateUpdate()
    {
        viewObstructed();
    }
    
    void viewObstructed()
    {
        float characterDistance = Vector3.Distance(transform.position, player.transform.position);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, player.position - transform.position, characterDistance, layers);
        if (hits.Length > 0)
        {   // Means that some stuff is blocking the view
            int newHits = hits.Length - oldHitsNumber;
            if (obstructions != null && obstructions.Length > 0 && newHits < 0)
            {
                // Repaint all the previous obstructions. Because some of the stuff might be not blocking anymore
                for (int i = 0; i < obstructions.Length; i++)
                {
                    
                    // Renderer renderer = obstructions[i].GetComponent<Renderer>();
                    // Material material = renderer.material;
                    // Color color = material.color;
                    // color.a = 1f; // Set alpha back to 1 (fully opaque)
                    // material.color = color;
                    
                    // obstructions[i].gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
            }
            obstructions = new Transform[hits.Length];
            // Hide the current obstructions 
            for (int i = 0; i < hits.Length; i++)
            {
                
                
                
                
                
                // Transform obstruction = hits[i].transform;
                // // Debug.Log("obstruction object");
                // // Debug.Log(obstruction.gameObject.name);
                //
                //
                // // obstruction.gameObject.GetComponent<MeshRenderer>().material.color.a = 0f;
                // Renderer renderer = obstruction.gameObject.GetComponent<MeshRenderer>();
                //
                // // renderer.materials
                //
                // renderer.material.shader = Shader.Find("Transparent/Diffuse");
                //
                // var color = renderer.material.color;
                // color.a = 0.3f;
                // renderer.material.color = color;

                
                
                
                
                
                
                
                
                
                
                
                
                
                
                // Material material = renderer.material;
                // Color color = material.color;
                // Color color = renderer.material.color;
                // color.a = 0.3f; // Set alpha to 0.3 (70% transparent)
                // obstruction.gameObject.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, color.a);
                // renderer.material.color = color;
                // obstructions[i] = obstruction;


                // Renderer renderer = obstruction.gameObject.GetComponent<Renderer>();
                // Material material = renderer.material;
                // Color color = material.color;
                // color.a = 0.3f; // Set alpha to 0.3 (70% transparent)
                // material.color = color;
                // material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                // material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // // material.SetInt("_ZWrite", 0);
                // material.DisableKeyword("_ALPHATEST_ON");
                // material.EnableKeyword("_ALPHABLEND_ON");
                // material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                // material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                // obstructions[i] = obstruction;



                // Transform obstruction = hits[i].transform;
                // obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                // obstructions[i] = obstruction;
            }
            oldHitsNumber = hits.Length;
        }
        else
        {   // Mean that no more stuff is blocking the view and sometimes all the stuff is not blocking as the same time
            if (obstructions != null && obstructions.Length > 0)
            {
                for (int i = 0; i < obstructions.Length; i++)
                {
                    if (obstructions[i])
                    {
                        // Renderer renderer = obstructions[i].GetComponent<Renderer>();
                        // Material material = renderer.material;
                        // Color color = material.color;
                        // color.a = 1f; // Set alpha back to 1 (fully opaque)
                        // material.color = color;
                        
                        // obstructions[i].gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;    
                    }
                    
                }
                oldHitsNumber = 0;
                obstructions = null;
            }
        }
    }
}