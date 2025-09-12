using UnityEngine;


namespace BeautifyHDRP_Demo {

    public class SphereAnimator : MonoBehaviour {

        Rigidbody rb;

        void Start() {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate() {
            if (transform.position.z < 1.5f) {
                rb.AddForce(Vector3.forward * 200f * Time.fixedDeltaTime);
            } else if (transform.position.z > 8f) {
                rb.AddForce(Vector3.back * 200f * Time.fixedDeltaTime);
            }
        }
    }
}