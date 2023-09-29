namespace ArionDigital
{
    using UnityEngine;

    public class CrashCrate : MonoBehaviour
    {
        [Header("Whole Create")]
        public MeshRenderer wholeCrate;
        public BoxCollider boxCollider;
        [Header("Fractured Create")]
        public GameObject fracturedCrate;
        [Header("Audio")]
        public AudioSource crashAudioClip;

        private Animator playerAnim;

        void Start()
        {
            GameObject player = GameObject.FindWithTag("Player");
            playerAnim = player.GetComponent<Animator>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Debug.Log(collision.gameObject);
            if (playerAnim.GetBool("Attacking") == true)
            {
                if (collision.gameObject.tag == "Weapon")
                {
                    destroyed();
                }
            }
        }

        [ContextMenu("Test")]
        public void Test()
        {
            wholeCrate.enabled = false;
            boxCollider.enabled = false;
            fracturedCrate.SetActive(true);
        }

        public void destroyed()
        {
            wholeCrate.enabled = false;
            boxCollider.enabled = false;
            fracturedCrate.SetActive(true);
            crashAudioClip.Play();
        }
    }
}