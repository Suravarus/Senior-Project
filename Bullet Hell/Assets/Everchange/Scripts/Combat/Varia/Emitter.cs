using ND_VariaBULLET;

using UnityEngine;

namespace Combat.Varia
{
    public class Emitter: MonoBehaviour
    {
        public BasePattern Controller { get; set; }

        void Awake()
        {
            this.Controller = Emitter.GetController(this.transform);
        }

        private void Start()
        {
            Debug.Log(this.Controller.gameObject.name);
        }

        private static BasePattern GetController(Transform em)
        {
            if (em.transform.parent.GetComponent<BasePattern>() != null)
                return em.transform.parent.GetComponent<BasePattern>();
            return GetController(em.transform.parent);
        }
    }
}
